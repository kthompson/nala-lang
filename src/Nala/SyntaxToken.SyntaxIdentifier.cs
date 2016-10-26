using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nala
{
    public partial class SyntaxToken
    {
        internal class SyntaxIdentifier : SyntaxToken
        {
            protected readonly string TextField;

            internal SyntaxIdentifier(string text)
                : base(SyntaxKind.IdentifierToken)
            {
                this.TextField = text;
            }

            public override string Text
            {
                get { return this.TextField; }
            }

            public override object Value
            {
                get { return this.TextField; }
            }

            public override string ValueText
            {
                get { return this.TextField; }
            }
        }
    }
}
