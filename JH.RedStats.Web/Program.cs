using JH.RedStats.Core.EventProcessors;
using JH.RedStats.Core.Streaming;
using JH.RedStats.Interfaces;
using JH.RedStats.RedditClient;

namespace JH.RedStats.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllersWithViews();

        // add dependency modules
        AddDependencies(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        StartServices(app);

        app.Run();
    }

    private static void StartServices(WebApplication app)
    {
        StartRedditMonitoringService(app);
        StartCounterThreads(app);
    }

    private static void StartCounterThreads(WebApplication app)
    {
        Console.WriteLine("StartCounterThreads");

        var postUpVoteStatCounter = app.Services.GetRequiredService<IPostUpVoteStatCounter>();
        var userPostStatCounter = app.Services.GetRequiredService<IUserPostStatCounter>();
        var stopThreadSignal = false;

        var counterThread = new Thread(() =>
        {
            Console.WriteLine("CounterThreads inner loop started");
            while (true)
            {
                postUpVoteStatCounter.ConsumeQueue();
                userPostStatCounter.ConsumeQueue();
                Thread.Sleep(RedditConnectionSettings.CounterStatsTicksMs);
                if (stopThreadSignal) break;
            }
            Console.WriteLine("CounterThreads ended");
        });
            
        // register application end events
        app.Lifetime.ApplicationStopping.Register(() =>
        {
            Console.WriteLine("CounterThreads Stopping");
            stopThreadSignal = true;
            // server is not going to shutdown 
            // until the callback is done
        });

        counterThread.Start();
        Console.WriteLine("CounterThreads Started");
    }


    private static void StartRedditMonitoringService(WebApplication app)
    {
        Console.WriteLine("StartRedditMonitoringService");
        // start monitoring client
        var redditMonitoring = app.Services.GetRequiredService<IRedditApiClient>();

        // register application end events
        app.Lifetime.ApplicationStopping.Register(() =>
        {
            Console.WriteLine("RedditMonitoringService ApplicationStopping");
            Task.Run(() => redditMonitoring.StopMonitoring(RedditConnectionSettings.DefaultSubReddit));
            // server is not going to shutdown 
            // until the callback is done
        });

        Task.Run(() => redditMonitoring.StartMonitoring(RedditConnectionSettings.DefaultSubReddit));
    }

    private static void AddDependencies(WebApplicationBuilder builder)
    {
        Console.WriteLine("Registering Dependencies");
        builder.Services.AddSingleton<IUserPostStatCounter, UserPostStatCounter>();
        builder.Services.AddSingleton<IPostUpVoteStatCounter, PostUpVoteStatCounter>();
        builder.Services.AddSingleton<IRedditPostEventsQueue, RedditPostEventsQueue>();
        builder.Services.AddSingleton<IRedditApiClient, RedditApiClient>();
    }
}