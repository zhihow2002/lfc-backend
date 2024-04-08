using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Api.Configurations
{
    internal static class Startup
    {
        internal static void AddConfigurations(
            this WebApplicationBuilder builder,
            IHostEnvironment environment
        )
        {
            const string configurationsDirectory = "Configurations";

            builder
                .Configuration.AddJsonFile(
                    $"{configurationsDirectory}/appsettings.json",
                    false,
                    true
                )
                .AddJsonFile(
                    $"{configurationsDirectory}/appsettings.{environment.EnvironmentName}.json",
                    true,
                    true
                )
                .AddEnvironmentVariables();
        }
    }
}
