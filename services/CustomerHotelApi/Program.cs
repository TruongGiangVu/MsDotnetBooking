using System.Text.Json.Serialization;

using Core.Modules;

using CustomerHotelApi.Helper;

using OpenSearch.Client;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

string environment = builder.Environment.EnvironmentName;

builder.AddCultureInfo();
builder.Services.AddSerilog(environment);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
// Add services to the container.

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

// * Configure OpenSearch
var settings = new ConnectionSettings(new Uri(Core.EnvConfig.OpenSearch.Url))
    .BasicAuthentication(Core.EnvConfig.OpenSearch.UserName, Core.EnvConfig.OpenSearch.Password)
    .ServerCertificateValidationCallback(OpenSearch.Net.CertificateValidations.AllowAll) // for self-signed certs
    .DefaultIndex(Core.EnvConfig.OpenSearch.HotelIndex);

OpenSearchClient openSearchClient = new(settings);
builder.Services.AddSingleton<IOpenSearchClient>(openSearchClient);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseExceptionHandler();
// Configure the HTTP request pipeline.
app.MapScalarApiReference("/scalar", options =>
{
    options.Theme = ScalarTheme.BluePlanet;
    options.AddDocument("v1", "Production API", "/openapi/{documentName}.json");
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

Log.Information($"{Ct.Common.AppName} start successfully");
Log.Information($"Run at environment: {environment}");

app.Run();
