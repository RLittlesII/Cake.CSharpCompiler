using Cake.Core.IO;
using Cake.Core.Tooling;
using System.Collections.Generic;

namespace Cake.CSharpCompiler
{
    /// <summary>
    /// Contains settings used by <see cref="CSharpCompiler"/>.
    /// </summary>
    public class CSharpCompilerSettings : ToolSettings
    {
        /// <summary>
        ///
        /// </summary>
        public IEnumerable<FilePath> Modules { get; set; }

        /// <summary>
        /// This option specifies the location of app.config at assembly binding time.
        /// </summary>
        public FilePath AppConfig { get; set; }

        /// <summary>
        /// This option specifies the base address for the DLL. This address can be specified as a decimal, hexadecimal, or octal number.
        /// </summary>
        public string BaseAddress { get; set; }

        /// <summary>
        /// This option specifies that debug information should be placed in a file for later analysis.
        /// </summary>
        public FilePath BugReport { get; set; }

        /// <summary>
        /// This option specifies whether an integer arithmetic statement that results in a value that is outside the range
        /// of the data type, and that is not in the scope of a checked or unchecked keyword, causes a run-time exception.
        /// </summary>
        public bool? Checked { get; set; }

        /// <summary>
        /// This option specifies the algorithm for calculating the source file checksum stored in PDB. Supported values are:
        /// SHA1 (default) or SHA256.
        /// </summary>
        public CheckSumAlgorithm? CheckSumAlgorithm { get; set; }

        /// <summary>
        /// This option specifies which codepage to use during compilation if the required page is not the current default codepage for the system.
        ///  <code>
        ///  /codepage:id
        /// </code>
        /// </summary>
        public string CodePage { get; set; }

        /// <summary>
        /// This option specifies whether to emit debugging informaion.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DebugType? DebugType { get; set; }

        /// <summary>
        /// This option specifies the #define preprocessor directive except that the compiler option is in effect for all files in the project.
        /// </summary>
        public IEnumerable<string> Define { get; set; }

        /// <summary>
        /// This option specifies whether to delay-sign the assembly by using only the public part of the strong name key.
        /// </summary>
        public bool DelaySign { get; set; }

        /// <summary>
        /// This option specifies an XML Documentation file to generate.
        /// </summary>
        public FilePath Doc { get; set; }

        /// <summary>
        /// This option specifies how to handle internal compiler errors: prompt, send, or none. The default is none.
        /// </summary>
        public ErrorReport? ErrorReport { get; set; }

        /// <summary>
        /// This option specifies the size of sections in your output file.
        /// </summary>
        public FileAlign? FileAlign { get; set; }

        /// <summary>
        /// This option causes the compiler to specify the full path to the file when listing compilation errors and warnings.
        /// </summary>
        public bool FullPaths { get; set; }

        /// <summary>
        /// Displays a usage message to stdout.
        /// </summary>
        public bool Help { get; set; }

        /// <summary>
        /// Specifies that high entropy ASLR is supported.
        /// </summary>
        public bool HighEntropyVa { get; set; }

        /// <summary>
        /// This options specifies a strong name key container.
        /// </summary>
        public string KeyContainer { get; set; }

        /// <summary>
        /// This option specifies a strong name key file.
        /// </summary>
        public FilePath KeyFile { get; set; }

        /// <summary>
        /// Specify language version mode: ISO-1, ISO-2, 3, 4, 5, 6, or Default
        /// </summary>
        public string LanguageVersion { get; set; }

        /// <summary>
        /// This option specifies the location of assemblies referenced by means of the /reference (C# Compiler Options) option.
        /// </summary>
        public IEnumerable<DirectoryPath> Lib { get; set; }

        /// <summary>
        /// This option makes COM type information in specified assemblies available to the project.
        /// </summary>
        public IEnumerable<FilePath> Link { get; set; }

        /// <summary>
        /// This option links the specified resource to this assembly.
        /// </summary>
        public Resource LinkResource { get; set; }

        /// <summary>
        /// This option specifies the type that contains the entry point (ignore all other possible entry points).
        /// </summary>
        public string Main { get; set; }

        /// <summary>
        /// This option specifies an assembly whose non-public types a .netmodule can access.
        /// </summary>
        public string ModuleAssemblyName { get; set; }

