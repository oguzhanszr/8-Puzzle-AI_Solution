using EightPuzzle.lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EightPuzzle
{
    class Window : Form
    {
        private Puzzle puzzle;
        private Timer timer;

        public Window(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.White;
            this.Paint += Window_Paint;
            this.Click += Window_Click;
            this.KeyDown += Window_KeyDown;
            this.timer = new Timer();
            this.timer.Interval = 300;
            this.timer.Tick += Timer_Tick1;
            puzzle = new Puzzle();

            Form form1 = new Form();
            form1.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            form1.Size = new Size(400,150);
            form1.StartPosition = FormStartPosition.CenterParent;
            Label label = new Label();
            label.AutoSize = true;
            label.Text = "[S] Shuffle the puzzle\n";
            label.Text += "[Space] Show the full solution\n";
            label.Text += "[N] Show the solution step by step\n";
            label.Font = new Font(FontFamily.GenericSansSerif, 15);
            label.Top = 10;
            label.Left = 10;
            form1.Controls.Add(label);
            form1.Show();
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {
            Solver s = new Solver(this.puzzle);
            s.Solve();
            s.ShowSolutionStepByStep();
            Invalidate();
            if (s.Puzzle.isFinished())
                timer.Stop();
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.S)
            {
                puzzle.Shuffle();
                Invalidate();
            }
            if(e.KeyCode == Keys.Space)
            {
                Solver s = new Solver(this.puzzle);
                
                s.Solve();
                //Show the solution
                Form form1 = new Form();
                RichTextBox label = new RichTextBox();
                form1.Size = new Size(800,600);
                label.ScrollBars = RichTextBoxScrollBars.Vertical;
                label.Size = new Size(800,500);
                label.Font = new Font(FontFamily.GenericSansSerif, 12);
                string text = "";

                Stack<PuzzleState> stack = s.ShowSolution();
                int step = 0;
                while(stack.Count != 0)
                {
                    PuzzleState p = stack.Pop();

                    for(int i = 0; i < 3; i++)
                    {
                        for(int j = 0; j < 3; j++)
                        {
                            if (p.State.getBox(i, j).Index + 1 == 9)
                                text += "x";
                            else
                                text += (p.State.getBox(i, j).Index + 1).ToString();
                        }
                        text += "\n";
                    }
                    text += "Step : "+ (++step).ToString() + "-->\n";
                }
                text = text.Trim();
                label.Text = text;
                form1.Controls.Add(label);
                form1.Show();
                Invalidate();
            }
            if(e.KeyCode == Keys.N)
            {
                timer.Start();
            }
        }

        private void Window_Click(object sender, EventArgs e)
        {
            int mouseX = (Control.MousePosition.X - this.Location.X - 8);
            int mouseY = (Control.MousePosition.Y - this.Location.Y - 30);
            

            if(puzzle.Play(mouseX, mouseY))
            {
                Invalidate();

                if (puzzle.isFinished())
                    MessageBox.Show("Game over");
            }
        }
 

        private void Window_Paint(object sender, PaintEventArgs e)
        {
            puzzle.DrawPuzzle(e.Graphics);
        }
    }
}
