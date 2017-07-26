using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Cake.CSharpCompiler.Tests")]

namespace Cake.CSharpCompiler
{
    /// <summary>
    /// csc tool execution.
    /// </summary>
    /// <seealso cref="CSharpCompilerSettings" />
    internal class CSharpCompiler : Tool<CSharpCompilerSettings>
    {
        private readonly IGlobber _globber;

        /// <summary>
        /// Gets or sets the cake environment.
        /// </summary>
        public ICakeEnvironment Environment { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpCompiler"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="globber">The globber.</param>
        public CSharpCompiler(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, tools)
        {
            _globber = globber;
            Environment = environment;
        }

        /// <summary>
        /// Compiles the specified source file.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="settings">The settings.</param>
        public void Compile(FilePath sourceFile, CSharpCompilerSettings settings)
        {
            if (sourceFile == null)
            {
                throw new ArgumentNullException(nameof(sourceFile));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(sourceFile, settings));
        }

        public void Compile(string pattern, CSharpCompilerSettings settings)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (_globber.Match(pattern).Any())
            {
                throw new ArgumentOutOfRangeException(nameof(pattern));
            }

            Run(settings, GetArguments(pattern, settings));
        }

        public void Compile(DirectoryPath directoryPath, CSharpCompilerSettings settings)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(directoryPath, settings));
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>
        /// The tool executable name.
        /// </returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "csc.exe" };
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>
        /// The name of the tool.
        /// </returns>
        protected override string GetToolName()
        {
            return "csc";
        }

        private ProcessArgumentBuilder GetArguments(FilePath sourceFile, CSharpCompilerSettings settings)
        {
            var builder = GetArguments(settings);

            builder.AppendQuoted($"{sourceFile.MakeAbsolute(Environment).FullPath}");

            return builder;
        }

        private ProcessArgumentBuilder GetArguments(string pattern, CSharpCompilerSettings settings)
        {
            var builder = GetArguments(settings);

            if (settings.Recurse)
            {
                builder.AppendSwitch($"/{nameof(settings.Recurse).ToLowerInvariant()}", ":", $"{pattern}");
            }

            return builder;
        }

        private ProcessArgumentBuilder GetArguments(DirectoryPath directoryPath, CSharpCompilerSettings settings)
        {
            var builder = GetArguments(settings);

            if (settings.Recurse)
            {
                builder.AppendSwitchQuoted($"/{nameof(settings.Recurse).ToLowerInvariant()}", ":", $"{directoryPath.MakeAbsolute(Environment).FullPath}");
            }

            return builder;
        }

