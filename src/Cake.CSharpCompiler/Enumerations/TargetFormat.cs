namespace Cake.CSharpCompiler
{
    /// <summary>
    /// <para>Compiler option to set the <see href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/target-compiler-option">target</see> output format.</para>
    /// </summary>
    public enum TargetFormat
    {
        /// <summary>
        /// Value to create an .exe for Windows 8.x Store apps.
        /// </summary>
        AppContainerExe,

        /// <summary>
        /// Value to create an .exe file.
        /// </summary>
        Exe,

        /// <summary>
        /// Value to create a code library.
        /// </summary>
        Library,

        /// <summary>
        /// Value to create a module.
        /// </summary>
        Module,

        /// <summary>
        /// Value to create a Windows program.
        /// </summary>
        WinExe,

        /// <summary>
        /// Value to create an intermediate .winmdobj file.
        /// </summary>
        WinMdObj
    }
}