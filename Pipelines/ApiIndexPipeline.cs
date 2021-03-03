using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using System.Linq;

namespace Generator.Pipelines
{
    /// <summary>
    /// Generates the API index file.
    /// </summary>
    public class ApiIndexPipeline : Pipeline
    {
        public ApiIndexPipeline()
        {
            Dependencies.Add(nameof(ApiPipeline));

            InputModules = new ModuleList(
                new ReadFiles("_ApiIndex.cshtml"));

            ProcessModules = new ModuleList(
                new ExecuteIf(
                    Config.FromContext(ctx => ctx.Outputs.FromPipeline(nameof(ApiPipeline)).Any()),
                    new SetDestination(Config.FromSettings(settings => settings.GetPath(DocsKeys.ApiPath).Combine("index.html"))),
                    new SetMetadata(Keys.Title, "API"),
                    new ProcessShortcodes(),
                    new RenderRazor()
                        .IgnorePrefix(null)));

            OutputModules = new ModuleList(
                new WriteFiles());
        }
    }
}
