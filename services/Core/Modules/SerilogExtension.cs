using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog.Sinks.OpenSearch;

namespace Core.Modules;

public static class SerilogExtension
{
    /// <summary>
    /// Adds Serilog logging to the service collection
    /// </summary>
    /// <param name="services">The service collection to add Serilog to.</param>
    /// <param name="environment">The current environment (e.g., Development, Production).</param>
    public static void AddSerilog(this IServiceCollection services, string environment)
    {
        var basePath = AppContext.BaseDirectory;

        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("serilog.json", optional: true, reloadOnChange: true) // app-level
            .AddEnvironmentVariables();

        IConfigurationRoot configuration = configBuilder.Build();

        var loggerConfig = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithComputed("SourceContext", "Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)");

        loggerConfig.WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri(EnvConfig.OpenSearch.Url))
        {
            ModifyConnectionSettings = x => x.BasicAuthentication(EnvConfig.OpenSearch.UserName, EnvConfig.OpenSearch.Password)
                .ServerCertificateValidationCallback((sender, cert, chain, sslPolicyErrors) => true),

            AutoRegisterTemplate = true,
            IndexFormat = $"dotnet-api-logs-{DateTime.UtcNow:yyyy.MM.dd}",
            TypeName = null,
            EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog | EmitEventFailureHandling.RaiseCallback,
            FailureCallback = e => Console.WriteLine($"Failed to log to OpenSearch: {e.ToJsonString()}"),
        });

        Log.Logger = loggerConfig.CreateLogger();

        // Bind Serilog to Microsoft.Extensions.Logging
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
    }
}
