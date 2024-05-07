using Microsoft.Extensions.Azure;

namespace SimpleFeedReader;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<NewsService>();
        services.AddAutoMapper(typeof(Startup));

        services.AddRazorPages();
        
        var test = Configuration.GetValue<string>("ServiceBus:ConnectionString");

        services.AddAzureClients(b =>
            b.AddServiceBusClient(Configuration.GetValue<string>("ServiceBus:ConnectionString")));

        services.AddKeyedSingleton<IMessagePublisher, QueueMessagePublisher>("Queue");
        services.AddKeyedSingleton<IMessagePublisher, TopicMessagePublisher>("Topic");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
    }
}