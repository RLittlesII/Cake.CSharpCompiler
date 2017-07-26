using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.CSharpCompiler.Tests
{
    internal sealed class CakeCompilerFixture : ToolFixture<CSharpCompilerSettings>
    {
        public FilePath SourceFile { get; set; }

        public string Pattern { get; set; }

        public DirectoryPath Directory { get; set; }

        public CakeCompilerFixture()
            : base("csc.exe")
        {
            SourceFile = new FilePath("./cake.cs");
            Pattern = null;
            Settings.WorkingDirectory = "/Working";
        }

        protected override void RunTool()
        {
            var tool = new CSharpCompiler(FileSystem, Environment, ProcessRunner, Tools, Globber);

            if (Pattern != null)
            {
                tool.Compile(Pattern, Settings);
            }
            else if (Directory != null)
            {
                tool.Compile(Directory, Settings);
            }
            else
            {
                tool.Compile(SourceFile, Settings);
            }
        }
    }
}