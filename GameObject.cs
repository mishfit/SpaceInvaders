using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceInvaders
{
	/// <summary>
	/// 
	/// </summary>
    public class GameObject
    {
        protected Image TheImage = null;
        public Point Position = new Point(50, 50);
        protected Rectangle ImageBounds = new Rectangle(0, 0, 10, 10);
        protected Rectangle MovingBounds = new Rectangle();

        public GameObject(string fileName)
        {
            TheImage = Image.FromFile(fileName);
            this.Initialize();
        }

        public GameObject(Bitmap image)
        {
            TheImage = image;
            this.Initialize();
        }

        private void Initialize()
        {
            if (this.TheImage == null) return;

            this.ImageBounds.Width = this.TheImage.Width;
            this.ImageBounds.Height = this.TheImage.Height;

        }

        public GameObject()
        {
            TheImage = null;
            this.Initialize();
        }


        public int Width
        {
            get
            {
                return ImageBounds.Width;
            }
        }

        public int Height
        {
            get
            {
                return ImageBounds.Height;
            }
        }


        public virtual int GetWidth()
        {
            return ImageBounds.Width;
        }

        public Image GetImage()
        {
            return TheImage;
        }

        public virtual Rectangle GetBounds()
        {
            return MovingBounds;
        }

        public void UpdateBounds()
        {
            MovingBounds = ImageBounds;
            MovingBounds.Offset(Position);
        }


        public virtual void Draw(Graphics g)
        {
            UpdateBounds();
            g.DrawImage(TheImage, MovingBounds, 0, 0, ImageBounds.Width, ImageBounds.Height, GraphicsUnit.Pixel);
        }


    }
}
