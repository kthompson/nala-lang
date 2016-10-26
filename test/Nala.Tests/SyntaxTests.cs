// <auto-generated />

using Nala.Syntax;
using Xunit;

namespace Nala.Tests
{
    public partial class RedNodeTests
    {
        #region Red Generators
        private static IdentifierNameSyntax GenerateIdentifierName()
        {
            return SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("Identifier"));
        }
        
        private static QualifiedNameSyntax GenerateQualifiedName()
        {
            return SyntaxFactory.QualifiedName(GenerateIdentifierName(), SyntaxFactory.Token(SyntaxKind.DotToken), GenerateIdentifierName());
        }
        
        private static CompilationUnitSyntax GenerateCompilationUnit()
        {
            return SyntaxFactory.CompilationUnit(default(NamespaceDeclarationSyntax), new SyntaxList<OpenDirectiveSyntax>(), new SyntaxList<TopLevelMemberDeclarationSyntax>(), SyntaxFactory.Token(SyntaxKind.EndOfFileToken));
        }
        
        private static NamespaceDeclarationSyntax GenerateNamespaceDeclaration()
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.Token(SyntaxKind.NamespaceKeyword), GenerateIdentifierName());
        }
        
        private static OpenDirectiveSyntax GenerateOpenDirective()
        {
            return SyntaxFactory.OpenDirective(SyntaxFactory.Token(SyntaxKind.OpenKeyword), GenerateIdentifierName());
        }
        
        private static TraitDeclarationSyntax GenerateTraitDeclaration()
        {
            return SyntaxFactory.TraitDeclaration(SyntaxFactory.Token(SyntaxKind.TraitKeyword), GenerateIdentifierName(), SyntaxFactory.Token(SyntaxKind.OpenBracketToken), new SyntaxList<BaseMemberDeclarationSyntax>(), SyntaxFactory.Token(SyntaxKind.CloseBracketToken));
        }
        
        private static ObjectDeclarationSyntax GenerateObjectDeclaration()
        {
            return SyntaxFactory.ObjectDeclaration(SyntaxFactory.Token(SyntaxKind.ObjectKeyword), GenerateIdentifierName(), SyntaxFactory.Token(SyntaxKind.OpenBracketToken), new SyntaxList<BaseMemberDeclarationSyntax>(), SyntaxFactory.Token(SyntaxKind.CloseBracketToken));
        }
        
        private static ClassDeclarationSyntax GenerateClassDeclaration()
        {
            return SyntaxFactory.ClassDeclaration(default(SyntaxToken), SyntaxFactory.Token(SyntaxKind.ClassKeyword), GenerateIdentifierName(), SyntaxFactory.Token(SyntaxKind.OpenBracketToken), new SyntaxList<BaseMemberDeclarationSyntax>(), SyntaxFactory.Token(SyntaxKind.CloseBracketToken));
        }
        #endregion Red Generators
        
        #region Red Factory and Property Tests
        [Fact]
        public void TestIdentifierNameFactoryAndProperties()
        {
            var node = GenerateIdentifierName();
            
            Assert.Equal(SyntaxKind.IdentifierToken, node.Identifier.Kind);
            var newNode = node.WithIdentifier(node.Identifier);
            Assert.Equal(node, newNode);
        }
        
        [Fact]
        public void TestQualifiedNameFactoryAndProperties()
        {
            var node = GenerateQualifiedName();
            
            Assert.NotNull(node.Left);
            Assert.Equal(SyntaxKind.DotToken, node.DotToken.Kind);
            Assert.NotNull(node.Right);
            var newNode = node.WithLeft(node.Left).WithDotToken(node.DotToken).WithRight(node.Right);
            Assert.Equal(node, newNode);
        }
        
        [Fact]
        public void TestCompilationUnitFactoryAndProperties()
        {
            var node = GenerateCompilationUnit();
            
            Assert.Null(node.Namespace);
            Assert.NotNull(node.Usings);
            Assert.NotNull(node.Members);
            Assert.Equal(SyntaxKind.EndOfFileToken, node.EndOfFileToken.Kind);
            var newNode = node.WithNamespace(node.Namespace).WithUsings(node.Usings).WithMembers(node.Members).WithEndOfFileToken(node.EndOfFileToken);
            Assert.Equal(node, newNode);
        }
        
        [Fact]
        public void TestNamespaceDeclarationFactoryAndProperties()
        {
            var node = GenerateNamespaceDeclaration();
            
            Assert.Equal(SyntaxKind.NamespaceKeyword, node.NamespaceKeyword.Kind);
            Assert.NotNull(node.Name);
            var newNode = node.WithNamespaceKeyword(node.NamespaceKeyword).WithName(node.Name);
            Assert.Equal(node, newNode);
        }
        
        [Fact]
        public void TestOpenDirectiveFactoryAndProperties()
        {
            var node = GenerateOpenDirective();
            
            Assert.Equal(SyntaxKind.OpenKeyword, node.OpenKeyword.Kind);
            Assert.NotNull(node.Name);
            var newNode = node.WithOpenKeyword(node.OpenKeyword).WithName(node.Name);
            Assert.Equal(node, newNode);
        }
        
        [Fact]
        public void TestTraitDeclarationFactoryAndProperties()
        {
            var node = GenerateTraitDeclaration();
            
            Assert.Equal(SyntaxKind.TraitKeyword, node.TraitKeyword.Kind);
            Assert.NotNull(node.Name);
            Assert.Equal(SyntaxKind.OpenBracketToken, node.OpenBracketToken.Kind);
            Assert.NotNull(node.Members);
            Assert.Equal(SyntaxKind.CloseBracketToken, node.CloseBracketToken.Kind);
            var newNode = node.WithTraitKeyword(node.TraitKeyword).WithName(node.Name).WithOpenBracketToken(node.OpenBracketToken).WithMembers(node.Members).WithCloseBracketToken(node.CloseBracketToken);
            Assert.Equal(node, newNode);
        }
        
        [Fact]
        public void TestObjectDeclarationFactoryAndProperties()
        {
            var node = GenerateObjectDeclaration();
            
            Assert.Equal(SyntaxKind.ObjectKeyword, node.ObjectKeyword.Kind);
            Assert.NotNull(node.Name);
            Assert.Equal(SyntaxKind.OpenBracketToken, node.OpenBracketToken.Kind);
            Assert.NotNull(node.Members);
            Assert.Equal(SyntaxKind.CloseBracketToken, node.CloseBracketToken.Kind);
            var newNode = node.WithObjectKeyword(node.ObjectKeyword).WithName(node.Name).WithOpenBracketToken(node.OpenBracketToken).WithMembers(node.Members).WithCloseBracketToken(node.CloseBracketToken);
            Assert.Equal(node, newNode);
        }
        
        [Fact]
        public void TestClassDeclarationFactoryAndProperties()
        {
            var node = GenerateClassDeclaration();
            
            Assert.Equal(SyntaxKind.None, node.CaseKeyword.Kind);
            Assert.Equal(SyntaxKind.ClassKeyword, node.ClassKeyword.Kind);
            Assert.NotNull(node.Name);
            Assert.Equal(SyntaxKind.OpenBracketToken, node.OpenBracketToken.Kind);
            Assert.NotNull(node.Members);
            Assert.Equal(SyntaxKind.CloseBracketToken, node.CloseBracketToken.Kind);
            var newNode = node.WithCaseKeyword(node.CaseKeyword).WithClassKeyword(node.ClassKeyword).WithName(node.Name).WithOpenBracketToken(node.OpenBracketToken).WithMembers(node.Members).WithCloseBracketToken(node.CloseBracketToken);
            Assert.Equal(node, newNode);
        }
        #endregion Red Factory and Property Tests
        
    }
}