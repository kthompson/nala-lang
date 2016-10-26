using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Nala
{
    /// <summary>
    /// Represents a token in the syntax tree. This is the language agnostic equivalent of <see
    /// cref="T:Microsoft.CodeAnalysis.CSharp.SyntaxToken"/> and <see cref="T:Microsoft.CodeAnalysis.VisualBasic.SyntaxToken"/>.
    /// </summary>
#pragma warning restore RS0010
    public partial class SyntaxToken :  SyntaxNode
    {
        internal SyntaxToken(SyntaxKind kind)
            : base(kind)
        {
        }

        public override bool IsToken => true;

        public virtual string Text
        {
            get { return SyntaxFacts.GetText(this.Kind); }
        }

        public virtual object Value
        {
            get
            {
                switch (this.Kind)
                {
                    case SyntaxKind.TrueKeyword:
                        return true;
                    case SyntaxKind.FalseKeyword:
                        return false;
                    case SyntaxKind.NullKeyword:
                        return null;
                    default:
                        return this.Text;
                }
            }
        }

        public override object GetValue()
        {
            return this.Value;
        }

        public virtual string ValueText => this.Text;

        public override string GetValueText()
        {
            return this.ValueText;
        }

        internal override SyntaxNode GetSlot(int index)
        {
            throw new InvalidOperationException("This program location is thought to be unreachable."); ;
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.DefaultVisit(this);
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.DefaultVisit(this);
        }
    }
}