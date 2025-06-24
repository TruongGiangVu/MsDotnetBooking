using System.Text.Json.Serialization;

using AdminHotelApi.Modules;
using AdminHotelApi.Repositories;
using AdminHotelApi.Services;

using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

using Serilog.Sinks.OpenSearch;

var builder = WebApplication.CreateBuilder(args);
string environment = builder.Environment.EnvironmentName;
// Add services to the container.
builder.AddCultureInfo();

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
    .WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri(AdminHotelApi.EnvConfig.OpenSearch.Url))
    {
        ModifyConnectionSettings = x => x.BasicAuthentication(AdminHotelApi.EnvConfig.OpenSearch.UserName, AdminHotelApi.EnvConfig.OpenSearch.Password)
                .ServerCertificateValidationCallback((sender, cert, chain, sslPolicyErrors) => true),
        AutoRegisterTemplate = true, // Automatically register index template
        IndexFormat = $"admin-api-logs-{DateTime.Now:yyyy.MM.dd}", // Daily index
        TypeName = null, // For OpenSearch 2.x and above, this should be null
        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog | EmitEventFailureHandling.RaiseCallback,
        FailureCallback = e => Console.WriteLine($"Failed to log to OpenSearch: {e.ToJsonString()}")
    })
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecurityTransformer>();
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(AdminHotelApi.EnvConfig.PostgresSql.ConnectionString));

builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference("/scalar", options =>
{
    options.Theme = ScalarTheme.BluePlanet;
    options.AddDocument("v1", "Admin Hotel API", "/openapi/{documentName}.json");
    options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.MapGet("/", context =>
{
    context.Response.Redirect("scalar");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Log.Information($"{Ct.Common.ProjectName}.{Ct.Common.AppName} start successfully");
Log.Information($"Run at environment: {environment}");

app.Run();
