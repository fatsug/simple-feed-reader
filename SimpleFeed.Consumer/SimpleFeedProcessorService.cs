using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SimpleFeed.Consumer;

public class SimpleFeedProcessorService(IConfiguration configuration, ISimpleFeedProcessorFactory processorFactory) : BackgroundService
{
    private readonly ServiceBusProcessor _processor =
        processorFactory.Create(configuration.GetValue<string>("ServiceBus:QueueName")!);
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.StartProcessingAsync();
        return Task.CompletedTask;
    }
}