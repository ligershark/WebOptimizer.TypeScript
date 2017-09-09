using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.NodeServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace WebOptimizer.TypeScript
{
    /// <summary>
    /// Compiles TypeScript/ES6 files
    /// </summary>
    /// <seealso cref="IProcessor" />
    public class TypeScriptProcessor : IProcessor
    {
        private static object _syncRoot = new object();
        private const string _name = "WebOptimizer.TypeScript";

        /// <summary>
        /// Gets the custom key that should be used when calculating the memory cache key.
        /// </summary>
        public string CacheKey(HttpContext context) => string.Empty;

        /// <summary>
        /// Gets the directory of the node modules.
        /// </summary>
        public static string WorkingDirectory { get; } = Path.Combine(Path.GetTempPath(), "WebOptimizer.TypeScript");

        /// <summary>
        /// Executes the processor on the specified configuration.
        /// </summary>
        public async Task ExecuteAsync(IAssetContext context)
        {
            var nodeServices = (INodeServices)context.HttpContext.RequestServices.GetService(typeof(INodeServices));
            var env = (IHostingEnvironment)context.HttpContext.RequestServices.GetService(typeof(IHostingEnvironment));
            var fileProvider = context.Asset.GetFileProvider(env);
            var content = new Dictionary<string, byte[]>();

            if (!EnsureNodeFiles())
                return;

            string module = Path.Combine(WorkingDirectory, "typescript");

            foreach (string route in context.Content.Keys)
            {
                var file = fileProvider.GetFileInfo(route);
                var input = context.Content[route].AsString();
                var result = await nodeServices.InvokeAsync<string>(module, input, file.PhysicalPath);

                content[route] = result.AsByteArray();
            }

            context.Content = content;
        }

        private bool EnsureNodeFiles()
        {
            var packageFile = Path.Combine(WorkingDirectory, "node_modules", "typescript", "package.json");

            if (File.Exists(packageFile))
            {
                return true;
            }

            lock (_syncRoot)
            {
                if (!File.Exists(packageFile))
                {
                    try
                    {
                        if (Directory.Exists(WorkingDirectory))
                        {
                            Directory.Delete(WorkingDirectory, true);
                        }

                        var assembly = GetType().Assembly;

                        using (var resourceStream = assembly.GetManifestResourceStream("WebOptimizer.TypeScript.node_files.zip"))
                        using (var zip = new ZipArchive(resourceStream))
                        {
                            zip.ExtractToDirectory(WorkingDirectory);
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
