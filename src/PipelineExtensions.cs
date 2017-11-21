using System.Collections.Generic;
using WebOptimizer;
using WebOptimizer.TypeScript;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions methods for registrating the TypeScript compiler on the Asset Pipeline.
    /// </summary>
    public static class TypeScriptPipelineExtensions
    {
        /// <summary>
        /// Compile TypeScript files on the asset pipeline.
        /// </summary>
        public static IAsset CompileTypeScript(this IAsset asset)
        {
            asset.Processors.Add(new TypeScriptProcessor());
            return asset;
        }

        /// <summary>
        /// Compile TypeScript files on the asset pipeline.
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
        /// Compile ES6 and JSX files on the asset pipeline.
        /// </summary>
        public static IAsset TranspileJavaScript(this IAsset asset)
        {
            return asset.CompileTypeScript();
        }

        /// <summary>
        /// Compile ES6 and JSX files on the asset pipeline.
        /// </summary>
        public static IEnumerable<IAsset> TranspileJavaScript(this IEnumerable<IAsset> assets)
        {
            return assets.CompileTypeScript();
        }

        /// <summary>
        /// Compile markdown files on the asset pipeline.
        /// </summary>
        /// <param name="pipeline">The asset pipeline.</param>
        /// <param name="route">The route where the compiled markdown file will be available from.</param>
        /// <param name="sourceFiles">The path to the markdown source files to compile.</param>
        public static IAsset AddTypeScriptBundle(this IAssetPipeline pipeline, string route, params string[] sourceFiles)
        {
            return pipeline.AddBundle(route, "text/javascript; charset=UTF-8", sourceFiles)
                           .CompileTypeScript()
                           .Concatenate()
                           .MinifyJavaScript();
        }

        /// <summary>
        /// Compiles TypeScript files into HTML and makes them servable in the browser.
        /// </summary>
        /// <param name="pipeline">The asset pipeline.</param>
        public static IEnumerable<IAsset> CompileTypeScriptFiles(this IAssetPipeline pipeline)
        {
            return pipeline.CompileTypeScriptFiles("**/*.ts", "**/*.tsx");
        }

        /// <summary>
        /// Compiles the specified TypeScript files into JavaScript (ES5) and makes them servable in the browser.
        /// </summary>
        /// <param name="pipeline">The pipeline object.</param>
        /// <param name="sourceFiles">A list of relative file names of the sources to compile.</param>
        public static IEnumerable<IAsset> CompileTypeScriptFiles(this IAssetPipeline pipeline, params string[] sourceFiles)
        {
            return pipeline.AddFiles("text/javascript; charset=UTF-8", sourceFiles)
                           .CompileTypeScript()
                           .MinifyJavaScript();
        }

        /// <summary>
        /// Compiles TypeScript files into JavaScript (ES5) and makes them servable in the browser.
        /// </summary>
        /// <param name="pipeline">The asset pipeline.</param>
        public static IEnumerable<IAsset> TranspileJavaScriptFiles(this IAssetPipeline pipeline)
        {
            return pipeline.CompileTypeScriptFiles("**/*.js", "**/*.jsx");
        }

        /// <summary>
        /// Compiles the specified TypeScript files into JavaScript (ES5) and makes them servable in the browser.
        /// </summary>
        /// <param name="pipeline">The pipeline object.</param>
        /// <param name="sourceFiles">A list of relative file names of the sources to compile.</param>
        public static IEnumerable<IAsset> TranspileJavaScriptFiles(this IAssetPipeline pipeline, params string[] sourceFiles)
        {
            return pipeline.CompileTypeScriptFiles(sourceFiles);
        }
    }
}
