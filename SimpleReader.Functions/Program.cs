using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    // .ConfigureHostConfiguration(builder => builder.AddUserSecrets<Program>())
    .ConfigureAppConfiguration(builder => builder.AddUserSecrets<Program>())
    .Build();

host.Run();
