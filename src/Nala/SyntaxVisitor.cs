using System;
using System.Diagnostics;
using Nala.Syntax;

namespace Nala
{
    /// <summary>
    /// Represents a <see cref="NalaSyntaxNode"/> visitor that visits only the single NalaSyntaxNode
    /// passed into its Visit method and produces 
    /// a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of the return value this visitor's Visit method.
    /// </typeparam>
    public abstract partial class SyntaxVisitor<TResult>
    {
        public virtual TResult Visit(SyntaxNode node)
        {
            if (node != null)
            {
                return node.Accept(this);
            }

            // should not come here too often so we will put this at the end of the method.
            return default(TResult);
        }

        public virtual TResult VisitTrivia(SyntaxTrivia node)
        {
            return default(TResult);
        }

        public virtual TResult DefaultVisit(SyntaxNode node)
        {
            return default(TResult);
        }
    }

    /// <summary>
    /// Represents a <see cref="NalaSyntaxNode"/> visitor that visits only the single CSharpSyntaxNode
    /// passed into its Visit method.
    /// </summary>
    public abstract partial class SyntaxVisitor
    {
        public virtual void Visit(SyntaxNode node)
        {
            if (node != null)
            {
                node.Accept(this);
            }
        }

        public virtual void VisitTrivia(SyntaxTrivia node)
        {
        }

        public virtual void DefaultVisit(SyntaxNode node)
        {
        }
    }

    /// <summary>
    /// Represents a <see cref="SyntaxVisitor{TResult}"/> which descends an entire <see cref="SyntaxNode"/> graph and
    /// may replace or remove visited SyntaxNodes in depth-first order.
    /// </summary>
    public abstract partial class SyntaxRewriter : SyntaxVisitor<SyntaxNode>
    {
        private int _recursionDepth;

        public override SyntaxNode Visit(SyntaxNode node)
        {
            if (node != null)
            {
                _recursionDepth++;
                //StackGuard.EnsureSufficientExecutionStack(_recursionDepth);

                var result = ((NalaSyntaxNode)node).Accept(this);

                _recursionDepth--;
                return result;
            }
            else
            {
                return null;
            }
        }

        public virtual SyntaxToken VisitToken(SyntaxToken token)
        {
            return token;
        }

        public virtual SyntaxList VisitList(SyntaxList list)
        {
            throw new NotImplementedException();
        }

        public virtual SyntaxList<TNode> VisitList<TNode>(SyntaxList<TNode> list) where TNode : SyntaxNode
        {
            //SyntaxListBuilder alternate = null;
            //for (int i = 0, n = list.Count; i < n; i++)
            //{
            //    var item = list[i];
            //    var visited = this.VisitListElement(item);
            //    if (item != visited && alternate == null)
            //    {
            //        alternate = new SyntaxListBuilder(n);
            //        alternate.AddRange(list, 0, i);
            //    }

            //    if (alternate != null && visited != null && !visited.IsKind(SyntaxKind.None))
            //    {
            //        alternate.Add(visited);
            //    }
            //}

            //if (alternate != null)
            //{
            //    return alternate.ToList();
            //}

            //return list;

            throw new NotImplementedException();
        }

        public virtual TNode VisitListElement<TNode>(TNode node) where TNode : SyntaxNode
        {
            return (TNode)(SyntaxNode)this.Visit(node);
        }

        public virtual SeparatedSyntaxList<TNode> VisitList<TNode>(SeparatedSyntaxList<TNode> list)
            where TNode : SyntaxNode
        {
            //    var count = list.Count;
            //    var sepCount = list.SeparatorCount;

            //    SeparatedSyntaxListBuilder<TNode> alternate = default(SeparatedSyntaxListBuilder<TNode>);

            //    int i = 0;
            //    for (; i < sepCount; i++)
            //    {
            //        var node = list[i];
            //        var visitedNode = this.VisitListElement(node);

            //        var separator = list.GetSeparator(i);
            //        var visitedSeparator = this.VisitListSeparator(separator);

            //        if (alternate.IsNull)
            //        {
            //            if (node != visitedNode || separator != visitedSeparator)
            //            {
            //                alternate = new SeparatedSyntaxListBuilder<TNode>(count);
            //                alternate.AddRange(list, i);
            //            }
            //        }

            //        if (!alternate.IsNull)
            //        {
            //            if (visitedNode != null)
            //            {
            //                alternate.Add(visitedNode);

            //                if (visitedSeparator.Kind == SyntaxKind.None)
            //                {
            //                    throw new InvalidOperationException(NalaResources.SeparatorIsExpected);
            //                }

            //                alternate.AddSeparator(visitedSeparator);
            //            }
            //            else
            //            {
            //                if (visitedNode == null)
            //                {
            //                    throw new InvalidOperationException(NalaResources.ElementIsExpected);
            //                }
            //            }
            //        }
            //    }

            //    if (i < count)
            //    {
            //        var node = list[i];
            //        var visitedNode = this.VisitListElement(node);

            //        if (alternate.IsNull)
            //        {
            //            if (node != visitedNode)
            //            {
            //                alternate = new SeparatedSyntaxListBuilder<TNode>(count);
            //                alternate.AddRange(list, i);
            //            }
            //        }

            //        if (!alternate.IsNull && visitedNode != null)
            //        {
            //            alternate.Add(visitedNode);
            //        }
            //    }

            //    if (!alternate.IsNull)
            //    {
            //        return alternate.ToList();
            //    }

            //    return list;
            throw new NotImplementedException();
        }

        public virtual SyntaxToken VisitListSeparator(SyntaxToken separator)
        {
            return this.VisitToken(separator);
        }

        public virtual SyntaxTokenList VisitList(SyntaxTokenList list)
        {
            //SyntaxTokenListBuilder alternate = null;
            //var count = list.Count;
            //var index = -1;

            //foreach (var item in list)
            //{
            //    index++;
            //    var visited = this.VisitToken(item);
            //    if (item != visited && alternate == null)
            //    {
            //        alternate = new SyntaxTokenListBuilder(count);
            //        alternate.Add(list, 0, index);
            //    }

            //    if (alternate != null && visited.Kind != SyntaxKind.None) //skip the null check since SyntaxToken is a value type
            //    {
            //        alternate.Add(visited);
            //    }
            //}

            //if (alternate != null)
            //{
            //    return alternate.ToList();
            //}

            //return list;
            throw new NotImplementedException();
        }
    }
}