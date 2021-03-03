using Microsoft.Extensions.DependencyInjection;
using Statiq.CodeAnalysis;
using Statiq.Common;
using Statiq.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Generator.Pipelines
{
    /// <summary>
    /// Uses Roslyn to analyze any source files loaded in the previous
    /// pipeline along with any specified assemblies. This pipeline
    /// results in documents that represent Roslyn symbols.
    /// </summary>
    public class ApiPipeline : Pipeline
    {
        public ApiPipeline()
        {
            Dependencies.Add(nameof(CodePipeline));

            ProcessModules = new ModuleList(
                new ExecuteIf(
                    Config.FromContext(ctx =>
                        ctx.Outputs.FromPipeline(nameof(CodePipeline)).Any()
                        || ctx.Settings.GetList<string>(DocsKeys.AssemblyFiles).Count > 0
                        || ctx.Settings.GetList<string>(DocsKeys.ProjectFiles).Count > 0
                        || ctx.Settings.GetList<string>(DocsKeys.SolutionFiles).Count > 0),
                    new ConcatDocuments(nameof(CodePipeline)),
                    new ExecuteConfig(Config.FromContext(ctx =>
                        new AnalyzeCSharp()
                            .WhereNamespaces(ctx.Settings.GetBool(DocsKeys.IncludeGlobalNamespace))
                            .WherePublic()
                            .WithCssClasses("code", "cs")
                            .WithDestinationPrefix(ctx.GetPath(DocsKeys.ApiPath))
                            .WithAssemblies(Config.FromContext<IEnumerable<string>>(ctx => ctx.GetList<string>(DocsKeys.AssemblyFiles)))
                            .WithProjects(Config.FromContext<IEnumerable<string>>(ctx => ctx.GetList<string>(DocsKeys.ProjectFiles)))
                            .WithSolutions(Config.FromContext<IEnumerable<string>>(ctx => ctx.GetList<string>(DocsKeys.SolutionFiles)))
                            .WithAssemblySymbols()
                            .WithImplicitInheritDoc(ctx.GetBool(DocsKeys.ImplicitInheritDoc)))),
                    new ExecuteConfig(Config.FromDocument((doc, ctx) =>
                    {
                        // Calculate a type name to link lookup for auto linking
                        string name = null;
                        string kind = doc.GetString(CodeAnalysisKeys.Kind);
                        if (kind == "NamedType")
                        {
                            name = doc.GetString(CodeAnalysisKeys.DisplayName);
                        }
                        else if (kind == "Property" || kind == "Method")
                        {
                            IDocument containingType = doc.GetDocument(CodeAnalysisKeys.ContainingType);
                            if (containingType != null)
                            {
                                name = $"{containingType.GetString(CodeAnalysisKeys.DisplayName)}.{doc.GetString(CodeAnalysisKeys.DisplayName)}";
                            }
                        }
                        if (name != null)
                        {
                            ConcurrentDictionary<string, string> typeNamesToLink = ctx.GetService<ConcurrentDictionary<string, string>>();
                            typeNamesToLink.AddOrUpdate(WebUtility.HtmlEncode(name), ctx.GetLink(doc), (x, y) => string.Empty);
                        }
                    }))));
        }
    }
}
