using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nala.Syntax;

namespace Nala
{
    public partial class SyntaxFactory
    {
        /// <summary>
        /// Creates a token corresponding to a syntax kind. This method can be used for token syntax kinds whose text
        /// can be inferred by the kind alone.
        /// </summary>
        /// <param name="kind">A syntax kind value for a token. These have the suffix Token or Keyword.</param>
        /// <returns></returns>
        public static SyntaxToken Token(SyntaxKind kind)
        {
            return new SyntaxToken(kind);
        }

        /// <summary>
        /// Creates a token with kind IdentifierToken containing the specified text.
        /// <param name="text">The raw text of the identifier name, including any escapes or leading '@'
        /// character.</param>
        /// </summary>
        public static SyntaxToken Identifier(string text)
        {
            return new SyntaxToken.SyntaxIdentifier(text);
        }

        /// <summary>
        /// Creates a token with kind IdentifierToken containing the specified text.
        /// <param name="text">The raw text of the identifier name, including any escapes or leading '@'
        /// character.</param>
        /// </summary>
        public static IdentifierNameSyntax IdentifierName(string text)
        {
            //TODO: make sure the text is good text
            return IdentifierName(Identifier(text));
        }
    }
}
