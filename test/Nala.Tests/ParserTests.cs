using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GosuParser;
using Nala.Syntax;
using Xunit;

namespace Nala.Tests
{
    public class ParserTests
    {
        [Fact]
        public void HelloWorld()
        {
            var contents = @"
object HelloWorld {
  def main(args: Array[String]): Unit = {
    println(""Hello, world!"")
  }
}";

            var result = NalaParser.CompilationUnitParser.Run(contents);
            Assert.NotNull(result);
            var rcus = Assert.IsType<Success<CompilationUnitSyntax>>(result);
            var cus = rcus.Result;
            Assert.NotNull(cus);

            Assert.Collection(cus.Members, syntax =>
            {
                var obj = Assert.IsType<ObjectDeclarationSyntax>(syntax);
            });
        }

        [Fact]
        public void MethodDeclarationTest()
        {
            var contents = @"
object HelloWorld {
  def main(args: Array): Unit = {    
  }
}";

            var result = NalaParser.CompilationUnitParser.Run(contents);
            Assert.NotNull(result);
            var rcus = Assert.IsType<Success<CompilationUnitSyntax>>(result);
            var cus = rcus.Result;
            Assert.NotNull(cus);

            Assert.Collection(cus.Members, syntax =>
            {
                var obj = Assert.IsType<ObjectDeclarationSyntax>(syntax);
            });
        }

        [Fact]
        public void TraitMethodDeclarationTest()
        {
            var contents = @"
trait HelloWorld {
  def main(args: Array[String])
}";

            var result = NalaParser.CompilationUnitParser.Run(contents);
            Assert.NotNull(result);
            var rcus = Assert.IsType<Success<CompilationUnitSyntax>>(result);
            var cus = rcus.Result;
            Assert.NotNull(cus);

            Assert.Collection(cus.Members, syntax =>
            {
                var obj = Assert.IsType<TraitDeclarationSyntax>(syntax);
                Assert.NotNull(obj.Body);
                Assert.Collection(obj.Body.Members, declarationSyntax =>
                {
                    var method = Assert.IsType<MethodDeclarationSyntax>(declarationSyntax);
                });
            });
        }

        [Fact]
        public void QualifiedNamespace()
        {
            var contents = @"
namespace com.theautomaters

object HelloWorld {
  def main(args: Array[String]): Unit = {
  }
}";

            var result = NalaParser.CompilationUnitParser.Run(contents);
            Assert.NotNull(result);
            var rcus = Assert.IsType<Success<CompilationUnitSyntax>>(result);
            var cus = rcus.Result;
            Assert.NotNull(cus);

            Assert.Collection(cus.Members, syntax =>
            {
                var obj = Assert.IsType<ObjectDeclarationSyntax>(syntax);
            });
        }

        [Fact]
        public void Classes()
        {
            var contents = @"class TestClass {
}";

            var result = NalaParser.CompilationUnitParser.Run(contents);
            Assert.NotNull(result);
            var rcus = Assert.IsType<Success<CompilationUnitSyntax>>(result);
            var cus = rcus.Result;
            Assert.NotNull(cus);
            Assert.Collection(cus.Members, member =>
            {
                var klass = Assert.IsType<ClassDeclarationSyntax>(member);
                Assert.NotNull(klass.Name);
                Assert.Equal("TestClass", klass.Name.GetValueText());
            });
        }

        [Fact]
        public void CaseClasses()
        {
            var contents = @"case class TestCaseClass {
}";

            var result = NalaParser.CompilationUnitParser.Run(contents);
            Assert.NotNull(result);
            var rcus = Assert.IsType<Success<CompilationUnitSyntax>>(result);
            var cus = rcus.Result;
            Assert.NotNull(cus);
            Assert.Collection(cus.Members, member =>
            {
                var klass = Assert.IsType<ClassDeclarationSyntax>(member);
                Assert.NotNull(klass.Name);
                Assert.Equal("TestCaseClass", klass.Name.GetValueText());
            });
        }

        [Fact]
        public void CaseClasses1()
        {
            var contents = @"case class TestCaseClass(a: Int, b: String)";

            var result = NalaParser.CompilationUnitParser.Run(contents);
            Assert.NotNull(result);
            var rcus = Assert.IsType<Success<CompilationUnitSyntax>>(result);
            var cus = rcus.Result;
            Assert.NotNull(cus);
            Assert.Collection(cus.Members, member =>
            {
                var klass = Assert.IsType<ClassDeclarationSyntax>(member);
                Assert.NotNull(klass.Name);
                Assert.Equal("TestCaseClass", klass.Name.GetValueText());
            });
        }

        [Fact]
        public void Objects()
        {
            var contents = @"object TestObject {
}";

            var result = NalaParser.CompilationUnitParser.Run(contents);
            Assert.NotNull(result);
            var rcus = Assert.IsType<Success<CompilationUnitSyntax>>(result);
            var cus = rcus.Result;
            Assert.NotNull(cus);
            Assert.Collection(cus.Members, member =>
            {
                var klass = Assert.IsType<ObjectDeclarationSyntax>(member);
                Assert.NotNull(klass.Name);
                Assert.Equal("TestObject", klass.Name.GetValueText());
            });
        }

        [Fact]
        public void Traits()
        {
            var contents = @"trait TestTrait {
}";

            var result = NalaParser.CompilationUnitParser.Run(contents);
            Assert.NotNull(result);
            var rcus = Assert.IsType<Success<CompilationUnitSyntax>>(result);
            var cus = rcus.Result;
            Assert.NotNull(cus);

            Assert.Collection(cus.Members, member =>
            {
                var klass = Assert.IsType<TraitDeclarationSyntax>(member);
                Assert.NotNull(klass.Name);
                Assert.Equal("TestTrait", klass.Name.GetValueText());
            });
        }
    }
}
