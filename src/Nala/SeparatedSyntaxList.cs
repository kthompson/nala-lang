using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Nala
{
    public class SeparatedSyntaxList<TNode> : IEquatable<SeparatedSyntaxList<TNode>>, IReadOnlyList<TNode>
        where TNode : SyntaxNode
    {
        private readonly SyntaxList<SyntaxNode> _list;

        internal SyntaxNode Node => _list.Node;

        public int Count { get; }
        public int SeparatorCount { get; }


        public SeparatedSyntaxList(SyntaxList<SyntaxNode> list)
        {
            Validate(list);

            int allCount = list.Count;
            Count = (allCount + 1) >> 1;
            SeparatorCount = allCount >> 1;

            _list = list;
        }

        [Conditional("DEBUG")]
        private static void Validate(SyntaxList<SyntaxNode> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if ((i & 1) == 0)
                {
                    Debug.Assert(item.IsNode, "Node missing in separated list.");
                }
                else
                {
                    Debug.Assert(item.IsToken, "Separator token missing in separated list.");
                }
            }
        }



        public static bool operator ==(SeparatedSyntaxList<TNode> left, SeparatedSyntaxList<TNode> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SeparatedSyntaxList<TNode> left, SeparatedSyntaxList<TNode> right)
        {
            return !left.Equals(right);
        }

        public bool Equals(SeparatedSyntaxList<TNode> other)
        {
            return _list == other._list;
        }

        public override bool Equals(object obj)
        {
            return (obj is SeparatedSyntaxList<TNode>) && Equals((SeparatedSyntaxList<TNode>)obj);
        }

        public override int GetHashCode()
        {
            return _list.GetHashCode();
        }


        /// <summary>
        /// Creates a new list with the specified node added to the end.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public SeparatedSyntaxList<TNode> Add(TNode node)
        {
            return Insert(this.Count, node);
        }


        /// <summary>
        /// Creates a new list with the specified node inserted at the index.
        /// </summary>
        /// <param name="index">The index to insert at.</param>
        /// <param name="node">The node to insert.</param>
        public SeparatedSyntaxList<TNode> Insert(int index, TNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            return InsertRange(index, new[] { node });
        }

        /// <summary>
        /// Creates a new list with the specified nodes added to the end.
        /// </summary>
        /// <param name="nodes">The nodes to add.</param>
        public SeparatedSyntaxList<TNode> AddRange(IEnumerable<TNode> nodes)
        {
            return InsertRange(this.Count, nodes);
        }

        /// <summary>
        /// Creates a new list with specified element removed.
        /// </summary>
        /// <param name="node">The element to remove.</param>
        public SeparatedSyntaxList<TNode> Remove(TNode node)
        {
            var index = this.IndexOf(node);

            if (index >= 0 && index <= this.Count)
            {
                var nodesWithSeps = this.RemoveAt(index);

                // remove separator too
                if (index < nodesWithSeps.Count && nodesWithSeps[index].IsToken)
                {
                    nodesWithSeps = nodesWithSeps.RemoveAt(index);
                }
                else if (index > 0 && nodesWithSeps[index - 1].IsToken)
                {
                    nodesWithSeps = nodesWithSeps.RemoveAt(index - 1);
                }

                return nodesWithSeps;
            }

            return this;
        }

        /// <summary>
        /// Creates a new <see cref="SyntaxTokenList"/> with the token at the specified index removed.
        /// </summary>
        /// <param name="index">The index of the token to remove.</param>
        public SeparatedSyntaxList<TNode> RemoveAt(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var list = this.ToList();
            list.RemoveAt(index);
            return new SeparatedSyntaxList<TNode>(Node.CreateList(list));
        }

        public SyntaxList<SyntaxNode> GetWithSeparators()
        {
            return _list;
        }

        /// <summary>
        /// Creates a new list with the specified element replaced by the new nodes.
        /// </summary>
        /// <param name="nodeInList">The element to replace.</param>
        /// <param name="newNodes">The new nodes.</param>
        public SeparatedSyntaxList<TNode> ReplaceRange(TNode nodeInList, IEnumerable<TNode> newNodes)
        {
            if (newNodes == null)
            {
                throw new ArgumentNullException(nameof(newNodes));
            }

            var index = this.IndexOf(nodeInList);
            if (index >= 0 && index < this.Count)
            {
                var newNodeList = newNodes.ToList();
                if (newNodeList.Count == 0)
                {
                    return this.Remove(nodeInList);
                }

                var listWithFirstReplaced = this.Replace(nodeInList, newNodeList[0]);

                if (newNodeList.Count > 1)
                {
                    newNodeList.RemoveAt(0);
                    return listWithFirstReplaced.InsertRange(index + 1, newNodeList);
                }

                return listWithFirstReplaced;
            }

            throw new ArgumentOutOfRangeException(nameof(nodeInList));
        }

        /// <summary>
        /// Creates a new list with the specified element replaced by the new node.
        /// </summary>
        /// <param name="nodeInList">The element to replace.</param>
        /// <param name="newNode">The new node.</param>
        public SeparatedSyntaxList<TNode> Replace(TNode nodeInList, TNode newNode)
        {
            if (newNode == null)
            {
                throw new ArgumentNullException(nameof(newNode));
            }

            var index = this.IndexOf(nodeInList);
            if (index >= 0 && index < this.Count)
            {
                var list = this.ToList();
                list.RemoveAt(index);
                list.Insert(index, newNode);
                return new SeparatedSyntaxList<TNode>(Node.CreateList(list));
            }

            throw new ArgumentOutOfRangeException(nameof(nodeInList));
        }


        /// <summary>
        /// Creates a new list with the specified nodes inserted at the index.
        /// </summary>
        /// <param name="index">The index to insert at.</param>
        /// <param name="nodes">The nodes to insert.</param>
        public SeparatedSyntaxList<TNode> InsertRange(int index, IEnumerable<TNode> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            if (index < 0 || index > this.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var nodesWithSeps = this.GetWithSeparators();
            int insertionIndex = index < this.Count ? nodesWithSeps.IndexOf(this[index]) : nodesWithSeps.Count;

            // determine how to deal with separators (commas)
            if (insertionIndex > 0 && insertionIndex < nodesWithSeps.Count)
            {
                var previous = nodesWithSeps[insertionIndex - 1];
                if (previous.IsToken)
                {
                    // pull back so item in inserted before separator
                    insertionIndex--;
                }
            }

            var nodesToInsertWithSeparators = new List<SyntaxNode>();
            foreach (var item in nodes)
            {
                if (item != null)
                {
                    // if item before insertion point is a node, add a separator
                    if (nodesToInsertWithSeparators.Count > 0 || (insertionIndex > 0 && nodesWithSeps[insertionIndex - 1].IsNode))
                    {
                        nodesToInsertWithSeparators.Add(item.CreateSeparator<TNode>(item));
                    }

                    nodesToInsertWithSeparators.Add(item);
                }
            }

            // if item after last inserted node is a node, add separator
            if (insertionIndex < nodesWithSeps.Count && nodesWithSeps[insertionIndex].IsNode)
            {
                var node = nodesWithSeps[insertionIndex];
                nodesToInsertWithSeparators.Add(node.CreateSeparator<TNode>(node)); // separator
            }

            return new SeparatedSyntaxList<TNode>(nodesWithSeps.InsertRange(insertionIndex, nodesToInsertWithSeparators));
        }

        public int IndexOf(TNode node)
        {
            for (int i = 0, n = this.Count; i < n; i++)
            {
                if (object.Equals(this[i], node))
                {
                    return i;
                }
            }

            return -1;
        }

        public TNode this[int index]
        {
            get
            {
                var node = this.Node;
                if (node != null)
                {
                    if (!node.IsList)
                    {
                        if (index == 0)
                        {
                            return (TNode)node;
                        }
                    }
                    else
                    {
                        if (unchecked((uint)index < (uint)this.Count))
                        {
                            return (TNode)node.GetSlot(index << 1);
                        }
                    }
                }

                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}