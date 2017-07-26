namespace Cake.CSharpCompiler
{
    /// <summary>
    /// <para>Compiler options that set the target <see href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/platform-compiler-option">Platform</see>.</para>
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// (default) Compiles your assembly to run on any platform. Your application runs as a 64-bit process whenever possible and falls back to 32-bit when only that mode is available.
        /// </summary>
        AnyCpu = 0,

        /// <summary>
        /// Compiles your assembly to run on any platform. Your application runs in 32-bit mode on systems that support both 64-bit and 32-bit applications. You can specify this option only for projects that target the .NET Framework 4.5.
        /// </summary>
        AnyCpu32BitPreferred,

        /// <summary>
        /// Compiles your assembly to run on a computer that has an Advanced RISC Machine (ARM) processor.
        /// </summary>
        ARM,

        /// <summary>
        /// Compiles your assembly to be run by the 64-bit common language runtime on a computer that supports the AMD64 or EM64T instruction set.
        /// </summary>
        x64,

        /// <summary>
        /// Compiles your assembly to be run by the 32-bit, x86-compatible common language runtime.
        /// </summary>
        x86,

        /// <summary>
        /// Compiles your assembly to be run by the 64-bit common language runtime on a computer with an Itanium processor.
        /// </summary>
        Itanium
    }
}