        private ProcessArgumentBuilder GetArguments(CSharpCompilerSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (settings.Modules.Any())
            {
                var moduleString = FilePaths(settings.Modules);

                builder.AppendSwitch("/addmodule", ":", moduleString);
            }

            if (settings.AppConfig != null)
            {
                builder.AppendSwitchQuoted($"/{nameof(settings.AppConfig).ToLowerInvariant()}", ":", $"{settings.AppConfig.MakeAbsolute(Environment).FullPath}");
            }

            if (!string.IsNullOrEmpty(settings.BaseAddress))
            {
                builder.AppendSwitch($"/{nameof(settings.BaseAddress).ToLowerInvariant()}", ":", $"{settings.BaseAddress}");
            }

            if (settings.BugReport != null)
            {
                builder.AppendSwitchQuoted($"/{nameof(settings.BugReport).ToLowerInvariant()}", ":", $"{settings.BugReport.MakeAbsolute(Environment).FullPath}");
            }

            if (settings.Checked != null && settings.Checked == true)
            {
                builder.Append($"/{nameof(settings.Checked).ToLowerInvariant()}");
            }

            if (settings.CheckSumAlgorithm != null)
            {
                builder.AppendSwitch($"/{nameof(settings.CheckSumAlgorithm).ToLowerInvariant()}", ":", $"{settings.CheckSumAlgorithm.ToString().ToLowerInvariant()}");
            }

            if (!string.IsNullOrEmpty(settings.CodePage))
            {
                builder.AppendSwitch($"/{nameof(settings.CodePage).ToLowerInvariant()}", ":", $"{settings.CodePage}");
            }

            if (settings.Debug || settings.DebugType != null)
            {
                builder.Append(settings.DebugType == null
                    ? $"/{nameof(settings.Debug).ToLowerInvariant()}"
                    : $"/{nameof(settings.Debug).ToLowerInvariant()}:{settings.DebugType.Value.ToString().ToLowerInvariant()}");
            }

            if (settings.Define.Any())
            {
                var definitions = settings.Define.Select(def => string.Format(CultureInfo.InvariantCulture, "{0};", def));
                var definitionStrings = string.Concat(definitions.ToArray()).TrimEnd(';');
                builder.AppendSwitch($"/{nameof(settings.Define).ToLowerInvariant()}", ":", $"{definitionStrings}");
            }

            if (settings.DelaySign)
            {
                builder.Append($"/{nameof(settings.DelaySign).ToLowerInvariant()}");
            }

            if (settings.Doc != null)
            {
                builder.AppendSwitchQuoted($"/{nameof(settings.Doc).ToLowerInvariant()}", ":", $"{settings.Doc.MakeAbsolute(Environment).FullPath}");
            }

            if (settings.ErrorReport != null)
            {
                builder.AppendSwitch($"/{nameof(settings.ErrorReport).ToLowerInvariant()}", ":", $"{settings.ErrorReport.ToString().ToLowerInvariant()}");
            }

            if (settings.FileAlign != null)
            {
                builder.AppendSwitch($"/{nameof(settings.FileAlign).ToLowerInvariant()}", ":", $"{(int)settings.FileAlign.Value}");
            }

            if (settings.FullPaths)
            {
                builder.Append($"/{nameof(settings.FullPaths).ToLowerInvariant()}");
            }

            if (settings.Help)
            {
                builder.Append($"/{nameof(settings.Help).ToLowerInvariant()}");
            }

            if (settings.HighEntropyVa)
            {
                builder.Append($"/{nameof(settings.HighEntropyVa).ToLowerInvariant()}");
            }

            if (!string.IsNullOrEmpty(settings.KeyContainer))
            {
                builder.AppendSwitch($"/{nameof(settings.KeyContainer).ToLowerInvariant()}", ":", $"{settings.KeyContainer}");
            }

            if (settings.KeyFile != null)
            {
                builder.AppendSwitchQuoted($"/{nameof(settings.KeyFile).ToLowerInvariant()}", ":", $"{settings.KeyFile.MakeAbsolute(Environment).FullPath}");
            }

            if (!string.IsNullOrEmpty(settings.LanguageVersion))
            {
                builder.Append($"/{nameof(settings.LanguageVersion).ToLowerInvariant()}:{settings.LanguageVersion}");
            }

            if (settings.Lib.Any())
            {
                var libs = settings.Lib.Select(lib => string.Format(CultureInfo.InvariantCulture, "\"{0}\",", lib.MakeAbsolute(Environment).FullPath));
                var libStrings = string.Concat(libs.ToArray()).TrimEnd(',');
                builder.Append($"/{nameof(settings.Lib).ToLowerInvariant()}:{libStrings}");
            }

            if (settings.Link.Any())
            {
                string linkString = FilePaths(settings.Link);

                builder.AppendSwitch($"/{nameof(settings.Link).ToLowerInvariant()}", ":", $"{linkString}");
            }

            if (settings.LinkResource?.File != null)
            {
                string filePath = settings.LinkResource.File.MakeAbsolute(Environment).FullPath;
                string linkResourceName = nameof(settings.LinkResource).ToLowerInvariant();

                builder.AppendSwitchQuoted($"/{linkResourceName}", ":", $"{filePath}");

                if (settings.LinkResource.Identifier != null && settings.LinkResource.AccessibilityModifier != null)
                {
                    builder.Append($"/t:{settings.LinkResource.Identifier} {settings.LinkResource.AccessibilityModifier}");
                }
                else if (settings.LinkResource.Identifier != null && settings.LinkResource.AccessibilityModifier == null)
                {
                    builder.Append($"/t:{settings.LinkResource.Identifier}");
                }
                else if (settings.LinkResource.AccessibilityModifier != null && settings.LinkResource.Identifier == null)
                {
                    builder.Append($"{settings.LinkResource.AccessibilityModifier}");
                }
            }

            if (!string.IsNullOrEmpty(settings.Main))
            {
                builder.AppendSwitch($"/{nameof(settings.Main).ToLowerInvariant()}", ":", $"{settings.Main}");
            }

            if (!string.IsNullOrEmpty(settings.ModuleAssemblyName))
            {
                builder.AppendSwitch($"/{nameof(settings.ModuleAssemblyName).ToLowerInvariant()}", ":", $"{settings.ModuleAssemblyName}");
            }

            if (!string.IsNullOrEmpty(settings.ModuleName))
            {
                builder.AppendSwitch($"/{nameof(settings.ModuleName).ToLowerInvariant()}", ":", $"{settings.ModuleName}");
            }

            if (settings.NoConfig)
            {
                builder.Append($"/{nameof(settings.NoConfig).ToLowerInvariant()}");
            }

            if (settings.NoLogo)
            {
                builder.Append($"/{nameof(settings.NoLogo).ToLowerInvariant()}");
            }

            if (settings.NoStandardLibrary)
            {
                builder.Append("/nostdlib");
            }

            if (settings.NoWarnings.Any())
            {
                var warnings = settings.NoWarnings.Select(x => string.Format(CultureInfo.InvariantCulture, "{0},", x));
                var warningString = string.Concat(warnings.ToArray()).TrimEnd(',');
                builder.AppendSwitch("/nowarn", ":", $"{warningString}");
            }

            if (settings.NoWin32Manifest)
            {
                builder.Append($"/{nameof(settings.NoWin32Manifest).ToLowerInvariant()}");
            }

            if (settings.Optimize)
            {
                builder.Append($"/{nameof(settings.Optimize).ToLowerInvariant()}");
            }

            if (settings.OutputFile != null)
            {
                builder.AppendSwitchQuoted("/out", ":", $"{settings.OutputFile.MakeAbsolute(Environment).FullPath}");
            }

            if (settings.Pdb != null)
            {
                builder.AppendSwitchQuoted($"/{nameof(settings.Pdb).ToLowerInvariant()}", ":", $"{settings.Pdb.MakeAbsolute(Environment).FullPath}");
            }

            if (settings.Platform != null)
            {
                builder.AppendSwitch($"/{nameof(settings.Platform).ToLowerInvariant()}", ":", $"{settings.Platform.ToString().ToLowerInvariant()}");
            }

            if (!string.IsNullOrEmpty(settings.PrefferedUILanguage))
            {
                builder.AppendSwitch($"/{nameof(settings.PrefferedUILanguage).ToLowerInvariant()}", ":", $"{settings.PrefferedUILanguage}");
            }

            if (settings.Resource?.File != null)
            {
                Resource resource = settings.Resource;
                string filePath = resource.File.MakeAbsolute(Environment).FullPath;
                string resourceName = nameof(resource).ToLowerInvariant();

                builder.AppendSwitchQuoted($"/{resourceName}", ":", $"{filePath}");

                if (resource.Identifier != null && resource.AccessibilityModifier != null)
                {
                    builder.Append($"/t:{resource.Identifier} {resource.AccessibilityModifier}");
                }
                else if (resource.Identifier != null && resource.AccessibilityModifier == null)
                {
                    builder.Append($"/t:{resource.Identifier}");
                }
                else if (resource.AccessibilityModifier != null && resource.Identifier == null)
                {
                    builder.Append($"{resource.AccessibilityModifier}");
                }
            }

            if (!string.IsNullOrEmpty(settings.SubSystemVersion))
            {
                builder.AppendSwitch($"/{nameof(settings.SubSystemVersion).ToLowerInvariant()}", ":", $"{settings.SubSystemVersion}");
            }

            if (settings.Target != null)
            {
                builder.AppendSwitch($"/{nameof(settings.Target).ToLowerInvariant()}", ":", $"{settings.Target.ToString().ToLowerInvariant()}");
            }

            if (settings.UnSafe)
            {
                builder.Append($"/{nameof(settings.UnSafe).ToLowerInvariant()}");
            }

            if (settings.UTF8Output)
            {
                builder.Append($"/{nameof(settings.UTF8Output).ToLowerInvariant()}");
            }

            if (!string.IsNullOrEmpty(settings.WarningLevel))
            {
                builder.AppendSwitch("/warn", ":", $"{settings.WarningLevel}");
            }

            if (settings.WarningsAsErrors.Any())
            {
                var errors = settings.WarningsAsErrors.Select(x => string.Format(CultureInfo.InvariantCulture, "{0},", x));
                var errorString = string.Concat(errors.ToArray()).TrimEnd(',');
                builder.AppendSwitch("/warnaserror", ":", $"{errorString}");
            }

            if (settings.Win32Icon != null)
            {
                builder.AppendSwitchQuoted($"/{nameof(settings.Win32Icon).ToLowerInvariant()}", ":", $"{ settings.Win32Icon.MakeAbsolute(Environment).FullPath}");
            }

            if (settings.Win32Manifest != null)
            {
                builder.AppendSwitchQuoted($"/{nameof(settings.Win32Manifest).ToLowerInvariant()}", ": ", $"{settings.Win32Manifest.MakeAbsolute(Environment).FullPath}");
            }

            if (settings.Win32ResourceFile != null)
            {
                builder.AppendSwitchQuoted("/win32res", ":", $"{settings.Win32ResourceFile.MakeAbsolute(Environment).FullPath}");
            }

            return builder;
        }

        private string FilePaths(IEnumerable<FilePath> filePaths)
        {
            var modules =
                filePaths.Select(
                    link =>
                            string.Format(CultureInfo.InvariantCulture, "\"{0}\";", link.MakeAbsolute(Environment).FullPath));

            var linkString = string.Concat(modules.ToArray()).TrimEnd(';');
            return linkString;
        }
    }
}