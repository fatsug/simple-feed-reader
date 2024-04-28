using System.Text;
using System.Text.Json;
using System.Threading;
using Azure.Messaging.ServiceBus;

namespace SimpleFeedReader.Services;

public class QueueMessagePublisher(IConfiguration configuration, ServiceBusClient serviceBusClient)
    : IMessagePublisher
{
    // the sender used to publish messages to the queue
    readonly ServiceBusSender _sender = serviceBusClient.CreateSender(configuration.GetValue<string>("ServiceBus:QueueName"));

    public async Task Publish<T>(T obj)
    {
        var objectAsText = JsonSerializer.Serialize(obj);
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(objectAsText));
        await _sender.SendMessageAsync(message, new CancellationToken());
    }

    public async Task Publish(string raw)
    {
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(raw));
        await _sender.SendMessageAsync(message, new CancellationToken());
    }
}