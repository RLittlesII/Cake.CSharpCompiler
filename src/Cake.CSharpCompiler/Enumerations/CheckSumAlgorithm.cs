namespace Cake.CSharpCompiler
{
    /// <summary>
    /// <para>Compiler option to specify the algorithm for calculating the source file checksum stored in PDB.</para>
    /// </summary>
    public enum CheckSumAlgorithm
    {
        /// <summary>
        /// (default) Secure Hash Algorithm 1
        /// </summary>
        SHA1,

        /// <summary>
        /// Secure Hash Algorithm 256
        /// </summary>
        SHA256
    }
}