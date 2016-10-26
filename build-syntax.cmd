@echo off
pushd tools\NalaSyntaxGenerator
dotnet run Syntax.xml ..\..\src\Nala\Syntax
dotnet run Syntax.xml ..\..\test\Nala.Tests\SyntaxTests.cs /test
popd