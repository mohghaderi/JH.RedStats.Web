namespace JH.RedStats.Interfaces;

public static class RedditConnectionSettings
{
    // TODO: Move the client settings to appsettings.json file
    public static readonly string DefaultSubReddit = "AskReddit";


    // Connection Settings
    public static readonly string AppId = "825Wbpxe0t5KtXRpI5TulA";
    public static readonly string AppSecret = "";

    // Obtain this from JH.RedStats.RedditOauth.Console and paste it here
    public static readonly string RefreshToken = "645362134361-tUIMoCnrKpLXI1grMGw851IlZIawYA";

    // tick wait time to consume items in the queue
    public static readonly int CounterStatsTicksMs = 100;
}
