using System.Collections.Generic;
using WebOptimizer;
using WebOptimizer.TypeScript;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions methods for registrating the Sass compiler on the Asset Pipeline.
    /// </summary>
    public static class PipelineExtensions
    {
        /// <summary>
        /// Compile markdown files on the asset pipeline.
        /// </summary>
        public static IAsset CompileTypeScript(this IAsset asset)
        {
            asset.Processors.Add(new TypeScriptProcessor());
            return asset;
        }

        /// <summary>
        /// Compile markdown files on the asset pipeline.
        /// </summary>
        public static IEnumerable<IAsset> CompileTypeScript(this IEnumerable<IAsset> assets)
        {
            var list = new List<IAsset>();

            foreach (IAsset asset in assets)
            {
                list.Add(asset.CompileTypeScript());
            }

            return list;
        }

        /// <summary>
        /// Compile markdown files on the asset pipeline.
        /// </summary>
        /// <param name="pipeline">The asset pipeline.</param>
        /// <param name="route">The route where the compiled markdown file will be available from.</param>
        /// <param name="sourceFiles">The path to the markdown source files to compile.</param>
        public static IAsset AddTypeScriptBundle(this IAssetPipeline pipeline, string route, params string[] sourceFiles)
        {
            pipeline.ServiceCollection.AddNodeServices();

            return pipeline.AddBundle(route, "application/javascript; charset=UTF-8", sourceFiles)
                           .CompileTypeScript()
                           .Concatenate();
        }

        /// <summary>
        /// Compiles markdown files into HTML and makes them servable in the browser.
        /// </summary>
        /// <param name="pipeline">The asset pipeline.</param>
        public static IEnumerable<IAsset> CompileTypeScriptFiles(this IAssetPipeline pipeline)
        {
            pipeline.ServiceCollection.AddNodeServices();

            return pipeline.AddFiles("application/javascript; charset=UTF-8", "**/*.ts")
                           .CompileTypeScript();
        }

        /// <summary>
        /// Compiles the specified markdown files into HTML and makes them servable in the browser.
        /// </summary>
        /// <param name="pipeline">The pipeline object.</param>
        /// <param name="sourceFiles">A list of relative file names of the sources to compile.</param>
        public static IEnumerable<IAsset> CompileTypeScriptFiles(this IAssetPipeline pipeline, params string[] sourceFiles)
        {
            pipeline.ServiceCollection.AddNodeServices();

            return pipeline.AddFiles("application/javascript; charset=UTF-8", sourceFiles)
                           .CompileTypeScript();
        }
    }
}
