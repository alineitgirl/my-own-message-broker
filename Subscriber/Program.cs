using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Net.Http.Json;
using Subscriber;

Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddHttpClient("broker", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7005");
        });

        services.AddHostedService<SubscriberWorker>();
    })
    .Build()
    .Run();


