using System.Diagnostics;
using JH.RedStats.Interfaces;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
namespace JH.RedStats.RedditClient;

public class RedditApiClient : IRedditApiClient
{
    private readonly RedditApiConnectionPool _connectionPool;
    private readonly IRedditPostEventsQueue _subredditPostsQueue;
    private bool _isMonitoring;

    public RedditApiClient(IRedditPostEventsQueue queue)
    {
        _connectionPool = new RedditApiConnectionPool();
        _subredditPostsQueue = queue;
    }

    private Subreddit GetSubreddit(string subRedditName)
    {
        if (string.IsNullOrEmpty(subRedditName)) throw new ArgumentException("Subreddit name is necessary", nameof(subRedditName));
        var r = _connectionPool.GetClientInstance();
    
        // Get info on another subreddit.
        var subreddit = r.Subreddit(subRedditName);
        subreddit.Posts.NewUpdated += C_NewPostsUpdated;
        subreddit.Posts.TopUpdated += PostsOnTopUpdated;
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
            subreddit.Posts.MonitorTop();
            _isMonitoring = true;
            return true;
        }
        catch (Exception e)
        {
            Debug.Print(e.ToString()); // TODO: replace with crash handling
            return await Task.FromResult(false);
        }
    }

    public async Task<bool> StopMonitoring(string subRedditName)
    {
        if (!_isMonitoring) return false;
        var subreddit = GetSubreddit(subRedditName);
        if (!subreddit.Posts.NewPostsIsMonitored()) return false;

        subreddit.Posts.KillAllMonitoringThreads();
        return await Task.FromResult(true);
    }

    private void PostsOnTopUpdated(object? sender, PostsUpdateEventArgs e)
    {
        // Monitoring Top does not change up votes
        // we need to have our own monitoring, but it takes time to develop,
        // So, I assume this should be good enough for now.
        foreach (var post in e.NewPosts)
        {
            _subredditPostsQueue.Push(new RedditPostEvent()
            {
                postId = post.Id,
                title = post.Title,
                upVotes = post.UpVotes,
                authorId = post.Author,
                authorName = post.Author,
                eventType = RedditPostEventType.PostVotesUpdated
            });
            Console.WriteLine("NewPosts by " + post.Author + ": " + post.Title);
        }
    }

    private void C_NewPostsUpdated(object sender, PostsUpdateEventArgs e)
    {
        foreach (var post in e.Added)
        {
            _subredditPostsQueue.Push(new RedditPostEvent()
            {
                postId = post.Id,
                title = post.Title,
                upVotes = post.UpVotes,
                authorId = post.Author,
                authorName = post.Author,
                eventType = RedditPostEventType.PostAdded
            });
            Console.WriteLine("New Post by " + post.Author + ": " + post.Title);
        }
        foreach (var post in e.OldPosts)
        {
            Console.WriteLine("OldPosts by " + post.Author + ": " + post.Title);
        }
        foreach (var post in e.NewPosts)
        {
            _subredditPostsQueue.Push(new RedditPostEvent()
            {
                postId = post.Id,
                title = post.Title,
                upVotes = post.UpVotes,
                authorId = post.Author,
                authorName = post.Author,
                eventType = RedditPostEventType.PostAdded
            });
            Console.WriteLine("NewPosts by " + post.Author + ": " + post.Title);
        }
        foreach (var post in e.Removed)
        {
            _subredditPostsQueue.Push(new RedditPostEvent()
            {
                postId = post.Id,
                title = post.Title,
                upVotes = post.UpVotes,
                authorId = post.Author,
                authorName = post.Author,
                eventType = RedditPostEventType.PostRemoved
            });
            Console.WriteLine("Removed by " + post.Author + ": " + post.Title);
        }
    }

}