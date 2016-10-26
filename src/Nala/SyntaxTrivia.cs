using System;

namespace Nala
{
    public class SyntaxTrivia : SyntaxNode
    {
        private readonly string _text;

        public SyntaxTrivia(SyntaxKind kind, string text) : base(kind)
        {
            _text = text;
        }

        internal override SyntaxNode GetSlot(int slot)
        {
            throw new InvalidOperationException();
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitTrivia(this);
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitTrivia(this);
        }

        public override string GetValueText()
        {
            return _text;
        }

        public override object GetValue()
        {
            return _text;
        }

        public override string ToString()
        {
            return _text;
        }
    }
}