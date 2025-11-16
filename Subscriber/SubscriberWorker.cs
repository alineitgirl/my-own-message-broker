using Microsoft.Extensions.Hosting;
using System.Net.Http.Json;
using Subscriber.Dtos;

namespace Subscriber;

public class SubscriberWorker : BackgroundService
{
    private readonly IHttpClientFactory _clientFactory;

    public SubscriberWorker(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Subscriber started. Press CRTL+C to stop.");

        var client = _clientFactory.CreateClient("broker");

        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await client.GetAsync("/api/topics/2/subscriptions/2/messages");

            if (!messages.IsSuccessStatusCode)
            {
                await Task.Delay(2000, stoppingToken);
                continue;
            }
            
            var list = await messages.Content.ReadFromJsonAsync<List<MessageReadDto>>();

            if (list != null && list.Count > 0)
            {
                Console.WriteLine($"Received {list.Count} messages:");

                var ackIds = list.Select(m => m.Id).ToList();

                foreach (var m in list)
                {
                    Console.WriteLine($"{m.Id} - {m.TopicMessage} - {m.MessageStatus}");
                }

                await client.PostAsJsonAsync(
                    "/api/topics/2/subscriptions/2/messages",
                    ackIds,
                    stoppingToken);

                Console.WriteLine("Messages ACKed.");
            }

            await Task.Delay(2000, stoppingToken);
        }
    }
}