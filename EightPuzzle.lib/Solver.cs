using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EightPuzzle.lib
{
    public class Solver
    {
        public Puzzle Puzzle { get; set; }

        private Puzzle tempPuzzle { get; set; }

        private Tree Tree { get; set; }

        private Queue<PuzzleState> PriorityQueue { get; set; }

        public Solver(Puzzle puzzle)
        {
            this.Puzzle = puzzle;
            this.tempPuzzle = new Puzzle(puzzle);
            this.Tree = new Tree(new PuzzleState(this.Puzzle));
            this.PriorityQueue = new Queue<PuzzleState>();
            this.PriorityQueue.Enqueue(new PuzzleState(this.Puzzle));
        }

        private Tree lastChild { get; set; }

        public void Solve()
        {
            if (Puzzle.isFinished())
            {
                return;
            }

            Queue<PuzzleState> tempQueue = new Queue<PuzzleState>();
            Queue<PuzzleState> lastLevelNodes = new Queue<PuzzleState>();

            while (true)
            {
                //Critical optimization
                for (int i = 1; i <= this.PriorityQueue.Count; i++)
                {
                    tempQueue.Enqueue(PriorityQueue.Peek());
                    lastLevelNodes.Enqueue(PriorityQueue.Peek());

                    PuzzleState q = PriorityQueue.Dequeue();
                    List<PuzzleState> allMoves = FindPossibleMoves(q.State);

                    foreach (var move in allMoves)
                    {
                        Tree currNode = Tree.FindTreeNode(node => node.Data == move);
                        if (currNode == null)
                        {
                            lastChild = Tree.FindTreeNode(node => node.Data.State == q.State).AddChild(move);
                            move.UpdateManhattanDistance();
                        }
                    }
                }

                int minDistance = 100;
                for (int i = 1; i <= tempQueue.Count; i++)
                {
                    PuzzleState p = tempQueue.Dequeue();
                    minDistance = Math.Min(FindMinDistance(Tree.FindTreeNode(node => node.Data == p)), minDistance);
                }

                if (minDistance == 0)
                    break;

                for (int i = 1; i <= lastLevelNodes.Count; i++)
                {
                    PuzzleState p = lastLevelNodes.Dequeue();
                    Tree parent = Tree.FindTreeNode(node => node.Data.State == p.State);

                    foreach (var child in parent.Children)
                    {
                        if (child.Data.ManhattanDistance == minDistance)
                        {
                            PriorityQueue.Enqueue(child.Data);
                        }
                    }
                }
            }
        }

        public Stack<PuzzleState> ShowSolution()
        {
            Tree finishNode = Tree.FindTreeNode(node => node.Data.State.isFinished());
            Stack<PuzzleState> solution = new Stack<PuzzleState>();
            Stack<PuzzleState> value = new Stack<PuzzleState>();

            while (finishNode.Parent != null)
            {
                solution.Push(finishNode.Data);
                value.Push(finishNode.Data);
                finishNode = finishNode.Parent;
            }


            while (solution.Count != 0)
            {
                Puzzle p1 = solution.Pop().State;
                play(p1);
            }

            return value;
        }

        private static int stepCount = 0;
        private static Stack<PuzzleState> solution;
        public void ShowSolutionStepByStep()
        {
            if (stepCount == 0)
            {
                solution = new Stack<PuzzleState>();
                Tree finishNode = Tree.FindTreeNode(node => node.Data.State.isFinished());

                while (finishNode.Parent != null)
                {
                    solution.Push(finishNode.Data);
                    finishNode = finishNode.Parent;
                }
                stepCount++;

                if(solution.Count != 0)
                    play(solution.Pop().State);
            }
            else if (this.Puzzle.isFinished())
            {
                stepCount = 0;
                return;
            }
            else
            {
                stepCount++;

                if(solution.Count == 0)
                {
                    stepCount = 0;
                    solution = new Stack<PuzzleState>();
                    return;
                }

                Puzzle p1 = solution.Pop().State;
                play(p1);
                
            }

        }

        private void play(Puzzle p1)
        {
            int sub = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sub = p1.getBox(i, j).Index - this.Puzzle.getBox(i, j).Index;
                    if (sub != 0)
                    {
                        if (p1.getBox(i, j).Index == 8)
                        {

                        }
                        else
                        {
                            this.Puzzle.Play(p1.getBox(i, j).X, p1.getBox(i, j).Y);
                        }
                        if (this.Puzzle.getBox(i, j).Index == 8)
                        {

                        }
                        else
                        {
                            this.Puzzle.Play(this.Puzzle.getBox(i, j).X, this.Puzzle.getBox(i, j).Y);
                        }
                    }
                }
            }
        }

        private int FindMinDistance(Tree parent)
        {
            int minDistance = 100;

            foreach (var children in parent.Children)
            {
                if (minDistance > children.Data.ManhattanDistance)
                    minDistance = children.Data.ManhattanDistance;
            }

            return minDistance;
        }


        private List<PuzzleState> FindPossibleMoves(Puzzle tempPuzzle)
        {
            List<PuzzleState> list = new List<PuzzleState>();
            Box left, right, top, bottom;
            Puzzle temp;
            int[] zeroBox = tempPuzzle.getLocationOfBox(8);

            if (!(zeroBox[1] - 1 < 0))
            {
                left = tempPuzzle.getBox(zeroBox[0], zeroBox[1] - 1);
                temp = new Puzzle(tempPuzzle.getPuzzle());
                temp.Play(left.X, left.Y);
                list.Add(new PuzzleState(new Puzzle(temp)));
            }

            if (!(zeroBox[1] + 1 > 2))
            {
                right = tempPuzzle.getBox(zeroBox[0], zeroBox[1] + 1);
                temp = new Puzzle(tempPuzzle.getPuzzle());
                temp.Play(right.X, right.Y);
                list.Add(new PuzzleState(new Puzzle(temp)));
            }

            if (!(zeroBox[0] - 1 < 0))
            {
                top = tempPuzzle.getBox(zeroBox[0] - 1, zeroBox[1]);
                temp = new Puzzle(tempPuzzle.getPuzzle());
                temp.Play(top.X, top.Y);
                list.Add(new PuzzleState(new Puzzle(temp)));
            }

            if (!(zeroBox[0] + 1 > 2))
            {
                bottom = tempPuzzle.getBox(zeroBox[0] + 1, zeroBox[1]);
                temp = new Puzzle(tempPuzzle.getPuzzle());
                temp.Play(bottom.X, bottom.Y);
                list.Add(new PuzzleState(new Puzzle(temp)));
            }


            return list;
        }
    }
}
