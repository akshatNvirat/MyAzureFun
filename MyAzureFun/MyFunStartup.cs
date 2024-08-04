using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(MyAzureFun.MyFunStartup))]
namespace MyAzureFun
{
    public class MyFunStartup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var kvUrl = new Uri(Environment.GetEnvironmentVariable("KVUrl"));
            var secretClient = new SecretClient(kvUrl, new DefaultAzureCredential());
            var conStr = secretClient.GetSecret("ConStr").Value.Value;

            builder.Services.AddDbContext<AppDbCtx>(options => options.UseSqlServer(conStr));
        }
    }
}
