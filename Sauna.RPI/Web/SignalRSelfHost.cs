using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace Sauna.RPI.Web
{
    internal class SignalRSelfHost
    {
        public async Task StartListenAsync()
        {
            await CreateHostBuilder(null).Build().RunAsync();
        }

        IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.ListenLocalhost(31337);
                    })
                    .UseStartup<Startup>();
                });
    }
}