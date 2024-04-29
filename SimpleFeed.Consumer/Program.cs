using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleFeed.Consumer;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
// IConfigurationRoot config = new ConfigurationBuilder()
//     .AddJsonFile("appsettings.json")
//     .AddEnvironmentVariables()
//     .Build();
//
// var test = config.GetValue<string>("ServiceBus:ConnectionString");

builder.Services.AddAzureClients(b =>
    b.AddServiceBusClient(builder.Configuration.GetValue<string>("ServiceBus:ConnectionString")));
builder.Services.AddSingleton<ISimpleFeedProcessorFactory, SimpleFeedProcessorFactory>();

// LoggerProviderOptions.RegisterProviderOptions<
//     EventLogSettings, EventLogLoggerProvider>(builder.Services);

// builder.Services.AddHostedService<SimpleFeedReceiverService>();
builder.Services.AddHostedService<SimpleFeedProcessorService>();
builder.Services.AddHostedService<SimpleFeedTopicReceiverService>();

IHost host = builder.Build();
host.Run();