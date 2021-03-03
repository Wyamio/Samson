using Statiq.Common;
using Statiq.Core;
using System.Linq;

namespace Generator.Pipelines
{
    /// <summary>
    /// Loads source files.
    /// </summary>
    public class CodePipeline : Pipeline
    {
        public CodePipeline()
        {
            InputModules = new ModuleList(
                new ReadFiles(
                    Config.FromSettings(settings
                        => settings.GetList<string>(DocsKeys.SourceFiles).AsEnumerable())));
        }
    }
}
