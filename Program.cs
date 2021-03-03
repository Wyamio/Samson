using Microsoft.Extensions.DependencyInjection;
using Statiq.App;
using Statiq.Common;
using Statiq.Web;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Generator
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            return await Bootstrapper.Factory
                .CreateWeb(args)
                .AddSetting(DocsKeys.SourceFiles, new[]
                {
                    "src/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs",
                    "../src/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs"
                })
                .AddSetting(DocsKeys.ApiPath, new NormalizedPath("api"))
                .ConfigureServices(services => services
                    .AddSingleton<ConcurrentDictionary<string, string>>())
                .RunAsync();
        }
    }
}
