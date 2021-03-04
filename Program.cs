using Microsoft.Extensions.DependencyInjection;
using Statiq.App;
using Statiq.Common;
using Statiq.Docs;
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
                .CreateDocs(args)
                .RunAsync();
        }
    }
}
