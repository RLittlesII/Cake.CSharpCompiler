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
                Assert.Equal("/addmodule:\"/Working/netmodule.cs\";\"/Working/netmodule2.cs\" \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/appconfig:\"/Working/app.config\" \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/baseaddress:https://cakebuile.net \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/bugreport:\"/Working/bugreport.xml\" \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/checked \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("\"/Working/cake.cs\"", result.Args);
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
                Assert.Equal($"/checksumalgorithm:{algorithm.ToString().ToLowerInvariant()} \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/codepage:456 \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/debug \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("\"/Working/cake.cs\"", result.Args);
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
                Assert.Equal($"/debug:{debug.ToString().ToLowerInvariant()} \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/define:DEBUG;RELEASE;IPHONE \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/delaysign \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/doc:\"/Working/cake.doc\" \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal($"/errorreport:{errorReport.ToString().ToLowerInvariant()} \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/fullpaths \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/highentropyva \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal($"/keycontainer:key \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal("/keyfile:\"/Working/keystore.snk\" \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal($"/languageversion:{languageVersion} \"/Working/cake.cs\"", result.Args);
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
                Assert.Equal($"/lib:\"/user\",\"/user/source\" \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Link_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Link = new List<FilePath> { "./user.cs", "./user/source.cs" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/link:\"/Working/user.cs\";\"/Working/user/source.cs\" \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Link_Resource_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.LinkResource = new Resource
                {
                    File = "./source.cs",
                    Identifier = "csharp",
                    AccessibilityModifier = "private"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/linkresource:\"/Working/source.cs\" /t:csharp private \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Link_Resource_With_File_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.LinkResource = new Resource
                {
                    File = "./source.cs"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/linkresource:\"/Working/source.cs\" \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Link_Resource_With_Identifier_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.LinkResource = new Resource
                {
                    File = "./source.cs",
                    Identifier = "csharp"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/linkresource:\"/Working/source.cs\" /t:csharp \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Link_Resource_With_Accessibility_Modifier_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.LinkResource = new Resource
                {
                    File = "./source.cs",
                    AccessibilityModifier = "private"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/linkresource:\"/Working/source.cs\" private \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Main_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Main = "Cake.Core";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/main:Cake.Core \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Module_Assembly_Name_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.ModuleAssemblyName = "Cake.Core";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/moduleassemblyname:Cake.Core \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Module_Name_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.ModuleName = "Cake.Core";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/modulename:Cake.Core \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_No_Config_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.NoConfig = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/noconfig \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_No_Logo_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.NoLogo = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/nologo \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_No_Standard_Library_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.NoStandardLibrary = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/nostdlib \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_No_Warning_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.NoWarnings = new List<string> { "SC101", "SC103" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/nowarn:SC101,SC103 \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_No_Win32_Manifest_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.NoWin32Manifest = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/nowin32manifest \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Optimize_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Optimize = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/optimize \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Output_File_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.OutputFile = "./output.dll";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/out:\"/Working/output.dll\" \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Pdb_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Pdb = "./output.pdb";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/pdb:\"/Working/output.pdb\" \"/Working/cake.cs\"", result.Args);
            }

            [Theory]
            [InlineData(Platform.AnyCpu)]
            [InlineData(Platform.AnyCpu32BitPreferred)]
            [InlineData(Platform.ARM)]
            [InlineData(Platform.Itanium)]
            [InlineData(Platform.x64)]
            [InlineData(Platform.x86)]
            public void Should_Add_Platform_If_Provided(Platform platform)
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Platform = platform;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/platform:{platform.ToString().ToLowerInvariant()} \"/Working/cake.cs\"", result.Args);
            }

            [Theory]
            [InlineData("*.cs")]
            [InlineData("./*.cs")]
            [InlineData("./**/*.cs")]
            public void Should_Add_Pattern_Recurse_If_Provided(string pattern)
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Pattern = pattern;
                fixture.Settings.Recurse = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/recurse:{pattern}", result.Args);
            }

            [Theory]
            [InlineData("*./")]
            [InlineData("./cake")]
            [InlineData("cake")]
            public void Should_Add_Directory_Recurse_If_Provided(DirectoryPath directoryPath)
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Directory = directoryPath;
                fixture.Settings.Recurse = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/recurse:\"{directoryPath}\"", result.Args);
            }

            [Fact]
            public void Should_Add_Resource_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Resource = new Resource
                {
                    File = "./source.cs",
                    Identifier = "csharp",
                    AccessibilityModifier = "private"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/resource:\"/Working/source.cs\" /t:csharp private \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Resource_With_File_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Resource = new Resource
                {
                    File = "./source.cs"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/resource:\"/Working/source.cs\" \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Resource_With_Identifier_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Resource = new Resource
                {
                    File = "./source.cs",
                    Identifier = "csharp"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/resource:\"/Working/source.cs\" /t:csharp \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Resource_With_Accessibility_Modifier_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Resource = new Resource
                {
                    File = "./source.cs",
                    AccessibilityModifier = "private"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/resource:\"/Working/source.cs\" private \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Sub_System_Version_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.SubSystemVersion = "1.1";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/subsystemversion:1.1 \"/Working/cake.cs\"", result.Args);
            }

            [Theory]
            [InlineData(TargetFormat.AppContainerExe)]
            [InlineData(TargetFormat.Exe)]
            [InlineData(TargetFormat.Library)]
            [InlineData(TargetFormat.Module)]
            [InlineData(TargetFormat.WinExe)]
            [InlineData(TargetFormat.WinMdObj)]
            public void Should_Add_Target_If_Provided(TargetFormat target)
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Target = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/target:{target.ToString().ToLowerInvariant()} \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_UnSafe_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.UnSafe = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/unsafe \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_UTF8_Output_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.UTF8Output = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/utf8output \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Warning_Level_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.WarningLevel = "0";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/warn:0 \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Warnings_As_Errors_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.WarningsAsErrors = new List<string> { "C175", "G123" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/warnaserror:C175,G123 \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Win_32_Icon_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Win32Icon = "./icon.ico";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/win32icon:\"/Working/icon.ico\" \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Win_32_Manifest_File_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Win32Manifest = "./manifest.xml";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/win32manifest: \"/Working/manifest.xml\" \"/Working/cake.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Win_32_Resource_File_If_Provided()
            {
                // Given
                var fixture = new CakeCompilerFixture();
                fixture.Settings.Win32ResourceFile = "./resource.res";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/win32res:\"/Working/resource.res\" \"/Working/cake.cs\"", result.Args);
            }
        }
    }
}