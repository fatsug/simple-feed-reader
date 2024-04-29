using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DevOpsForAspNetCoreDevelopers
{
    public class SimpleReader_Functions
    {
        private readonly ILogger<SimpleReader_Functions> _logger;

        public SimpleReader_Functions(ILogger<SimpleReader_Functions> logger)
        {
            _logger = logger;
        }

        [Function(nameof(SimpleReader_Functions))]
        public void Run([ServiceBusTrigger("mytopic", "mysubscription", Connection = "")] ServiceBusReceivedMessage message)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
        }
    }
}
