#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context, 
							buildSystem: BuildSystem,
							sourceDirectoryPath: "./src",
							title: "Cake.CSharpCompiler",
							repositoryOwner: "RLittlesII",
							repositoryName: "Cake.CSharpCompiler",
							appVeyorAccountName: "RLittlesII",
							shouldPostToGitter: false,
							shouldPostToSlack: false,
							shouldPostToTwitter: false);

BuildParameters.PrintParameters(Context);


ToolSettings.SetToolSettings(context: Context,

                            dupFinderExcludePattern: new string[] { 
                                BuildParameters.RootDirectoryPath + "/src/Cake.CSharpCompiler.Tests/*.cs" },
                            testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* ",
                            testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

Build.RunDotNetCore();