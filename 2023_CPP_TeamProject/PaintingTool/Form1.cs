using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Forms.VisualStyles;

namespace PaintingTool
{

    public partial class Form1 : Form
    {
        private Rectangle Ground;
        private Rectangle Player;
        private Rectangle[] Woods = new Rectangle[3];

        private Brush Player_B;
        private Brush Woods_B;


        private Boolean GameStarted = false;
        private Boolean IsLeftClicked = true;

        private Random WoodType;

        private int NowWood = 0;
        private int Score = 0;
        private int TimeLimit = 300;
        private int TimerTime = 0;
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            Size = new Size(600, 800);

            Ground = new Rectangle(0, 600, 600, 200);

            Player_B = new SolidBrush(Color.Orange);
            Woods_B = new SolidBrush(Color.SaddleBrown);

            WoodType = new Random();
            this.TimeLeft.Hide();




        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                if (IsLeftClicked == false)
                {
                    Player.Offset(-450, 0);
                    IsLeftClicked = true;
                }
                WoodChanged(NowWood);
                this.Invalidate();
                
                WoodNumbering();
                GameOver(Player, Woods[NowWood]);
                TimeControl();


            }
            else if(e.KeyCode == Keys.Right)
            {
                if (IsLeftClicked == true)
                {
                    Player.Offset(450, 0);
                    IsLeftClicked = false;
                }
                WoodChanged(NowWood);
                this.Invalidate();
                
                WoodNumbering();
                GameOver(Player, Woods[NowWood]);
                TimeControl();

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {           

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush b = new SolidBrush(Color.Green);

            g.FillRectangle(b, Ground);
            if(GameStarted == true)
            {
                for(int i =0; i < 3; i++)
                {
                    g.FillRectangle(Woods_B, Woods[i]);
                }
                g.FillRectangle(Player_B, Player);
            }

        }

        private void GameStart_Click(object sender, EventArgs e)
        {
            this.GameStart.Hide();

            GameStarted = true;

            IsLeftClicked = true;

            Woods[0] = new Rectangle(190, 400, 370, 200);
            Woods[1] = new Rectangle(20, 200, 370, 200);
            Woods[2] = new Rectangle(190, 0, 370, 200);

            Player = new Rectangle(50, 500, 50, 100);
            this.TimeLeft.Show();
            this.Invalidate();
            this.Focus();

            TimeLimit = 300;
            TimeControl();
           
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if(TimerTime <= 0)
            {
                GameTimer.Stop();
                GameStarted = false;
                NowWood = 0;
                this.Invalidate();
                this.GameStart.Show();
                this.TimeLeft.Hide();
                TimerTime = 0;
                MessageBox.Show("Score : " + Score, "게임 오버", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Score = 0;
            }
            else
            {
                TimeLeft.Text = TimerTime / 10 + "." + TimerTime % 10;
                TimerTime--;
            }
        }

        private void WoodChanged(int idx)
        {
            int WoodTypeTmp = WoodType.Next(2);

            switch(WoodTypeTmp)
            {
                case 0:
                    {
                        Woods[idx].Location = new Point(190, 0);
                        WoodIndexing(idx);
                    }
                    break;
                case 1:
                    {
                        Woods[idx].Location = new Point(20, 0);
                        WoodIndexing(idx);
                    }
                    break;
            }
        }
        private void WoodIndexing(int idx)
        {
            if (idx == 2)
            {
                Woods[0].Location = new Point(Woods[0].X, 400);
                Woods[1].Location = new Point(Woods[1].X, 200);
            }
            else if (idx == 1)
            {
                Woods[2].Location = new Point(Woods[2].X, 400);
                Woods[0].Location = new Point(Woods[0].X, 200);
            }
            else
            {
                Woods[1].Location = new Point(Woods[1].X, 400);
                Woods[2].Location = new Point(Woods[2].X, 200);
            }
        }

        private void WoodNumbering()
        {
            if(NowWood != 2)
            {
                NowWood++;
            }
            else
            {
                NowWood = 0;
            }
        }

        private void GameOver(Rectangle player, Rectangle wood)
        {
            Rectangle isIntersect = Rectangle.Intersect(player, wood);

            if (!isIntersect.IsEmpty)
            {
                GameTimer.Stop();
                GameStarted = false;
                NowWood = 0;
                this.Invalidate();
                this.GameStart.Show();
                this.TimeLeft.Hide();
                TimerTime = 0;
                MessageBox.Show("Score : " + Score, "게임 오버", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Score = 0;
            }
            else
            {
                Score++;
            }
        }

        private void TimeControl()
        {
            GameTimer.Stop();
            if (TimeLimit > 5)
            {
                TimeLimit -= 5;
            }
            else
            {
                TimeLimit = 2;
            }
            TimerTime = TimeLimit;
            TimeLeft.Text = TimerTime / 10  + "." + TimerTime % 10;
            GameTimer.Start();
        }
    }
}
