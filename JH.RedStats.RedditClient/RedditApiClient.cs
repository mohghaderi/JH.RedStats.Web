using JH.RedStats.Interfaces;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
namespace JH.RedStats.RedditClient;

public class RedditApiClient : IRedditApiClient
{
    private readonly RedditApiConnectionPool _connectionPool;
    private IList<IRedditPostModel> _subredditPostsQueue;
    private bool _isMonitoring;
    
    public RedditApiClient()
    {
        _connectionPool = new RedditApiConnectionPool();
        _subredditPostsQueue = new List<IRedditPostModel>();
    }

    public IList<IRedditPostModel> GetSubredditPostsQueue()
    {
        return _subredditPostsQueue;
    }
    //
    // public async Task<IList<IRedditPostModel>> GetSubRedditPosts(string subRedditName)
    // {
    //     var result = new List<IRedditPostModel>();
    //
    //     var r = _connectionPool.GetClientInstance();
    //
    //     //         // Display the name and cake day of the authenticated user.
    //     // Console.WriteLine("Username: " + r.Account.Me.Name);
    //     // Console.WriteLine("Cake Day: " + r.Account.Me.Created.ToString("D"));
    //
    //     // Get info on another subreddit.
    //     var askReddit = r.Subreddit(subRedditName).About();
    //
    //     var posts = askReddit.Posts.GetNew();
    //
    //     foreach (var post in posts)
    //     {
    //         result.Add(new RedditPostModel()
    //         {
    //             id = post.Id,
    //             title = post.Title,
    //             upVotes = post.UpVotes,
    //             userId = post.Author
    //         });
    //     }
    //
    //     // askReddit.Posts.GetNew();
    //     // askReddit.Posts.NewUpdated += C_NewPostsUpdated;
    //     askReddit.Posts.MonitorNew();
    //     
    //     askReddit.Posts.Monitor
    //
    //     // // Stop monitoring r/AskReddit for new posts.
    //     // askReddit.Posts.MonitorNew();
    //     // askReddit.Posts.NewUpdated -= C_NewPostsUpdated;
    //
    //     return await Task.FromResult(result);
    // }
    
    
    private Subreddit GetSubreddit(string subRedditName)
    {
        if (string.IsNullOrEmpty(subRedditName)) throw new ArgumentException("Subreddit name is necessary", nameof(subRedditName));
        var r = _connectionPool.GetClientInstance();
    
        // Get info on another subreddit.
        var subreddit = r.Subreddit(subRedditName);
        subreddit.Posts.NewUpdated += C_NewPostsUpdated;
        return subreddit;
    }

    
    public async Task<bool> StartMonitoring(string subRedditName)
    {
        if (_isMonitoring)
            throw new Exception("Monitoring has started before. Currently, only one monitoring can be done.");

        var subreddit = GetSubreddit(subRedditName);

        if (subreddit.Posts.NewPostsIsMonitored())
        {
            Console.WriteLine("The posts are already being monitored.");
            return true;
        }

        try
        {
            subreddit.Posts.MonitorNew();
            _isMonitoring = true;
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task StopMonitoring(string subRedditName)
    {
        if (!_isMonitoring) return;
        var subreddit = GetSubreddit(subRedditName);
        if (!subreddit.Posts.NewPostsIsMonitored()) return;

        subreddit.Posts.KillAllMonitoringThreads();
    }
    

    public void C_NewPostsUpdated(object sender, PostsUpdateEventArgs e)
    {
        foreach (var post in e.Added)
        {
            _subredditPostsQueue.Add(new RedditPostModel()
            {
                id = post.Id,
                title = post.Title,
                upVotes = post.UpVotes,
                userId = post.Author
            });
            Console.WriteLine("New Post by " + post.Author + ": " + post.Title);
        }
        foreach (var post in e.OldPosts)
        {
            Console.WriteLine("OldPosts by " + post.Author + ": " + post.Title);
        }
        foreach (var post in e.NewPosts)
        {
            Console.WriteLine("NewPosts by " + post.Author + ": " + post.Title);
        }
        foreach (var post in e.Removed)
        {
            Console.WriteLine("Removed by " + post.Author + ": " + post.Title);
        }
    }

}