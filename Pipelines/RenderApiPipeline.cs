using Statiq.Common;
using Statiq.Core;
using Statiq.Html;
using Statiq.Razor;
using System.Linq;

namespace Generator.Pipelines
{
    /// <summary>
    /// Renders and outputs the API pages using the API template layouts
    /// (this pipeline might take a bit of time).
    /// </summary>
    public class RenderApiPipeline : Pipeline
    {
        public RenderApiPipeline()
        {
            Dependencies.Add(nameof(ApiPipeline));

            ProcessModules = new ModuleList(
                new ExecuteIf(
                    Config.FromContext(ctx => ctx.Outputs.FromPipeline(nameof(ApiPipeline)).Any()),
                    new ConcatDocuments(nameof(ApiPipeline)),
                    new ProcessShortcodes(),
                    new RenderRazor()
                        .WithLayout("/_ApiLayout.cshtml"),
                    new GatherHeadings(),
                    new InsertHtml("div#infobar-headings", Config.FromDocument(async (IDocument doc, IExecutionContext ctx) => await ctx.GenerateInfobarHeadingsAsync(doc)))));

            OutputModules = new ModuleList(
                new WriteFiles());
        }
    }
}
