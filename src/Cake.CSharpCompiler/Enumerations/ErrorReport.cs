namespace Cake.CSharpCompiler
{
    /// <summary>
    /// <para>Compiler option provides a convenient way to report a C# internal compiler error to Microsoft.</para>
    /// </summary>
    public enum ErrorReport
    {
        /// <summary>
        /// Reports about internal compiler errors will not be collected or sent to Microsoft.
        /// </summary>
        None,

        /// <summary>
        /// Prompts you to send a report when you receive an internal compiler error.
        /// </summary>
        Prompt,

        /// <summary>
        /// (default) You will not be prompted to send reports for failures more than once every three days. queue is the default when you compile an application at the command line.
        /// </summary>
        Queue,

        /// <summary>
        /// Automatically sends reports of internal compiler errors to Microsoft.
        /// </summary>
        Send
    }
}