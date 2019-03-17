using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace EightPuzzle.lib
{
    public class Puzzle
    {
        private Box[,] Boxes;

        public Puzzle()
        {
            LoadBoxes();
        }

        public Puzzle(Puzzle puzzle)
        {
            this.Boxes = new Box[3, 3];
            
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    this.Boxes[i, j] = new Box(puzzle.getBox(i,j));
                }
            }
        }

        private void LoadBoxes()
        {
            Boxes = new Box[3, 3];
            int k = 1;
            int index = 0;
            int width = 81;
            int height = 81;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Boxes[i, j] = new Box(Environment.CurrentDirectory + "\\Images\\" + $"{k++}.png", width * (j + 0), height * (i + 0), width, height, index++);
                }
            }
        }

        private void UpdateLocations()
        {
            int width = 81;
            int height = 81;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Boxes[i, j].X = width * j;
                    Boxes[i, j].Y = height * i;
                }
            }
        }

        public Box getBox(int i, int j)
        {
            return Boxes[i, j];
        }

        public void Swap(int first, int second)
        {
            int firstX = 0, firstY = 0, secondX = 0, secondY = 0;

            firstX = first / 3;
            firstY = first % 3;

            secondX = second / 3;
            secondY = second % 3;

            var temp = Boxes[firstX, firstY];
            this.Boxes[firstX, firstY] = Boxes[secondX, secondY];
            this.Boxes[secondX, secondY] = temp;
            UpdateLocations();
        }

        private Box getSelectedBox(int mouseX, int mouseY)
        {
            int width = this.getBox(0, 0).Width;
            int boxSelectX = 0, boxSelectY = 0;

            boxSelectX = mouseX / width;
            boxSelectY = mouseY / width;

            return this.getBox(boxSelectY, boxSelectX);

        }

        public Box getBoxByIndex(int index)
        {
            foreach(var box in Boxes)
            {
                if (box.Index == index)
                    return box;
            }
            throw new Exception("Invalid index");
        }

        private int[] getBoxIndex(int mouseX, int mouseY)
        {
            int width = this.getBox(0, 0).Width;
            int boxSelectX = 0, boxSelectY = 0;

            boxSelectX = mouseX / width;
            boxSelectY = mouseY / width;

            int[] ret = new int[2];
            ret[0] = boxSelectY;
            ret[1] = boxSelectX;

            return ret;
        }

        public int[] getLocationOfBox(int index)
        {
            int count = 0;

            foreach(var box in Boxes)
            {
                if(box.Index == index)
                {
                    return new int[] { count / 3, count % 3};
                }
                else
                {
                    count++;
                }
            }
            throw new Exception("Invalid input");
        }

        public bool Play(int mouseX, int mouseY)
        {
            Box selectedBox = getSelectedBox(mouseX, mouseY);
            int[] LocationOfSelectedBox = getBoxIndex(mouseX, mouseY);
            int[] LocationOfZeroBox = getBoxIndex(getBoxByIndex(8).X, getBoxByIndex(8).Y);
            Box left, right, top, bottom;

            int first = LocationOfSelectedBox[0] * 3 + LocationOfSelectedBox[1];
            int second = LocationOfZeroBox[0] * 3 + LocationOfZeroBox[1];

            if (LocationOfSelectedBox[1] - 1 < 0)
                left = new Box();
            else
                left = getBox(LocationOfSelectedBox[0], LocationOfSelectedBox[1] - 1);

            if (LocationOfSelectedBox[1] + 1 > 2)
                right = new Box();
            else
                right = Boxes[LocationOfSelectedBox[0], LocationOfSelectedBox[1] + 1];

            if (LocationOfSelectedBox[0] - 1 < 0)
                top = new Box();
            else
                top = Boxes[LocationOfSelectedBox[0] - 1, LocationOfSelectedBox[1]];

            if (LocationOfSelectedBox[0] + 1 > 2)
                bottom = new Box();
            else
                bottom = Boxes[LocationOfSelectedBox[0] + 1, LocationOfSelectedBox[1]];



            if (left.Index == 8 || right.Index == 8 || top.Index == 8 || bottom.Index == 8)
            {
                Swap(first, second);
                return true;
            }
            else
            {
                return false;
            }

        }



        public void Shuffle()
        {
            Random random = new Random();
            int i = random.Next(100,200);

            int width = getBox(0, 0).Width;
            int randomX; 
            int randomY; 

            for (int j = 0; j < i; j++)
            {
                randomX = random.Next(0, width * 3);
                randomY = random.Next(0, width * 3);
                Play(randomX, randomY);
            }

        }

        public bool isFinished()
        {
            int count = 0;
            foreach(var box in Boxes)
            {
                if (box.Index == count)
                {
                    count++;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void DrawPuzzle(Graphics g)
        {
            foreach (var box in Boxes)
            {
                box.Draw(g);
            }
        }

        public Puzzle getPuzzle()
        {
            return this;
        }
    }
}
