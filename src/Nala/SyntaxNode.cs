using System.Collections.Generic;

namespace Nala
{
    public abstract class SyntaxNode
    {
        internal bool IsList => false;

        public SyntaxKind Kind { get; }

        public int SlotCount { get; protected set; }

        internal SyntaxNode(SyntaxKind kind)
        {
            this.Kind = kind;
        }

        public virtual string KindText => this.Kind.ToString();

        public virtual bool IsToken => false;
        public virtual bool IsNode => !IsToken;

        public virtual SyntaxToken CreateSeparator<TNode>(SyntaxNode element) 
            where TNode : SyntaxNode
        {
            return SyntaxFactory.Token(SyntaxKind.CommaToken);
        }

        /// <summary>
        /// Gets node at given node index. 
        /// This WILL force node creation if node has not yet been created.
        /// </summary>
        internal abstract SyntaxNode GetSlot(int slot);


        public virtual object GetValue() { return null; }
        public virtual string GetValueText() { return string.Empty; }
        public abstract TResult Accept<TResult>(SyntaxVisitor<TResult> visitor);

        public abstract void Accept(SyntaxVisitor visitor);

        public SyntaxNode CreateList(IEnumerable<SyntaxNode> list)
        {
            return new SyntaxList(list);
        }
    }
}