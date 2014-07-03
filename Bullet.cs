using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceInvaders
{
	/// <summary>
	/// Represents a missile fired at the Invaders
	/// </summary>
    [System.Diagnostics.DebuggerDisplay("Bullet Position={Position}")]
	public class Bullet : GameObject
	{
		const int kBulletInterval = 20;
		public int BulletInterval = kBulletInterval;

        private bool IsReset = false;

		public Bullet(int x, int y)
		{
			ImageBounds.Width = 5;
			ImageBounds.Height = 15;
			Position.X = x;
			Position.Y = y;
		}

		public void Reset()
		{
            this.IsReset = true;

			if (Mainform.ActiveForm != null)
			{
                Mainform.ActiveForm.Paint += new System.Windows.Forms.PaintEventHandler(OnActiveFormPaint);
			}

			BulletInterval = kBulletInterval;
		}

        void OnActiveFormPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (Mainform.ActiveForm != null)
            {
                Mainform.ActiveForm.Paint -= new System.Windows.Forms.PaintEventHandler(OnActiveFormPaint);
                e.Graphics.FillRectangle(Brushes.Black, this.MovingBounds);
            }
        }

		public void Slow()
		{
//		  BulletInterval = 3;
		}


		public override void Draw(Graphics g)
		{
            if (this.IsReset || g == null) return;

			UpdateBounds();
			g.FillRectangle(Brushes.Chartreuse , MovingBounds);
			Position.Y -= BulletInterval;
		}

        public Rectangle GetCollisionBounds()
        {
            Rectangle rect = this.GetBounds();

            return new Rectangle(new Point(rect.Location.X, rect.Location.Y - rect.Height), new Size(rect.Width, rect.Height * 2));
        }
	}
}
