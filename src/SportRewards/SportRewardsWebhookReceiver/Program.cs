using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<IConnectionFactory>(new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "admin",
            Password = "password"            
        });
    })
    .Build();

host.Run();
