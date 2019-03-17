using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightPuzzle.lib
{
    public class PuzzleState
    {
        public Puzzle State { get; set; }

        //public int MissPlaced { get; set; }

        public int ManhattanDistance { get; set; }

        public int Moves { get; set; }

        public PuzzleState(Puzzle input)
        {
            this.State = input;
        }

        public PuzzleState()
        {
            this.State = new Puzzle();
        }

        public void UpdateManhattanDistance()
        {
            int count = 0;
            int[] current, real = new int[2];
            for (int i = 0; i <= 7; i++) // 8 zero index
            {
                current = State.getLocationOfBox(i);
                real[0] = i / 3;
                real[1] = i % 3;
                count += Math.Abs(current[0] - real[0]) + Math.Abs(current[1] - real[1]);
            }
            this.ManhattanDistance = count;
        }

        public static bool operator ==(PuzzleState left, PuzzleState right)
        {
            bool status = false;


            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (left.State.getBox(i, j).Index == right.State.getBox(i, j).Index)
                    {
                        status = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return status;
        }

        public static bool operator !=(PuzzleState left, PuzzleState right)
        {
            bool status = false;


            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (left.State.getBox(i, j).Index != right.State.getBox(i, j).Index)
                    {
                        status = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return status;
        }
    }
}
