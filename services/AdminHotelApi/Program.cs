using System.Text.Json.Serialization;

using AdminHotelApi.Helper;
using AdminHotelApi.Repositories;
using AdminHotelApi.Services;

using Core.Modules;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
string environment = builder.Environment.EnvironmentName;
// Add services to the container.
builder.AddCultureInfo();
builder.Services.AddSerilog(environment);
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
    options.UseNpgsql(Core.EnvConfig.PostgresSql.ConnectionString));

builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelService, HotelService>();

builder.Services.AddMassTransit(x =>
{
    // Configure RabbitMQ as the transport
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(Core.EnvConfig.RabbitMQ.Host, (ushort)Core.EnvConfig.RabbitMQ.Port, Core.EnvConfig.RabbitMQ.VirtualHost, h =>
        {
            h.Username(Core.EnvConfig.RabbitMQ.UserName);
            h.Password(Core.EnvConfig.RabbitMQ.Password);
        });
    });
});


var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
app.MapOpenApi();
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
// app.MapGet("/", () => $"${Ct.Common.AppName} is running");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Log.Information($"{Ct.Common.AppName} start successfully");
Log.Information($"Run at environment: {environment}");

app.Run();
