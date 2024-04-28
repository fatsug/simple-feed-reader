using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace SimpleFeed.Consumer;

public interface ISimpleFeedProcessorFactory
{
    public ServiceBusProcessor Create(string queueName);
}

public class SimpleFeedProcessorFactory(IConfiguration configuration, ServiceBusClient serviceBusClient) : ISimpleFeedProcessorFactory
{
    public ServiceBusProcessor Create(string queueName)
    {
        var processor = serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 2
        });

        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;

        return processor;
    }
    
    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        // string body = args.Message.Body.ToString();
        // Console.WriteLine(body);
        
        var simpleFeed = JsonSerializer.Deserialize<NewsStoryViewModel>(Encoding.UTF8.GetString(args.Message.Body));
            
        if (simpleFeed is null) return;
        Console.WriteLine($"Received feed: {simpleFeed.Title}");

        // we can evaluate application logic and use that to determine how to settle the message.
        await args.CompleteMessageAsync(args.Message);
    }

    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        // the error source tells me at what point in the processing an error occurred
        Console.WriteLine(args.ErrorSource);
        // the fully qualified namespace is available
        Console.WriteLine(args.FullyQualifiedNamespace);
        // as well as the entity path
        Console.WriteLine(args.EntityPath);
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}