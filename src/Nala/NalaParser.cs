using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using GosuParser;
using static GosuParser.Parser;
using Nala.Syntax;

namespace Nala
{
    namespace Syntax
    {
        public sealed partial class IdentifierNameSyntax : SimpleNameSyntax
        {
            public override object GetValue()
            {
                return this.identifier.GetValue();
            }

            public override string GetValueText()
            {
                return this.Identifier.GetValueText();
            }
        }
    }
    

    public partial class NalaParser
    {
        public void Parse()
        {
            //cd E:\code\NalaLang\tools\NalaSyntaxGenerator
            //dotnet run Syntax.xml ..\..\src\Nala\Syntax\
            /** 
             * SlidingTextWindow could be used to make a better
             * also look at Lexer.LexSyntaxTrivia
             */
        }

        public static Parser<SyntaxToken> NoneParser => new SyntaxToken(SyntaxKind.None).Return();

        public Parser<SeparatedSyntaxList<TNode>> SeparatedSyntaxListParser<TNode>() where TNode : SyntaxNode
        {
            throw new NotImplementedException();
        }
        
        public static Parser<SyntaxList<SyntaxTrivia>> SyntaxTriviaParser
        {
            get
            {
                var newLine = from x in NewLine
                              select new SyntaxTrivia(SyntaxKind.EndOfLineTrivia, x);

                var spaces = from x in Spaces1
                              select new SyntaxTrivia(SyntaxKind.WhitespaceTrivia, x);

                return SyntaxListParser(newLine.OrElse(spaces));
            }
        }

        public static Parser<SyntaxList<TNode>> SyntaxListParser<TNode>(Parser<TNode> parser) where TNode : SyntaxNode =>
            from items in parser.ZeroOrMore()
            select SyntaxList.List(items.ToList());

        public Parser<SyntaxTokenList> SyntaxTokenListParser
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        public static Parser<SyntaxToken> EndOfFileTokenParser =>
            from _ in EndOfStream
            select new SyntaxToken(SyntaxKind.EndOfFileToken);

        public static Parser<SyntaxToken> IdentifierTokenParser =>
            from firstChar in Satisfy(c => char.IsLetter(c) || c == '_')
            from rest in Satisfy(c => char.IsLetterOrDigit(c) || c == '_').ZeroOrMore()
            select (SyntaxToken)new SyntaxToken.SyntaxIdentifier(firstChar + string.Concat(rest));

        //public Parser<IdentifierNameSyntax> IdentifierNameParser =>
        //    from firstChar in Parser.Satisfy(c => char.IsLetter(c) || c == '_')
        //    from rest in Parser.Satisfy(c => char.IsLetterOrDigit(c) || c == '_').ZeroOrMore()
        //    select SyntaxFactory.IdentifierName(firstChar + string.Concat(rest));

        //public Parser<SimpleNameSyntax> SimpleNameParser =>
        //    from id in IdentifierNameParser
        //    select (SimpleNameSyntax) id;

        //public Parser<NameSyntax> QualifiedNameParser =>
        //    // TODO add generics support
        //    from name in SimpleNameParser
        //    from names in Parser.Char('.').TakeRight(SimpleNameParser).ZeroOrMore()
        //    select names.Aggregate((NameSyntax)name, SyntaxFactory.QualifiedName);

        //public Parser<OpenDirectiveSyntax> OpenDirectiveParser =>
        //    from openKeyword in OpenKeywordParser
        //    from name in QualifiedNameParser
        //    from __ in Parser.NewLine
        //    select SyntaxFactory.OpenDirective(openKeyword, name);

        //public Parser<SyntaxToken> OpenKeywordParser =>
        //    from _ in "open".S1()
        //    select SyntaxFactory.Token(SyntaxKind.OpenKeyword);

        //public Parser<NamespaceDeclarationSyntax> NamespaceDeclarationParser =>
        //    //TODO add errors for names with generics
        //    from _ in "namespace".S1()
        //    from name in QualifiedNameParser.S()
        //    select SyntaxFactory.NamespaceDeclaration(name);


        //public Parser<CompilationUnitSyntax> CompilationUnitParser =>
        //    NotImplementedParser<CompilationUnitSyntax>();

        public Parser<T> NotImplementedParser<T>()
        {
            throw new NotImplementedException();
        }
    }
}