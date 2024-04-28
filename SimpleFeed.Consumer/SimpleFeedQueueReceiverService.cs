using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SimpleFeed.Consumer;

public class SimpleFeedQueueReceiverService(IConfiguration configuration, ServiceBusClient serviceBusClient) : BackgroundService
{
    private readonly ServiceBusReceiver _receiver =
        serviceBusClient.CreateReceiver(configuration.GetValue<string>("ServiceBus:QueueName"));
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _receiver.ReceiveMessagesAsync(cancellationToken: stoppingToken))
        {
            var simpleFeed = JsonSerializer.Deserialize<NewsStoryViewModel>(Encoding.UTF8.GetString(message.Body));
            
            if (simpleFeed is null) return;
            Console.WriteLine($"Received feed: {simpleFeed.Title}");
            await _receiver.CompleteMessageAsync(message, stoppingToken);
        }    
    }
}

public class NewsStoryViewModel
{
    public DateTimeOffset Published { get; set; }

    public string Title { get; set; }

    public string Uri { get; set; }
}
