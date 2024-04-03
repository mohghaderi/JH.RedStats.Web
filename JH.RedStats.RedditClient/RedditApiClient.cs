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
    private bool _isStopMonitoringRequested;
    private string _lastNewPost = String.Empty;

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
        // subreddit.Posts.NewUpdated += C_NewPostsUpdated;
        // subreddit.Posts.TopUpdated += PostsOnTopUpdated;
        return subreddit;
    }

    public async Task<bool> StartMonitoring(string subRedditName)
    {
        if (_isMonitoring)
            throw new Exception("Monitoring has started before. Currently, only one monitoring can be done.");

        new Thread(() =>
        {
            Console.WriteLine("Entering monitoring thread.");
            _isMonitoring = true;
            while (true)
            {
                MonitorThread(subRedditName);
                if(_isStopMonitoringRequested) break;
            }
            Console.WriteLine("Ending monitoring thread.");
        }).Start();

        return await Task.FromResult(true);

        // TODO: Remove the extra code for MonitorNew, add code for handling removal of posts
        // var subreddit = GetSubreddit(subRedditName);
        // if (subreddit.Posts.NewPostsIsMonitored())
        // {
        //     Console.WriteLine("The posts are already being monitored.");
        //     return true;
        // }
        //
        // try
        // {
        //     subreddit.Posts.MonitorNew();
        //     subreddit.Posts.MonitorTop();
        //     _isMonitoring = true;
        //     return true;
        // }
        // catch (Exception e)
        // {
        //     Debug.Print(e.ToString()); // TODO: replace with crash handling
        //     return await Task.FromResult(false);
        // }
    }

    public async Task<bool> StopMonitoring(string subRedditName)
    {
        if (!_isMonitoring) return false;
        _isStopMonitoringRequested = true;
        var subreddit = GetSubreddit(subRedditName);
        if (!subreddit.Posts.NewPostsIsMonitored()) return false;

        subreddit.Posts.KillAllMonitoringThreads();
        return await Task.FromResult(true);
    }
    
    
    public void MonitorThread(string subredditName)
    {
        // update top posts
        var topPosts = GetSubreddit(subredditName).Posts.GetTop();
        foreach (var post in topPosts)
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
        }

        // update new posts
        var newPosts = GetSubreddit(subredditName).Posts.GetNew(_lastNewPost);
        foreach (var post in newPosts)
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

        if (newPosts.Count > 0)
        {
            _lastNewPost = newPosts[0].Fullname;
        }
    }
    //
    // private void PostsOnTopUpdated(object? sender, PostsUpdateEventArgs e)
    // {
    //     // Monitoring Top does not change up votes
    //     // we need to have our own monitoring, but it takes time to develop,
    //     // So, I assume this should be good enough for now.
    //     foreach (var post in e.NewPosts)
    //     {
    //         _subredditPostsQueue.Push(new RedditPostEvent()
    //         {
    //             postId = post.Id,
    //             title = post.Title,
    //             upVotes = post.UpVotes,
    //             authorId = post.Author,
    //             authorName = post.Author,
    //             eventType = RedditPostEventType.PostVotesUpdated
    //         });
    //         Console.WriteLine("NewPosts by " + post.Author + ": " + post.Title);
    //     }
    // }
    //
    // private void C_NewPostsUpdated(object sender, PostsUpdateEventArgs e)
    // {
    //     foreach (var post in e.Added)
    //     {
    //         _subredditPostsQueue.Push(new RedditPostEvent()
    //         {
    //             postId = post.Id,
    //             title = post.Title,
    //             upVotes = post.UpVotes,
    //             authorId = post.Author,
    //             authorName = post.Author,
    //             eventType = RedditPostEventType.PostAdded
    //         });
    //         Console.WriteLine("New Post by " + post.Author + ": " + post.Title);
    //     }
    //     foreach (var post in e.OldPosts)
    //     {
    //         Console.WriteLine("OldPosts by " + post.Author + ": " + post.Title);
    //     }
    //     foreach (var post in e.NewPosts)
    //     {
    //         _subredditPostsQueue.Push(new RedditPostEvent()
    //         {
    //             postId = post.Id,
    //             title = post.Title,
    //             upVotes = post.UpVotes,
    //             authorId = post.Author,
    //             authorName = post.Author,
    //             eventType = RedditPostEventType.PostAdded
    //         });
    //         Console.WriteLine("NewPosts by " + post.Author + ": " + post.Title);
    //     }
    //     foreach (var post in e.Removed)
    //     {
    //         _subredditPostsQueue.Push(new RedditPostEvent()
    //         {
    //             postId = post.Id,
    //             title = post.Title,
    //             upVotes = post.UpVotes,
    //             authorId = post.Author,
    //             authorName = post.Author,
    //             eventType = RedditPostEventType.PostRemoved
    //         });
    //         Console.WriteLine("Removed by " + post.Author + ": " + post.Title);
    //     }
    // }

}