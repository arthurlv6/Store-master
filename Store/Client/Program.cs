using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Store.Client.Services;
using Store.Client.Components;

namespace Store.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddApiAuthorization();

            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<ProductLinkService>();
            builder.Services.AddScoped<ProductCategoryService>();
            builder.Services.AddScoped<GlobalMessage>();

            await builder.Build().RunAsync();
        }
    }
}
