using Cake.Core.IO;

namespace Cake.CSharpCompiler
{
    /// <summary>
    /// .NET Framework resource file.
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        public FilePath File { get; set; }

        /// <summary>
        /// This option specifies the logical name for the resource; the name that is used to load the resource. The default is the name of the file.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// This option specifies the accessibility of the resource: public or private. The default is public.
        /// </summary>
        public string AccessibilityModifier { get; set; }
    }
}