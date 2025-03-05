using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using MongoDB.Driver;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<IConnectionFactory>(new ConnectionFactory
        {
            Uri = new Uri(context.Configuration.GetSection("ConnectionStrings")["MQConnectionString"] ?? "invalid")
        });
        services.AddSingleton<IMongoClient>(new MongoClient(context.Configuration.GetSection("ConnectionStrings")["MongoConnectionString"] ?? "invalid"));
        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(context.Configuration["MongoDatabase"]);
        });
    })
    .Build();

host.Run();
