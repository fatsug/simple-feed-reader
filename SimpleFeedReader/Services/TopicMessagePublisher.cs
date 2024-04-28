using System.Text;
using System.Text.Json;
using System.Threading;
using Azure.Messaging.ServiceBus;

namespace SimpleFeedReader.Services;

public class TopicMessagePublisher(IConfiguration configuration, ServiceBusClient serviceBusClient)
    : IMessagePublisher
{
    // the sender used to publish messages to the queue
    readonly ServiceBusSender _sender = serviceBusClient.CreateSender(configuration.GetValue<string>("ServiceBus:TopicName"));

    public async Task Publish<T>(T obj)
    {
        var objectAsText = JsonSerializer.Serialize(obj);
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(objectAsText));
        message.MessageId = typeof(T).Name;
        await _sender.SendMessageAsync(message, new CancellationToken());
    }

    public async Task Publish(string raw)
    {
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(raw));
        message.MessageId = "Raw";
        await _sender.SendMessageAsync(message, new CancellationToken());
    }
}