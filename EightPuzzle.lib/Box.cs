using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace EightPuzzle.lib
{
    public class Box
    {
        public Image Image { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
        
        public int Index { get; set; }

        public Box(string img, int x, int y, int width, int height, int index)
        {
            this.Image = Image.FromFile(img);
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Index = index;
        }

        public Box()
        {
            this.Index = -1;
            this.X = -1;
            this.Y = -1;
            this.Width = -1;
            this.Height = -1;
        }

        public Box(Box box)
        {
            this.Image = box.Image;
            this.X = box.X;
            this.Y = box.Y;
            this.Width = box.Width;
            this.Height = box.Height;
            this.Index = box.Index;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(Image, X, Y, Width, Height);
        }
    }
}
