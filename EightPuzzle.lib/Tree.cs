using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightPuzzle.lib
{
    public class Tree : IEnumerable<Tree>
    {

        public PuzzleState Data { get; set; }

        public Tree Parent { get; set; }

        public ICollection<Tree> Children { get; set; }


        public Tree(PuzzleState data)
        {
            this.Data = data;
            this.Children = new LinkedList<Tree>();

            this.ElementsIndex = new LinkedList<Tree>();
            this.ElementsIndex.Add(this);
        }

        public Tree AddChild(PuzzleState data)
        {
            Tree childNode = new Tree(data) { Parent = this };
            this.Children.Add(childNode);
            this.RegisterChildForSearch(childNode);

            return childNode;
        }

        public Tree FindTreeNode(Func<Tree, bool> predicate)
        {
            return this.ElementsIndex.FirstOrDefault(predicate);
        }

        private ICollection<Tree> ElementsIndex { get; set; }

        private void RegisterChildForSearch(Tree node)
        {
            ElementsIndex.Add(node);
            if (Parent != null)
                Parent.RegisterChildForSearch(node);
        }

        public bool isRoot()
        {
            return Parent == null;
        }

        public bool isLeaf()
        {
            return Children.Count == 0;
        }

        public IEnumerator<Tree> GetEnumerator()
        {
            yield return this;
            foreach(var directChild in this.Children)
            {
                foreach (var anyChild in directChild)
                    yield return anyChild;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        
    }
}
