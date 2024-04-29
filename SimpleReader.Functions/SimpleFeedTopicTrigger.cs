using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DevOpsForAspNetCoreDevelopers;

public class SimpleFeedTopicTrigger
{
    private readonly ILogger<SimpleFeedTopicTrigger> _logger;

    public SimpleFeedTopicTrigger(ILogger<SimpleFeedTopicTrigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(SimpleFeedTopicTrigger))]
    public void Run([ServiceBusTrigger("exampletopic", "SimpleFeed", Connection = "ServiceBus")] ServiceBusReceivedMessage message)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
    }
}