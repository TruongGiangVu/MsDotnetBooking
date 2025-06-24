using CommandHotelWorker;
using CommandHotelWorker.Constants;
using CommandHotelWorker.Helper;
using CommandHotelWorker.Services;

using OpenSearch.Client;

using Serilog;
using Serilog.Sinks.OpenSearch;

var builder = Host.CreateApplicationBuilder(args);

string environment = builder.Environment.EnvironmentName;

// * Serilog
// đọc serilog config từ file appsettings.json hoặc file appsettings.*.json, tùy vào môi trường api đang chạy
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// config lại logger của api
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .Enrich.WithComputed("SourceContext", "Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)") // chỗ placeholder SourceContext sẽ log tên class mà ko log namespace của log
                                                                                                           // config log vào opensearch
    .WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri(CommandHotelWorker.EnvConfig.OpenSearch.Url))
    {
        ModifyConnectionSettings = x => x.BasicAuthentication(CommandHotelWorker.EnvConfig.OpenSearch.UserName, CommandHotelWorker.EnvConfig.OpenSearch.Password)
                .ServerCertificateValidationCallback((sender, cert, chain, sslPolicyErrors) => true),
        AutoRegisterTemplate = true, // Automatically register index template
        IndexFormat = $"worker-api-logs-{DateTime.Now:yyyy.MM.dd}", // Daily index
        TypeName = null, // For OpenSearch 2.x and above, this should be null
        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog | EmitEventFailureHandling.RaiseCallback,
        FailureCallback = e => Console.WriteLine($"Failed to log to OpenSearch: {e.ToJsonString()}")
    })
    .CreateLogger();

builder.Services.AddSerilog();

// * Configure OpenSearch
var settings = new ConnectionSettings(new Uri(CommandHotelWorker.EnvConfig.OpenSearch.Url))
    .BasicAuthentication(CommandHotelWorker.EnvConfig.OpenSearch.UserName, CommandHotelWorker.EnvConfig.OpenSearch.Password)
    .ServerCertificateValidationCallback(OpenSearch.Net.CertificateValidations.AllowAll) // for self-signed certs
    .DefaultIndex(CommandHotelWorker.EnvConfig.OpenSearch.HotelIndex);

OpenSearchClient openSearchClient = new(settings);
builder.Services.AddSingleton<IOpenSearchClient>(openSearchClient);

builder.Services.AddSingleton<IOpenSearchService, OpenSearchService>();
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

Log.Information($"{AppConstant.Common.ProjectName}.{AppConstant.Common.AppName} start successfully");
Log.Information($"Run at environment: {environment}");

host.Run();
