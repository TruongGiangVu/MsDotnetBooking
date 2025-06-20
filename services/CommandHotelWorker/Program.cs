using CommandHotelWorker;
using CommandHotelWorker.Services;

using Core.Modules;

using MassTransit;

using OpenSearch.Client;

var builder = Host.CreateApplicationBuilder(args);

string environment = builder.Environment.EnvironmentName;
// Add services to the container.
builder.Services.AddSerilog(environment);

// Configure MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CommandHotelConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(Core.EnvConfig.RabbitMQ.Host, (ushort)Core.EnvConfig.RabbitMQ.Port, Core.EnvConfig.RabbitMQ.VirtualHost, h =>
        {
            h.Username(Core.EnvConfig.RabbitMQ.UserName);
            h.Password(Core.EnvConfig.RabbitMQ.Password);
        });

        cfg.ReceiveEndpoint(Core.EnvConfig.RabbitMQ.HotelQueue, e =>
        {
            e.ConfigureConsumer<CommandHotelConsumer>(context);
        });
    });
});

// * Configure OpenSearch
var settings = new ConnectionSettings(new Uri(Core.EnvConfig.OpenSearch.Url))
    .BasicAuthentication(Core.EnvConfig.OpenSearch.UserName, Core.EnvConfig.OpenSearch.Password)
    .ServerCertificateValidationCallback(OpenSearch.Net.CertificateValidations.AllowAll) // for self-signed certs
    .DefaultIndex(Core.EnvConfig.OpenSearch.HotelIndex);

OpenSearchClient openSearchClient = new(settings);
builder.Services.AddSingleton<IOpenSearchClient>(openSearchClient);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
