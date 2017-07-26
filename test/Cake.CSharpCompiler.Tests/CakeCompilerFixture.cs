using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.CSharpCompiler.Tests
{
    internal sealed class CakeCompilerFixture : ToolFixture<CSharpCompilerSettings>
    {
        public FilePath SourceFile { get; set; }

        public CakeCompilerFixture()
            : base("csc.exe")
        {
            SourceFile = new FilePath("./Solution.sln");
            Settings.WorkingDirectory = "/Working";
        }

        protected override void RunTool()
        {
            var tool = new CSharpCompiler(FileSystem, Environment, ProcessRunner, Tools);
            tool.Compile(SourceFile, Settings);
        }
    }
}