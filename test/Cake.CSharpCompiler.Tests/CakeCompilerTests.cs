using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cake.CSharpCompiler.Tests
{
    public sealed class CakeCompilerTests
    {
        public sealed class TheCompileMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentNullException>(result);
            }

            [Fact]
            public void Should_Throw_If_CSharp_Compiler_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("csc: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/csc/csc.exe", "/bin/tools/csc/csc.exe")]
            [InlineData("./tools/csc/csc.exe", "/Working/tools/csc/csc.exe")]
            public void Should_Use_CSharp_Compiler_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_CSharp_Compiler_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/csc.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new CakeCompilerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("csc: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("csc: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Source_File_Null()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.SourceFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("Value cannot be null.\r\nParameter name: sourceFile", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Settings_Null()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("Value cannot be null.\r\nParameter name: settings", result?.Message);
            }

            [Fact]
            public void Should_Add_Modules_To_Args_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Modules = new List<FilePath>
                {
                    "netmodule.cs",
                    "netmodule2.cs"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/addmodule:\"/Working/netmodule.cs\";\"/Working/netmodule2.cs\" \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_App_Configuration_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.AppConfig = new FilePath("./app.config");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/appconfig:\"/Working/app.config\" \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Base_Address_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.BaseAddress = "https://cakebuile.net";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/baseaddress:https://cakebuile.net \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Bug_Report_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.BugReport = "./bugreport.xml";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/bugreport:\"/Working/bugreport.xml\" \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Checked_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Checked = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/checked \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Not_Add_Checked_If_False()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Checked = false;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Solution.sln\"", result.Args);
            }

            [Theory]
            [InlineData(CheckSumAlgorithm.SHA1)]
            [InlineData(CheckSumAlgorithm.SHA256)]
            public void Should_Add_Check_Sum_Algorithm_If_Provided(CheckSumAlgorithm algorithm)
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.CheckSumAlgorithm = algorithm;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/checksumalgorithm:{algorithm.ToString().ToLowerInvariant()} \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Code_Page_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.CodePage = "456";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/codepage:456 \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Debug_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Debug = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/debug \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Not_Add_Debug_If_False()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Debug = false;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Solution.sln\"", result.Args);
            }

            [Theory]
            [InlineData(DebugType.Full)]
            [InlineData(DebugType.PdbOnly)]
            public void Should_Add_Debug_If_Type_Provided(DebugType debug)
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.DebugType = debug;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/debug:{debug.ToString().ToLowerInvariant()} \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Define_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Define = new List<string> { "DEBUG", "RELEASE", "IPHONE" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/define:DEBUG;RELEASE;IPHONE \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Delay_Sign_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.DelaySign = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/delaysign \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Doc_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Doc = "./cake.doc";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/doc:\"/Working/cake.doc\" \"/Working/Solution.sln\"", result.Args);
            }

            [Theory]
            [InlineData(ErrorReport.None)]
            [InlineData(ErrorReport.Prompt)]
            [InlineData(ErrorReport.Queue)]
            [InlineData(ErrorReport.Send)]
            public void Should_Add_Error_Report_If_Provided(ErrorReport errorReport)
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.ErrorReport = errorReport;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/errorreport:{errorReport.ToString().ToLowerInvariant()} \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Full_Paths_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.FullPaths = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/fullpaths \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_High_Entropy_Va_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.HighEntropyVa = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/highentropyva \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Key_Container_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.KeyContainer = "key";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/keycontainer:key \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Key_File_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.KeyFile = "./keystore.snk";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/keyfile:\"/Working/keystore.snk\" \"/Working/Solution.sln\"", result.Args);
            }

            [Theory]
            [InlineData("ISO-1")]
            [InlineData("ISO-2")]
            [InlineData("ISO-3")]
            [InlineData("ISO-4")]
            public void Should_Add_Language_Version_If_Provided(string languageVersion)
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.LanguageVersion = languageVersion;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/languageversion:{languageVersion} \"/Working/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Add_Lib_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Lib = new List<DirectoryPath> { "/user", "/user/source" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/lib:\"/user\",\"/user/source\" \"/Working/Solution.sln\"", result.Args);
            }
        }
    }
}