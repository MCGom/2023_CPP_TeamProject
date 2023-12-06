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
using System.IO;

namespace PaintingTool
{

    public partial class Form1 : Form
    {
        private Rectangle Ground;
        private Rectangle Player;
        private Rectangle[] Woods = new Rectangle[3];

        private PictureBox[] WoodsPic = new PictureBox[3];

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

            Player_B = new SolidBrush(Color.Black);
            Woods_B = new SolidBrush(Color.FromArgb(0, 1, 1, 1));

            WoodType = new Random();
            this.TimeLeft.Hide();
            this.ScoreLbl.Hide();


            this.ScoreLbl.BackColor = Color.Transparent;




        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                KeyDownEvent(e);

            }
            else if(e.KeyCode == Keys.Right)
            {
                KeyDownEvent(e);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {           

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush b = new SolidBrush(Color.Black);

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

        private void KeyDownEvent(KeyEventArgs keyCheck)
        {
            if (keyCheck.KeyCode == Keys.Right)
            {
                if (IsLeftClicked == true)
                {
                    Player.Offset(450, 0);
                    IsLeftClicked = false;
                }
            }
            else if (keyCheck.KeyCode == Keys.Left)
            {
                if (IsLeftClicked == false)
                {
                    Player.Offset(-450, 0);
                    IsLeftClicked = true;
                }
            }
            WoodChanged(NowWood);
            this.Invalidate();

            WoodNumbering();
            if (GameOver(Player, Woods[NowWood]))
            {
                TimeControl();
            }
            ScoreLbl.Text = Score + "";
        }

        private void GameStart_Click(object sender, EventArgs e)
        {
            this.GameStart.Hide();

            GameStarted = true;

            IsLeftClicked = true;

            Woods[0] = new Rectangle(190, 400, 370, 200);
            Woods[1] = new Rectangle(20, 200, 370, 200);
            Woods[2] = new Rectangle(190, 0, 370, 200);

            WoodsPic[0] = new PictureBox();
            WoodsPic[1] = new PictureBox();
            WoodsPic[2] = new PictureBox();

            WoodsPic[0].Size = new Size(370, 200);
            WoodsPic[1].Size = new Size(370, 200);
            WoodsPic[2].Size = new Size(370, 200);

            WoodsPic[0].Location = new Point(190, 400);
            WoodsPic[1].Location = new Point(20, 200);
            WoodsPic[2].Location = new Point(190, 0);

            WoodsPic[0].SizeMode = PictureBoxSizeMode.StretchImage;
            WoodsPic[1].SizeMode = PictureBoxSizeMode.StretchImage;
            WoodsPic[2].SizeMode = PictureBoxSizeMode.StretchImage;

            WoodsPicChange("images/TreeRight.png", 0);
            WoodsPicChange("images/TreeLeft.png", 1);
            WoodsPicChange("images/TreeRight.png", 2);


            Controls.Add(WoodsPic[0]);
            Controls.Add(WoodsPic[1]);
            Controls.Add(WoodsPic[2]);

            Player = new Rectangle(50, 500, 50, 100);
            this.TimeLeft.Show();
            this.ScoreLbl.Show();

            this.Invalidate();
            this.Focus();

            TimeLimit = 300;
            TimeControl();

            ScoreLbl.Text = "0";
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if(TimerTime <= 0)
            {
                GameTimer.Stop();
                GameStarted = false;
                NowWood = 0;
                this.Invalidate();
                this.TimeLeft.Hide();
                TimerTime = 0;
                Controls.Remove(WoodsPic[2]);
                Controls.Remove(WoodsPic[1]);
                Controls.Remove(WoodsPic[0]);
                MessageBox.Show("Score : " + Score, "게임 오버", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Score = 0;
                this.GameStart.Show();
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
                        WoodsPic[idx].Location = new Point(190, 0);
                        WoodsPicChange("images/TreeRight.png", idx);
                        Controls.Remove(WoodsPic[idx]);
                        WoodIndexing(idx);
                    }
                    break;
                case 1:
                    {
                        Woods[idx].Location = new Point(20, 0);
                        WoodsPic[idx].Location = new Point(20, 0);
                        WoodsPicChange("images/TreeLeft.png", idx);
                        Controls.Remove(WoodsPic[idx]);
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

                WoodsPic[0].Location = new Point(Woods[0].X, 400);
                WoodsPic[1].Location = new Point(Woods[1].X, 200);

                Controls.Remove(WoodsPic[0]);
                Controls.Remove(WoodsPic[1]);

                Controls.Add(WoodsPic[0]);
                Controls.Add(WoodsPic[1]);
                Controls.Add(WoodsPic[2]);


            }
            else if (idx == 1)
            {
                Woods[2].Location = new Point(Woods[2].X, 400);
                Woods[0].Location = new Point(Woods[0].X, 200);

                WoodsPic[2].Location = new Point(Woods[2].X, 400);
                WoodsPic[0].Location = new Point(Woods[0].X, 200);

                Controls.Remove(WoodsPic[2]);
                Controls.Remove(WoodsPic[0]);

                Controls.Add(WoodsPic[0]);
                Controls.Add(WoodsPic[1]);
                Controls.Add(WoodsPic[2]);

            }
            else
            {
                Woods[1].Location = new Point(Woods[1].X, 400);
                Woods[2].Location = new Point(Woods[2].X, 200);

                WoodsPic[1].Location = new Point(Woods[1].X, 400);
                WoodsPic[2].Location = new Point(Woods[2].X, 200);

                Controls.Remove(WoodsPic[0]);
                Controls.Remove(WoodsPic[1]);

                Controls.Add(WoodsPic[0]);
                Controls.Add(WoodsPic[1]);
                Controls.Add(WoodsPic[2]);

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

        private Boolean GameOver(Rectangle player, Rectangle wood)
        {
            Rectangle isIntersect = Rectangle.Intersect(player, wood);

            if (!isIntersect.IsEmpty)
            {
                GameTimer.Stop();
                GameStarted = false;
                NowWood = 0;
                this.Invalidate();
                this.TimeLeft.Hide();
                TimerTime = 0;
                Controls.Remove(WoodsPic[2]);
                Controls.Remove(WoodsPic[1]);
                Controls.Remove(WoodsPic[0]);
                MessageBox.Show("Score : " + Score, "게임 오버", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Score = 0;
                this.GameStart.Show();


                return true;
            }
            else
            {
                Score++;
                return false;
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


        private void WoodsPicChange(string imagePath, int idx)
        {
            // PictureBox의 이미지 업데이트
            Image newImage = Image.FromFile(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/" + imagePath);
            WoodsPic[idx].Image = newImage;
        }

    }
}
