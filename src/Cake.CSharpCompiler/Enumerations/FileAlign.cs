namespace Cake.CSharpCompiler
{
    /// <summary>
    /// <para>Compiler options that lets you specify the size of sections in your output file.</para>
    /// </summary>
    public enum FileAlign
    {
        /// <summary>
        /// Value that specifies 512 byte sections in the output file.
        /// </summary>
        x512 = 512,

        /// <summary>
        /// Value that specifies 1024 byte sections in the output file.
        /// </summary>
        x1024 = 1024,

        /// <summary>
        /// Value that specifies 2048 byte sections in the output file.
        /// </summary>
        x2048 = 2048,

        /// <summary>
        /// Value that specifies 4096 byte sections in the output file.
        /// </summary>
        x4096 = 4096,

        /// <summary>
        /// Value that specifies 8192 byte sections in the output file.
        /// </summary>
        x8192 = 8192
    }
}