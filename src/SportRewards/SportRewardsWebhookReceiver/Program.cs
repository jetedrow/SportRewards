using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Configuration;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<IConnectionFactory>(new ConnectionFactory
        {
            Uri = new Uri(context.Configuration.GetSection("ConnectionStrings")["MQConnectionString"] ?? "invalid")
        });
    })
    .Build();


host.Run();
