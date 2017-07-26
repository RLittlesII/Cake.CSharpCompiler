using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using System;

namespace Cake.CSharpCompiler
{
    /// <summary>
    /// <para>Contains functionality related to the command line tool csc.</para>
    /// <para>
    /// In order to use the commands for this addin, you will need to include the following in your build.cake file to download and reference from NuGet.org:
    /// <code>
    /// #addin Cake.CSharpCompiler
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory(nameof(Cake.CSharpCompiler))]
    public static class CSharpCompilerAliases
    {
        /// <summary>
        /// <code>
        ///  <example>
        ///   Compile("./src/cake.cs",
        ///                 new CSharpCompilerSettings
        ///                 {
        ///                     Platform = Platform.AnyCpu,
        ///                     Target = TargetFormat.Exe,
        ///                     NoLogo = true
        ///                 });
        ///  </example>
        /// </code>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Compile")]
        public static void Compile(this ICakeContext context, FilePath filePath, CSharpCompilerSettings settings)
        {
            CSharpCompiler compiler = Compiler(context);

            compiler.Compile(filePath, settings);
        }

        /// <summary>
        /// <code>
        ///  <example>
        ///   Compile("./src/cake.cs",
        ///                  settings =>
        ///                  {
        ///                      settings.Platform = Platform.AnyCpu;
        ///                      settings.Target = TargetFormat.Exe;
        ///                      settings.NoLogo = true;
        ///                  });
        ///   </example>
        /// </code>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="action">The action.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Compile")]
        public static void Compile(this ICakeContext context, FilePath filePath, Action<CSharpCompilerSettings> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var settings = new CSharpCompilerSettings();

            action(settings);

            Compile(context, filePath, settings);
        }

        /// <summary>
        /// <code>
        ///  <example>
        ///   Compile("./src/cake.cs",
        ///                 new CSharpCompilerSettings
        ///                 {
        ///                     Platform = Platform.AnyCpu,
        ///                     Target = TargetFormat.Exe,
        ///                     NoLogo = true
        ///                 });
        ///  </example>
        /// </code>
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Compile")]
        public static void Compile(this ICakeContext context, string pattern, CSharpCompilerSettings settings)
        {
            var compiler = Compiler(context);

            compiler.Compile(pattern, settings);
        }

        /// <summary>
        /// <code>
        ///  <example>
        ///   Compile("./src/cake.cs",
        ///                  settings =>
        ///                  {
        ///                      settings.Platform = Platform.AnyCpu;
        ///                      settings.Target = TargetFormat.Exe;
        ///                      settings.NoLogo = true;
        ///                  });
        ///   </example>
        /// </code>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern</param>
        /// <param name="action">The action.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Compile")]
        public static void Compile(this ICakeContext context, string pattern, Action<CSharpCompilerSettings> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var settings = new CSharpCompilerSettings();

            action(settings);

            Compile(context, pattern, settings);
        }

        /// <summary>
        /// <code>
        ///  <example>
        ///   Compile("./src/cake.cs",
        ///                 new CSharpCompilerSettings
        ///                 {
        ///                     Platform = Platform.AnyCpu,
        ///                     Target = TargetFormat.Exe,
        ///                     NoLogo = true
        ///                 });
        ///  </example>
        /// </code>
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Compile")]
        public static void Compile(this ICakeContext context, DirectoryPath directoryPath, CSharpCompilerSettings settings)
        {
            var compiler = Compiler(context);

            compiler.Compile(directoryPath, settings);
        }

        /// <summary>
        /// <code>
        ///  <example>
        ///   Compile("./src/cake.cs",
        ///                  settings =>
        ///                  {
        ///                      settings.Platform = Platform.AnyCpu;
        ///                      settings.Target = TargetFormat.Exe;
        ///                      settings.NoLogo = true;
        ///                  });
        ///   </example>
        /// </code>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="action">The action.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Compile")]
        public static void Compile(this ICakeContext context, DirectoryPath directoryPath, Action<CSharpCompilerSettings> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var settings = new CSharpCompilerSettings();

            action(settings);

            Compile(context, directoryPath, settings);
        }

        private static CSharpCompiler Compiler(ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var compiler = new CSharpCompiler(context.FileSystem,
                context.Environment,
                context.ProcessRunner,
                context.Tools,
                context.Globber);
            return compiler;
        }
    }
}