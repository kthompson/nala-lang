using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Nala
{
    public class SyntaxList : SyntaxNode, IEnumerable<SyntaxNode>
    {
        private readonly IList<SyntaxNode> _items;

        internal SyntaxList(IEnumerable<SyntaxNode> items) 
            : base(SyntaxKind.List)
        {
            _items = items.ToList();
            this.SlotCount = _items.Count;
        }

        internal override SyntaxNode GetSlot(int slot)
        {
            return _items[slot];
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.DefaultVisit(this);
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.DefaultVisit(this);
        }

        public IEnumerator<SyntaxNode> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static SyntaxList<TNode> List<TNode>(List<TNode> items) where TNode : SyntaxNode
        {
            if (items.Count == 0)
            {
                return new SyntaxList<TNode>();
            }
            else
            {
                return List(items[0], items);
            }
        }


        private static SyntaxList<TNode> List<TNode>(SyntaxNode creator, List<TNode> items) where TNode : SyntaxNode
        {
            if (items.Count == 0)
            {
                return new SyntaxList<TNode>();
            }

            var newGreen = creator.CreateList(items);
            return new SyntaxList<TNode>(newGreen);
        }

    }

    public class SyntaxList<TNode> : IEnumerable<TNode>
        where TNode: SyntaxNode
    {
        public SyntaxNode Node { get; }
        public int Count { get; }

        public SyntaxList()
            : this(null)
        {
        }

        public SyntaxList(SyntaxNode node)
        {
            Node = node;
            Count = node == null ? 0 : node.IsList ? node.SlotCount : 1;
        }

        /// <summary>
        /// Creates a new list with the specified nodes added at the end.
        /// </summary>
        /// <param name="nodes">The nodes to add.</param>
        public SyntaxList<TNode> AddRange(IEnumerable<TNode> nodes)
        {
            return this.InsertRange(this.Count, nodes);
        }

        /// <summary>
        /// The index of the node in this list, or -1 if the node is not in the list.
        /// </summary>
        public int IndexOf(TNode node)
        {
            var index = 0;
            foreach (var child in this)
            {
                if (object.Equals(child, node))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            if(this.Node == null)
                yield break;

            for (int i = 0; i < this.Node.SlotCount; i++)
            {
                yield return (TNode)this.Node.GetSlot(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Creates a new list with the specified element replaced with new nodes.
        /// </summary>
        /// <param name="nodeInList">The element to replace.</param>
        /// <param name="newNodes">The new nodes.</param>
        public SyntaxList<TNode> ReplaceRange(TNode nodeInList, IEnumerable<TNode> newNodes)
        {
            if (nodeInList == null)
            {
                throw new ArgumentNullException(nameof(nodeInList));
            }

            if (newNodes == null)
            {
                throw new ArgumentNullException(nameof(newNodes));
            }

            var index = this.IndexOf(nodeInList);
            if (index >= 0 && index < this.Count)
            {
                var list = this.ToList();
                list.RemoveAt(index);
                list.InsertRange(index, newNodes);
                return CreateList(list);
            }
            else
            {
                throw new ArgumentException(nameof(nodeInList));
            }
        }

        private static SyntaxList<TNode> CreateList(List<TNode> items)
        {
            if (items.Count == 0)
            {
                return default(SyntaxList<TNode>);
            }
            else
            {
                return CreateList(items[0], items);
            }
        }


        private static SyntaxList<TNode> CreateList(SyntaxNode creator, List<TNode> items)
        {
            if (items.Count == 0)
            {
                return default(SyntaxList<TNode>);
            }

            var newGreen = creator.CreateList(items);
            return new SyntaxList<TNode>(newGreen);
        }


        /// <summary>
        /// Creates a new list with the specified nodes inserted at the index.
        /// </summary>
        /// <param name="index">The index to insert at.</param>
        /// <param name="nodes">The nodes to insert.</param>
        public SyntaxList<TNode> InsertRange(int index, IEnumerable<TNode> nodes)
        {
            if (index < 0 || index > this.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            var list = this.ToList();
            list.InsertRange(index, nodes);

            if (list.Count == 0)
            {
                return this;
            }
            else
            {
                return CreateList(list[0], list);
            }
        }
    }

    ///// <summary>
    ///// A list of <see cref="SyntaxNode"/>.
    ///// </summary>
    //public partial class SyntaxList<TNode> : SyntaxNode, IReadOnlyList<TNode>, IEquatable<SyntaxList<TNode>>
    //    where TNode : SyntaxNode
    //{
    //    private readonly SyntaxNode _node;

    //    internal SyntaxList(SyntaxNode node)
    //        : base(node.Kind)
    //    {
    //        _node = node;
    //    }

    //    internal SyntaxNode Node
    //    {
    //        get
    //        {
    //            return _node;
    //        }
    //    }

    //    /// <summary>
    //    /// The number of nodes in the list.
    //    /// </summary>
    //    public int Count
    //    {
    //        get
    //        {
    //            return _node == null ? 0 : (_node.IsList ? _node.SlotCount : 1);
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the node at the specified index.
    //    /// </summary>
    //    /// <param name="index">The zero-based index of the node to get or set.</param>
    //    /// <returns>The node at the specified index.</returns>
    //    public TNode this[int index]
    //    {
    //        get
    //        {
    //            if (_node != null)
    //            {
    //                if (_node.IsList)
    //                {
    //                    if (unchecked((uint)index < (uint)_node.SlotCount))
    //                    {
    //                        return (TNode)_node.GetSlot(index);
    //                    }
    //                }
    //                else if (index == 0)
    //                {
    //                    return (TNode)_node;
    //                }
    //            }
    //            throw new ArgumentOutOfRangeException();
    //        }
    //    }

    //    internal SyntaxNode ItemInternal(int index)
    //    {
    //        if (_node.IsList)
    //        {
    //            return _node.GetSlot(index);
    //        }

    //        Debug.Assert(index == 0);
    //        return _node;
    //    }

    //    /// <summary>
    //    /// Returns the string representation of the nodes in this list, not including 
    //    /// the first node's leading trivia and the last node's trailing trivia.
    //    /// </summary>
    //    /// <returns>
    //    /// The string representation of the nodes in this list, not including 
    //    /// the first node's leading trivia and the last node's trailing trivia.
    //    /// </returns>
    //    public override string ToString()
    //    {
    //        return _node != null ? _node.ToString() : string.Empty;
    //    }

    //    /// <summary>
    //    /// Creates a new list with the specified node added at the end.
    //    /// </summary>
    //    /// <param name="node">The node to add.</param>
    //    public SyntaxList<TNode> Add(TNode node)
    //    {
    //        return this.Insert(this.Count, node);
    //    }

    //    /// <summary>
    //    /// Creates a new list with the specified nodes added at the end.
    //    /// </summary>
    //    /// <param name="nodes">The nodes to add.</param>
    //    public SyntaxList<TNode> AddRange(IEnumerable<TNode> nodes)
    //    {
    //        return this.InsertRange(this.Count, nodes);
    //    }

    //    /// <summary>
    //    /// Creates a new list with the specified node inserted at the index.
    //    /// </summary>
    //    /// <param name="index">The index to insert at.</param>
    //    /// <param name="node">The node to insert.</param>
    //    public SyntaxList<TNode> Insert(int index, TNode node)
    //    {
    //        if (node == null)
    //        {
    //            throw new ArgumentNullException(nameof(node));
    //        }

    //        return InsertRange(index, new[] { node });
    //    }

    //    /// <summary>
    //    /// Creates a new list with the specified nodes inserted at the index.
    //    /// </summary>
    //    /// <param name="index">The index to insert at.</param>
    //    /// <param name="nodes">The nodes to insert.</param>
    //    public SyntaxList<TNode> InsertRange(int index, IEnumerable<TNode> nodes)
    //    {
    //        if (index < 0 || index > this.Count)
    //        {
    //            throw new ArgumentOutOfRangeException(nameof(index));
    //        }

    //        if (nodes == null)
    //        {
    //            throw new ArgumentNullException(nameof(nodes));
    //        }

    //        var list = this.ToList();
    //        list.InsertRange(index, nodes);

    //        if (list.Count == 0)
    //        {
    //            return this;
    //        }
    //        else
    //        {
    //            return CreateList(list[0], list);
    //        }
    //    }

    //    /// <summary>
    //    /// Creates a new list with the element at specified index removed.
    //    /// </summary>
    //    /// <param name="index">The index of the element to remove.</param>
    //    public SyntaxList<TNode> RemoveAt(int index)
    //    {
    //        if (index < 0 || index > this.Count)
    //        {
    //            throw new ArgumentOutOfRangeException(nameof(index));
    //        }

    //        return this.Remove(this[index]);
    //    }

    //    /// <summary>
    //    /// Creates a new list with the element removed.
    //    /// </summary>
    //    /// <param name="node">The element to remove.</param>
    //    public SyntaxList<TNode> Remove(TNode node)
    //    {
    //        return CreateList(this.Where(x => x != node).ToList());
    //    }

    //    /// <summary>
    //    /// Creates a new list with the specified element replaced with the new node.
    //    /// </summary>
    //    /// <param name="nodeInList">The element to replace.</param>
    //    /// <param name="newNode">The new node.</param>
    //    public SyntaxList<TNode> Replace(TNode nodeInList, TNode newNode)
    //    {
    //        return ReplaceRange(nodeInList, new[] { newNode });
    //    }

    //    /// <summary>
    //    /// Creates a new list with the specified element replaced with new nodes.
    //    /// </summary>
    //    /// <param name="nodeInList">The element to replace.</param>
    //    /// <param name="newNodes">The new nodes.</param>
    //    public SyntaxList<TNode> ReplaceRange(TNode nodeInList, IEnumerable<TNode> newNodes)
    //    {
    //        if (nodeInList == null)
    //        {
    //            throw new ArgumentNullException(nameof(nodeInList));
    //        }

    //        if (newNodes == null)
    //        {
    //            throw new ArgumentNullException(nameof(newNodes));
    //        }

    //        var index = this.IndexOf(nodeInList);
    //        if (index >= 0 && index < this.Count)
    //        {
    //            var list = this.ToList();
    //            list.RemoveAt(index);
    //            list.InsertRange(index, newNodes);
    //            return CreateList(list);
    //        }
    //        else
    //        {
    //            throw new ArgumentException(nameof(nodeInList));
    //        }
    //    }

    //    private static SyntaxList<TNode> CreateList(List<TNode> items)
    //    {
    //        if (items.Count == 0)
    //        {
    //            return default(SyntaxList<TNode>);
    //        }
    //        else
    //        {
    //            return CreateList(items[0], items);
    //        }
    //    }

    //    private static SyntaxList<TNode> CreateList(SyntaxNode creator, List<TNode> items)
    //    {
    //        if (items.Count == 0)
    //        {
    //            return default(SyntaxList<TNode>);
    //        }

    //        var newGreen = creator.CreateList(items);
    //        return new SyntaxList<TNode>(newGreen);
    //    }

    //    /// <summary>
    //    /// The first node in the list.
    //    /// </summary>
    //    public TNode First()
    //    {
    //        return this[0];
    //    }

    //    /// <summary>
    //    /// The first node in the list or default if the list is empty.
    //    /// </summary>
    //    public TNode FirstOrDefault()
    //    {
    //        if (this.Any())
    //        {
    //            return this[0];
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// The last node in the list.
    //    /// </summary>
    //    public TNode Last()
    //    {
    //        return this[this.Count - 1];
    //    }

    //    /// <summary>
    //    /// The last node in the list or default if the list is empty.
    //    /// </summary>
    //    public TNode LastOrDefault()
    //    {
    //        if (this.Any())
    //        {
    //            return this[this.Count - 1];
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// True if the list has at least one node.
    //    /// </summary>
    //    public bool Any()
    //    {
    //        Debug.Assert(_node == null || Count != 0);
    //        return _node != null;
    //    }

    //    // for debugging
    //    private TNode[] Nodes => this.ToArray();

    //    /// <summary>
    //    /// Get's the enumerator for this list.
    //    /// </summary>
    //    public Enumerator GetEnumerator()
    //    {
    //        return new Enumerator(this);
    //    }

    //    IEnumerator<TNode> IEnumerable<TNode>.GetEnumerator()
    //    {
    //        if (this.Any())
    //        {
    //            return new EnumeratorImpl(this);
    //        }

    //        return Enumerable.Empty<TNode>().GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        if (this.Any())
    //        {
    //            return new EnumeratorImpl(this);
    //        }

    //        return Enumerable.Empty<TNode>().GetEnumerator();
    //    }

    //    public static bool operator ==(SyntaxList<TNode> left, SyntaxList<TNode> right)
    //    {
    //        return left._node == right._node;
    //    }

    //    public static bool operator !=(SyntaxList<TNode> left, SyntaxList<TNode> right)
    //    {
    //        return left._node != right._node;
    //    }

    //    public bool Equals(SyntaxList<TNode> other)
    //    {
    //        return _node == other._node;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        return obj is SyntaxList<TNode> && Equals((SyntaxList<TNode>)obj);
    //    }

    //    public override int GetHashCode()
    //    {
    //        return _node?.GetHashCode() ?? 0;
    //    }

    //    public static implicit operator SyntaxList<TNode>(SyntaxList<SyntaxNode> nodes)
    //    {
    //        return new SyntaxList<TNode>(nodes._node);
    //    }

    //    public static implicit operator SyntaxList<SyntaxNode>(SyntaxList<TNode> nodes)
    //    {
    //        return new SyntaxList<SyntaxNode>(nodes.Node);
    //    }

    //    /// <summary>
    //    /// The index of the node in this list, or -1 if the node is not in the list.
    //    /// </summary>
    //    public int IndexOf(TNode node)
    //    {
    //        var index = 0;
    //        foreach (var child in this)
    //        {
    //            if (object.Equals(child, node))
    //            {
    //                return index;
    //            }

    //            index++;
    //        }

    //        return -1;
    //    }

    //    public int IndexOf(Func<TNode, bool> predicate)
    //    {
    //        var index = 0;
    //        foreach (var child in this)
    //        {
    //            if (predicate(child))
    //            {
    //                return index;
    //            }

    //            index++;
    //        }

    //        return -1;
    //    }

    //    internal int IndexOf(SyntaxKind rawKind)
    //    {
    //        var index = 0;
    //        foreach (var child in this)
    //        {
    //            if (child.Kind == rawKind)
    //            {
    //                return index;
    //            }

    //            index++;
    //        }

    //        return -1;
    //    }

    //    public int LastIndexOf(TNode node)
    //    {
    //        for (int i = this.Count - 1; i >= 0; i--)
    //        {
    //            if (object.Equals(this[i], node))
    //            {
    //                return i;
    //            }
    //        }

    //        return -1;
    //    }

    //    public int LastIndexOf(Func<TNode, bool> predicate)
    //    {
    //        for (int i = this.Count - 1; i >= 0; i--)
    //        {
    //            if (predicate(this[i]))
    //            {
    //                return i;
    //            }
    //        }

    //        return -1;
    //    }

    //    internal override SyntaxNode GetSlot(int slot)
    //    {
    //        return this[slot];
    //    }

    //    public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
    //    {
    //        return visitor.DefaultVisit(this);
    //    }

    //    public override void Accept(SyntaxVisitor visitor)
    //    {
    //        visitor.DefaultVisit(this);
    //    }
    //}
}