        /// <summary>
        /// This option specifies the name of the source module.
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// This option instructs the compiler not to auto include CSC.RSP file.
        /// </summary>
        public bool NoConfig { get; set; }

        /// <summary>
        /// This option suppresses compiler copyright message.
        /// </summary>
        public bool NoLogo { get; set; }

        /// <summary>
        /// This option instructs the compiler not to reference standard library (mscorlib.dll).
        /// </summary>
        public bool NoStandardLibrary { get; set; }

        /// <summary>
        /// This option disables specific warning messages
        /// </summary>
        public IEnumerable<string> NoWarnings { get; set; }

        /// <summary>
        /// This option enables or disables whether to embed an application manifest in the executable file.
        /// </summary>
        public bool NoWin32Manifest { get; set; }

        /// <summary>
        /// This option enables or disables optimizations performed by the compiler to make your output file smaller, faster, and more efficient.
        /// </summary>
        public bool Optimize { get; set; }

        /// <summary>
        /// This option specifies the output file name (default: base name of file with main class or first file).
        /// </summary>
        public FilePath OutputFile { get; set; }

        /// <summary>
        /// This option specifies an input file as a command line argument.
        /// </summary>
        public FilePath InputFile { get; set; }

        /// <summary>
        /// This option specifies which version of the common language runtime (CLR) can run the assembly.
        /// </summary>
        public Platform? Platform { get; set; }

        /// <summary>
        /// This option specifies the language in which the C# compiler displays output.
        /// </summary>
        public string PrefferedUILanguage { get; set; }

        /// <summary>
        /// This option enables you to compile source code files in all child directories of either the specified directory (dir) or of the project directory.
        /// </summary>
        public bool Recurse { get; set; }

        /// <summary>
        /// This option causes the compiler to import public type information in the specified file, with the specified alias into the current project.
        /// </summary>
        public IDictionary<string, string> ReferenceAliases { get; set; }

        /// <summary>
        /// This option causes the compiler to import public type information in the specified file into the current project.
        /// </summary>
        public IEnumerable<FilePath> ReferenceFiles { get; set; }

        /// <summary>
        /// This option embeds the specified resource.
        /// </summary>
        public Resource Resource { get; set; }

        /// <summary>
        /// This option specifies a ruleset file that disables specific diagnostics.
        /// </summary>
        public FilePath RuleSet { get; set; }

        /// <summary>
        /// This option specifies the minimum version of the subsystem on which the generated executable file can run.
        /// </summary>
        public string SubSystemVersion { get; set; }

        /// <summary>
        /// This option specifies the format of the output file by using <see cref="TargetFormat"/> options.
        /// </summary>
        public TargetFormat? Target { get; set; }

        /// <summary>
        /// This option allows unsafe code.
        /// </summary>
        public bool UnSafe { get; set; }

        /// <summary>
        /// This option outputs compiler messages in UTF-8 encoding.
        /// </summary>
        public bool UTF8Output { get; set; }

        /// <summary>
        /// This option sets the warning level (0-4).
        /// </summary>
        public string WarningLevel { get; set; }

        /// <summary>
        /// This option treats all warnings as errors.
        /// </summary>
        public bool AllWarningsAsErrors { get; set; }

        /// <summary>
        /// This option treats the specified warnings as errors.
        /// </summary>
        public IEnumerable<string> WarningsAsErrors { get; set; }

        /// <summary>
        /// This option uses this icon for the output.
        /// </summary>
        public FilePath Win32Icon { get; set; }

        /// <summary>
        /// This option specifies a custom win32 manifest file.
        /// </summary>
        public FilePath Win32Manifest { get; set; }

        /// <summary>
        /// The option specifies the win32 resource file (.res).
        /// </summary>
        public FilePath Win32ResourceFile { get; set; }

        /// <summary>
        /// This option specifies the file name and location of the .pdb file.
        /// </summary>
        public FilePath Pdb { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpCompilerSettings"/> class.
        /// </summary>
        public CSharpCompilerSettings()
        {
            Modules = new List<FilePath>();
            Define = new List<string>();
            Lib = new List<DirectoryPath>();
            Link = new List<FilePath>();
            NoWarnings = new List<string>();
            ReferenceFiles = new List<FilePath>();
            WarningsAsErrors = new List<string>();
        }
    }
}