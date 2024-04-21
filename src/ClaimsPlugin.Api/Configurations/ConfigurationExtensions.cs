namespace ClaimsPlugin.Api.Configurations
{
    public static class ConfigurationExtensions
    {
        public static void AddConfigurations(
            this WebApplicationBuilder builder,
            IHostEnvironment environment
        )
        {
            const string configurationsDirectory = "Configurations";

            builder
                .Configuration.AddJsonFile(
                    $"{configurationsDirectory}/appsettings.json",
                    optional: false,
                    reloadOnChange: true
                )
                .AddJsonFile(
                    $"{configurationsDirectory}/appsettings.{environment.EnvironmentName}.json",
                    optional: true,
                    reloadOnChange: true
                )
                .AddEnvironmentVariables();
        }
    }
}
