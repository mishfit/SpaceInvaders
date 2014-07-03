using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Linq;

// adapted from Mike Gold's original implimentation by Mish Ochu

namespace SpaceInvaders
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class Mainform : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Timer gameLoopTimer;
		private System.ComponentModel.IContainer components;

		private const int kNumberOfRows = 5;
		private const int kNumberOfTries = 3;
		private const int kNumberOfShields = 4;

		private long TimerCounter = 0;
        
		private int TheSpeed = 6;

		private int TheLevel = 0;


		private int NumberOfMen = 3;
		private Man TheMan = null;
		private Saucer CurrentSaucer = null;
		private bool SaucerStart = false;
		private bool GameGoing = true;
        private List<Bullet> Bullets = new List<Bullet>();
		private Score TheScore = null;
		private HighScore TheHighScore = null;
		private InvaderRow[] InvaderRows = new InvaderRow[6];
		private Shield[] Shields = new Shield[4];
		private InvaderRow TheInvaders = null;

		private int kSaucerInterval = 400;

        private int missileFireCount = 0;
        private bool leftKeyDepressed = false, rightKeyDepressed = false;


		private System.Windows.Forms.MainMenu MainFormMainMenu;
		private System.Windows.Forms.MenuItem MainMenuItemGroup;
		private System.Windows.Forms.MenuItem RestartMenuItem;
		private System.Windows.Forms.MenuItem ExitMenuItem;

        [System.Runtime.InteropServices.DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true, CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        private static extern bool PlaySound(string szSound, System.IntPtr hMod, PlaySoundFlags flags);


		public Mainform()
		{
			InitializeComponent();
			
            
            // reduce flicker
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
		}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.gameLoopTimer.Interval = 30;
            this.gameLoopTimer.Tick += new System.EventHandler(this.OnGameLoopTimerTick);

            
            this.InitializeAllGameObjects(true);
            gameLoopTimer.Start();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this.TheHighScore.Write(this.TheScore.Count);
            Mute();
        }

		private void InitializeAllGameObjects(bool bScore)
		{
			InitializeShields();

			InitializeMan();

			if (bScore)
				InitializeScore();

			InitializeInvaderRows(TheLevel);

			CurrentSaucer = new Saucer(SpaceInvaders.Images.saucer0, SpaceInvaders.Images.saucer1, SpaceInvaders.Images.saucer2);
            


			TheScore.GameOver = false;
			GameGoing = true;
			TheSpeed = 6;
		}

		private void InitializeSaucer()
		{
		  CurrentSaucer.Reset();
		  SaucerStart = true;
		}

		private void InitializeMan()
		{
			TheMan = new Man();
			TheMan.Position.Y = ClientRectangle.Bottom - 50;
			NumberOfMen = 3;
		}

		private void InitializeScore()
		{
			TheScore = new Score(ClientRectangle.Right - 400, 50);
			TheHighScore = new HighScore(ClientRectangle.Left + 25, 50);
			TheHighScore.Read();
		}

		private void InitializeShields()
		{
			for (int i = 0; i < kNumberOfShields; i++)
			{
			  Shields[i] = new Shield();
			  Shields[i].UpdateBounds();
			  Shields[i].Position.X = (Shields[i].GetBounds().Width + 75) * i + 25;
			  Shields[i].Position.Y = ClientRectangle.Bottom - 
							(Shields[i].GetBounds().Height + 75);
			}
		}

		void InitializeInvaderRows(int level)
		{
          InvaderRows[0] = new InvaderRow(Images.invader1, Images.invader1c, 2 + level);
          InvaderRows[1] = new InvaderRow(Images.invader2, Images.invader2c, 3 + level);
		  InvaderRows[2] = new InvaderRow(Images.invader2, Images.invader2c, 4 + level);
		  InvaderRows[3] = new InvaderRow(Images.invader3, Images.invader3c, 5 + level);
		  InvaderRows[4] = new InvaderRow(Images.invader3, Images.invader3c, 6 + level);
		}

		public void PlayASound(string soundFile)
		{
            if (!String.IsNullOrEmpty(soundFile))
                PlaySound(Application.StartupPath + "\\" + soundFile,
                    IntPtr.Zero,
                    PlaySoundFlags.SND_FILENAME | PlaySoundFlags.SND_NODEFAULT | PlaySoundFlags.SND_ASYNC);
            else
                PlaySound(null, IntPtr.Zero, PlaySoundFlags.SND_NODEFAULT | PlaySoundFlags.SND_ASYNC);

		}

        public void PlayASound(System.IO.UnmanagedMemoryStream sound)
        {
            using (System.Media.SoundPlayer player = new System.Media.SoundPlayer(sound))
            {
                player.Load();
                player.Play();
            }
        }

        public void Mute()
        { PlaySound(null, IntPtr.Zero, PlaySoundFlags.SND_SYNC); }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainform));
            this.gameLoopTimer = new System.Windows.Forms.Timer(this.components);
            this.MainFormMainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.MainMenuItemGroup = new System.Windows.Forms.MenuItem();
            this.RestartMenuItem = new System.Windows.Forms.MenuItem();
            this.ExitMenuItem = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // MainFormMainMenu
            // 
            this.MainFormMainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MainMenuItemGroup});
            // 
            // MainMenuItemGroup
            // 
            this.MainMenuItemGroup.Index = 0;
            this.MainMenuItemGroup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.RestartMenuItem,
            this.ExitMenuItem});
            this.MainMenuItemGroup.Text = "File";
            // 
            // RestartMenuItem
            // 
            this.RestartMenuItem.Index = 0;
            this.RestartMenuItem.Text = "Restart...";
            this.RestartMenuItem.Click += new System.EventHandler(this.OnRestartMenuItemClick);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Index = 1;
            this.ExitMenuItem.Text = "Exit";
            this.ExitMenuItem.Click += new System.EventHandler(this.OnExitMenuItemClick);
            // 
            // Mainform
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(672, 622);
            this.KeyPreview = true;
            this.Menu = this.MainFormMainMenu;
            this.Name = "Mainform";
            this.Text = "Space Invaders Game";
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{

			Application.Run(new Mainform());
		}

		private void HandleKeys()
		{

            while (missileFireCount > 0)
            {
                Bullet bullet = new Bullet(20, 30);

                this.Bullets.Add(bullet);

                bullet.Position = TheMan.GetBulletStart();
                PlayASound(Sounds._1);
                missileFireCount--;
            }

            if (leftKeyDepressed && !rightKeyDepressed)
            {
                        TheMan.MoveLeft();
                        Invalidate(TheMan.GetBounds());
                        if (gameLoopTimer.Enabled == false)
                            gameLoopTimer.Start();
            }

            if (!leftKeyDepressed && rightKeyDepressed)
            {
                        TheMan.MoveRight(ClientRectangle.Right);
                        Invalidate(TheMan.GetBounds());
                        if (gameLoopTimer.Enabled == false)
                            gameLoopTimer.Start();
            }
		}


        

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!this.DesignMode && LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                Graphics g = e.Graphics;

                for (int i = 0; i < kNumberOfShields; i++)
                {
                    Shields[i].Draw(g);
                }

                //			g.FillRectangle(Brushes.Black, 0, 0, ClientRectangle.Width, ClientRectangle.Height);
                TheMan.Draw(g);
                TheScore.Draw(g);
                TheHighScore.Draw(g);

                // watch out for multi-thread issues
                foreach (Bullet bullet in this.Bullets)
                    bullet.Draw(g);

                if (SaucerStart)
                {
                    CurrentSaucer.Draw(g);
                }

                for (int i = 0; i < kNumberOfRows; i++)
                {
                    TheInvaders = InvaderRows[i];
                    TheInvaders.Draw(g);
                }
            }
        }

		private int CalculateLargestLastPosition()
		{
			int max = 0;
			for (int i = 0; i < kNumberOfRows; i++)
			{
				TheInvaders = InvaderRows[i];
				int nLastPos = TheInvaders.GetLastInvader().Position.X;
				if (nLastPos > max)
					max = nLastPos;
			}

			return max;
		}

		private int CalculateSmallestFirstPosition()
		{
			int min = 50000;

			try
			{
				for (int i = 0; i < kNumberOfRows; i++)
				{
					TheInvaders = InvaderRows[i];
					int nFirstPos = TheInvaders.GetFirstInvader().Position.X;
					if (nFirstPos < min)
						min = nFirstPos;
				}

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}

			return min;

		}

		private void MoveInvaders()
		{
			bool bMoveDown = false;

			for (int i = 0; i < kNumberOfRows; i++)
			{

				TheInvaders = InvaderRows[i];
				TheInvaders.Move();

			}

//			if (InvaderSoundCounter % 5)
            PlayASound(Sounds._4);


			if ((CalculateLargestLastPosition()) > ClientRectangle.Width - InvaderRows[4][0].GetWidth())
			{
				TheInvaders.DirectionRight = false;
				SetAllDirections(false);
			}

			if ((CalculateSmallestFirstPosition()) < InvaderRows[4][0].Width/3) 
			{
				TheInvaders.DirectionRight = true;
				SetAllDirections(true);
				for (int i = 0; i < kNumberOfRows; i++)
				{
					bMoveDown = true;
				}
			}

			if (bMoveDown)
			{
				for (int i = 0; i < kNumberOfRows; i++)
				{

					TheInvaders = InvaderRows[i];
					TheInvaders.MoveDown();

				}
			}
		}

		private int TotalNumberOfInvaders()
		{
		  int sum = 0;
			for (int i = 0; i < kNumberOfRows; i++)
			{
				TheInvaders = InvaderRows[i];
				sum += TheInvaders.NumberOfLiveInvaders();
			}

			return sum;
		}

		private void MoveInvadersInPlace()
		{
			for (int i = 0; i < kNumberOfRows; i++)
			{
				TheInvaders = InvaderRows[i];
				TheInvaders.MoveInPlace();
			}

		}

		private void SetAllDirections(bool bDirection)
		{
				for (int i = 0; i < kNumberOfRows; i++)
				{
					TheInvaders = InvaderRows[i];
				    TheInvaders.DirectionRight = bDirection;
				}

		}

		public int CalcScoreFromRow(int num)
		{
			int nScore = 10;
			switch (num)
			{
				case 0:
					nScore = 30;
					break;
				case 1:
					nScore = 20;
					break;
				case 2:
					nScore = 20;
					break;
				case 3:
					nScore = 10;
					break;
				case 4:
					nScore = 10;
					break;
			}

			return nScore;
		}

		void TestBulletCollision()
		{
			int rowNum = 0;

            foreach (Bullet b in this.Bullets.ToList())
            {

                // test invader hit first
                for (int i = 0; i < kNumberOfRows; i++)
                {
                    TheInvaders = InvaderRows[i];
                    rowNum = i;

                    int collisionIndex = TheInvaders.CollisionTest(b.GetBounds());

                    if (collisionIndex >= 0)
                    {
                        TheInvaders.Invaders[collisionIndex].BeenHit = true;
                        TheScore.AddScore(CalcScoreFromRow(rowNum));
                        PlayASound(Sounds._0);


                        b.Reset();

                        this.Bullets.Remove(b);
                        continue;
                    }
                }

                if (this.SaucerStart && !this.CurrentSaucer.BeenHit && this.CurrentSaucer.GetBounds().IntersectsWith(b.GetBounds()))
                {
                    this.CurrentSaucer.BeenHit = true;
                    if (!this.CurrentSaucer.ScoreCalculated)
                    {
                        this.TheScore.AddScore(this.CurrentSaucer.CalculateScore());
                        this.PlayASound(Sounds._3);
                    }

                    b.Reset();
                    
                    this.Bullets.Remove(b);
                    continue;
                }
            }
		}

		void TestForLanding()
		{
			for (int i = 0; i < kNumberOfRows; i++)
			{
				TheInvaders = InvaderRows[i];
				if (TheInvaders.AlienHasLanded(ClientRectangle.Bottom))
				{
					TheMan.BeenHit = true;
                    PlayASound(Sounds._2);
					TheScore.GameOver = true;
					TheHighScore.Write(TheScore.Count);
					GameGoing = false;
				}
			}
		  
		}

		void ResetAllBombCounters()
		{
			for (int i = 0; i < kNumberOfRows; i++)
			{
				TheInvaders = InvaderRows[i];
				TheInvaders.ResetBombCounters();
			}
		}

		void TestBombCollision()
		{
			if (TheMan.Died)
			{
			  NumberOfMen --;
				if (NumberOfMen == 0)
				{
					TheHighScore.Write(TheScore.Count);
					TheScore.GameOver = true;
					GameGoing = false;
				}
				else
				{
				  TheMan.Reset();
				  ResetAllBombCounters();
				}
			}

			if (TheMan.BeenHit == true)
				return;

			for (int i = 0; i < kNumberOfRows; i++)
			{
				TheInvaders = InvaderRows[i];
				for (int j = 0; j < TheInvaders.Invaders.Length; j++)
				{
			        for (int k = 0; k < kNumberOfShields; k++)
					{
						bool bulletHole = false;
						if (Shields[k].TestCollision(TheInvaders.Invaders[j].GetBombBounds(), true, out bulletHole))
						{
							TheInvaders.Invaders[j].ResetBombPosition();
							Invalidate(Shields[k].GetBounds());
						}


                        foreach (Bullet b in this.Bullets.ToList())
                        {
                            if (Shields[k].TestCollision(b.GetBounds(), false, out bulletHole))
                            {
                                
                                Invalidate(Shields[k].GetBounds());
                                b.Reset();
                                this.Bullets.Remove(b);

                            }
                        }
					}
				
					if (TheInvaders.Invaders[j].IsBombColliding(TheMan.GetBounds()) )
					{
					  TheMan.BeenHit = true;
                      PlayASound(Sounds._2);
					}
				}
			}
		}


		private int nTotalInvaders = 0;
        private void OnGameLoopTimerTick(object sender, System.EventArgs e)
		{
			HandleKeys();

			TimerCounter++;

			if (GameGoing == false)
			{
				if (TimerCounter % 6 == 0)
					MoveInvadersInPlace();
				Invalidate();
				return;
			}


            foreach (Bullet b in this.Bullets.ToList())
            {
                if (b.Position.Y < 0)
                {
                    this.Bullets.Remove(b);
                    b.Reset();
                }
            }

			if (TimerCounter % kSaucerInterval == 0)
			{
				InitializeSaucer();
                PlayASound(Sounds._8);
				SaucerStart = true;
			}

			if (SaucerStart == true)
			{
				CurrentSaucer.Move();
				if (CurrentSaucer.GetBounds().Left > ClientRectangle.Right)
				{
				  SaucerStart = false;
				}
			}


			if (TimerCounter % TheSpeed == 0)
			{ 
				MoveInvaders();

				nTotalInvaders = TotalNumberOfInvaders();

				if (nTotalInvaders <= 20)
				{
					TheSpeed = 5;
				}

				if (nTotalInvaders <= 10)
				{
					TheSpeed = 4;
				}


				if (nTotalInvaders <= 5)
				{
					TheSpeed = 3;
				}

				if (nTotalInvaders <= 3)
				{
					TheSpeed = 2;
				}

				if (nTotalInvaders <= 1 )
				{
					TheSpeed = 1;
				}

				if (nTotalInvaders == 0)
				{
				 InitializeAllGameObjects(false); // don't initialize score					
				 TheLevel++;
				}


			}

			TestBulletCollision();
			TestBombCollision();


			Invalidate();
			// move invaders

			// move bullets
		}

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Left)
                this.leftKeyDepressed = true;

            if (e.KeyCode == Keys.Right)
                this.rightKeyDepressed = true;

        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == Keys.Left)
                this.leftKeyDepressed = false;

            if (e.KeyCode == Keys.Right)
                this.rightKeyDepressed = false;

            if (e.KeyCode == Keys.Space)
                this.missileFireCount++;


            if (e.Control && e.KeyCode == Keys.C)
                Application.ExitThread();

            if (e.Control && e.KeyCode == Keys.R)
                this.Restart();
        }

        private void OnRestartMenuItemClick(object sender, System.EventArgs e)
		{
            this.Restart();
		}

        private void Restart()
        {
            this.InitializeAllGameObjects(true);
            TheLevel = 0;
        }



        private void OnExitMenuItemClick(object sender, System.EventArgs e)
		{
            Application.ExitThread();
		}
	}
}
