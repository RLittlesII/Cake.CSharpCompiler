namespace Cake.CSharpCompiler
{
    /// <summary>
    /// <para>Compiler option to generate debugging information and place it in a program database (.pdb file).</para>
    /// </summary>
    public enum DebugType
    {
        /// <summary>
        /// Enables attaching a debugger to the running program.
        /// </summary>
        Full,

        /// <summary>
        /// Enables source code debugging when the program is started in the debugger but will only display assembler when the running program is attached to the debugger.
        /// </summary>
        PdbOnly
    }
}