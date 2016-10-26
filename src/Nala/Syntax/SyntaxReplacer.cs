using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nala.Syntax
{

    internal static class SyntaxReplacer
    {
        internal static SyntaxNode Replace<TNode>(
            SyntaxNode root,
            IEnumerable<TNode> nodes = null,
            Func<TNode, TNode, SyntaxNode> computeReplacementNode = null,
            IEnumerable<SyntaxToken> tokens = null,
            Func<SyntaxToken, SyntaxToken, SyntaxToken> computeReplacementToken = null)
            where TNode : SyntaxNode
        {
            var replacer = new Replacer<TNode>(
                nodes, computeReplacementNode,
                tokens, computeReplacementToken);

            if (replacer.HasWork)
            {
                return replacer.Visit(root);
            }
            else
            {
                return root;
            }
        }

        internal static SyntaxToken Replace(
            SyntaxToken root,
            IEnumerable<SyntaxNode> nodes = null,
            Func<SyntaxNode, SyntaxNode, SyntaxNode> computeReplacementNode = null,
            IEnumerable<SyntaxToken> tokens = null,
            Func<SyntaxToken, SyntaxToken, SyntaxToken> computeReplacementToken = null)
        {
            var replacer = new Replacer<SyntaxNode>(
                nodes, computeReplacementNode,
                tokens, computeReplacementToken);

            if (replacer.HasWork)
            {
                return replacer.VisitToken(root);
            }
            else
            {
                return root;
            }
        }

        private class Replacer<TNode> : SyntaxRewriter where TNode : SyntaxNode
        {
            private readonly Func<TNode, TNode, SyntaxNode> _computeReplacementNode;
            private readonly Func<SyntaxToken, SyntaxToken, SyntaxToken> _computeReplacementToken;

            private readonly HashSet<SyntaxNode> _nodeSet;
            private readonly HashSet<SyntaxToken> _tokenSet;

            public Replacer(
                IEnumerable<TNode> nodes,
                Func<TNode, TNode, SyntaxNode> computeReplacementNode,
                IEnumerable<SyntaxToken> tokens,
                Func<SyntaxToken, SyntaxToken, SyntaxToken> computeReplacementToken)
            {
                _computeReplacementNode = computeReplacementNode;
                _computeReplacementToken = computeReplacementToken;

                _nodeSet = nodes != null ? new HashSet<SyntaxNode>(nodes) : s_noNodes;
                _tokenSet = tokens != null ? new HashSet<SyntaxToken>(tokens) : s_noTokens;
            }

            private static readonly HashSet<SyntaxNode> s_noNodes = new HashSet<SyntaxNode>();
            private static readonly HashSet<SyntaxToken> s_noTokens = new HashSet<SyntaxToken>();

            public bool HasWork
            {
                get
                {
                    return _nodeSet.Count + _tokenSet.Count > 0;
                }
            }
            
            public override SyntaxNode Visit(SyntaxNode node)
            {
                SyntaxNode rewritten = node;

                if (node != null)
                {
                    rewritten = base.Visit(node);

                    if (_nodeSet.Contains(node) && _computeReplacementNode != null)
                    {
                        rewritten = _computeReplacementNode((TNode) node, (TNode) rewritten);
                    }
                }

                return rewritten;
            }

            public override SyntaxToken VisitToken(SyntaxToken token)
            {
                var rewritten = base.VisitToken(token);

                if (_tokenSet.Contains(token) && _computeReplacementToken != null)
                {
                    rewritten = _computeReplacementToken(token, rewritten);
                }

                return rewritten;
            }
        }

        internal static SyntaxNode ReplaceNodeInList(SyntaxNode root, SyntaxNode originalNode, IEnumerable<SyntaxNode> newNodes)
        {
            return new NodeListEditor(originalNode, newNodes, ListEditKind.Replace).Visit(root);
        }

        internal static SyntaxNode InsertNodeInList(SyntaxNode root, SyntaxNode nodeInList, IEnumerable<SyntaxNode> nodesToInsert, bool insertBefore)
        {
            return new NodeListEditor(nodeInList, nodesToInsert, insertBefore ? ListEditKind.InsertBefore : ListEditKind.InsertAfter).Visit(root);
        }

        public static SyntaxNode ReplaceTokenInList(SyntaxNode root, SyntaxToken tokenInList, IEnumerable<SyntaxToken> newTokens)
        {
            return new TokenListEditor(tokenInList, newTokens, ListEditKind.Replace).Visit(root);
        }

        public static SyntaxNode InsertTokenInList(SyntaxNode root, SyntaxToken tokenInList, IEnumerable<SyntaxToken> newTokens, bool insertBefore)
        {
            return new TokenListEditor(tokenInList, newTokens, insertBefore ? ListEditKind.InsertBefore : ListEditKind.InsertAfter).Visit(root);
        }
        
        private enum ListEditKind
        {
            InsertBefore,
            InsertAfter,
            Replace
        }

        private static InvalidOperationException GetItemNotListElementException()
        {
            return new InvalidOperationException(NalaResources.MissingListItem);
        }

        private abstract class BaseListEditor : SyntaxRewriter
        {
            protected readonly ListEditKind editKind;

            protected BaseListEditor(ListEditKind editKind)
            {
                this.editKind = editKind;
            }
            
            public override SyntaxNode Visit(SyntaxNode node)
            {
                SyntaxNode rewritten = node;

                if (node != null)
                {
                    rewritten = base.Visit(node);
                }

                return rewritten;
            }
        }

        private class NodeListEditor : BaseListEditor
        {
            private readonly SyntaxNode _originalNode;
            private readonly IEnumerable<SyntaxNode> _newNodes;

            public NodeListEditor(
                SyntaxNode originalNode,
                IEnumerable<SyntaxNode> replacementNodes,
                ListEditKind editKind)
                : base(editKind)
            {
                _originalNode = originalNode;
                _newNodes = replacementNodes;
            }

            public override SyntaxNode Visit(SyntaxNode node)
            {
                if (node == _originalNode)
                {
                    throw GetItemNotListElementException();
                }

                return base.Visit(node);
            }

            public override SeparatedSyntaxList<TNode> VisitList<TNode>(SeparatedSyntaxList<TNode> list)
            {
                if (_originalNode is TNode)
                {
                    var index = list.IndexOf((TNode)_originalNode);
                    if (index >= 0 && index < list.Count)
                    {
                        switch (this.editKind)
                        {
                            case ListEditKind.Replace:
                                return list.ReplaceRange((TNode)_originalNode, _newNodes.Cast<TNode>());

                            case ListEditKind.InsertAfter:
                                return list.InsertRange(index + 1, _newNodes.Cast<TNode>());

                            case ListEditKind.InsertBefore:
                                return list.InsertRange(index, _newNodes.Cast<TNode>());
                        }
                    }
                }

                return base.VisitList<TNode>(list);
            }

            public override SyntaxList<TNode> VisitList<TNode>(SyntaxList<TNode> list)
            {
                if (_originalNode is TNode)
                {
                    var index = list.IndexOf((TNode)_originalNode);
                    if (index >= 0 && index < list.Count)
                    {
                        switch (this.editKind)
                        {
                            case ListEditKind.Replace:
                                return list.ReplaceRange((TNode)_originalNode, _newNodes.Cast<TNode>());

                            case ListEditKind.InsertAfter:
                                return list.InsertRange(index + 1, _newNodes.Cast<TNode>());

                            case ListEditKind.InsertBefore:
                                return list.InsertRange(index, _newNodes.Cast<TNode>());
                        }
                    }
                }

                return base.VisitList<TNode>(list);
            }
        }

        private class TokenListEditor : BaseListEditor
        {
            private readonly SyntaxToken _originalToken;
            private readonly IEnumerable<SyntaxToken> _newTokens;

            public TokenListEditor(
                SyntaxToken originalToken,
                IEnumerable<SyntaxToken> newTokens,
                ListEditKind editKind)
                : base(editKind)
            {
                _originalToken = originalToken;
                _newTokens = newTokens;
            }

            public override SyntaxToken VisitToken(SyntaxToken token)
            {
                if (token == _originalToken)
                {
                    throw GetItemNotListElementException();
                }

                return base.VisitToken(token);
            }

            public override SyntaxTokenList VisitList(SyntaxTokenList list)
            {
                var index = list.IndexOf(_originalToken);
                if (index >= 0 && index < list.Count)
                {
                    switch (this.editKind)
                    {
                        case ListEditKind.Replace:
                            return list.ReplaceRange(_originalToken, _newTokens);

                        case ListEditKind.InsertAfter:
                            return list.InsertRange(index + 1, _newTokens);

                        case ListEditKind.InsertBefore:
                            return list.InsertRange(index, _newTokens);
                    }
                }

                return base.VisitList(list);
            }
        }
    }
}
