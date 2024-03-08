using JH.RedStats.Interfaces;
using Reddit.Controllers.EventArgs;
namespace JH.RedStats.RedditClient;

public class RedditApiClient : IRedditApiClient
{
    private readonly RedditApiConnectionPool _connectionPool;
    
    public RedditApiClient()
    {
        _connectionPool = new RedditApiConnectionPool();
    }

    public async Task<IList<IRedditPostModel>> GetSubRedditPosts(string subRedditName)
    {
        var result = new List<IRedditPostModel>();

        var r = _connectionPool.GetClientInstance();
    
        //         // Display the name and cake day of the authenticated user.
        // Console.WriteLine("Username: " + r.Account.Me.Name);
        // Console.WriteLine("Cake Day: " + r.Account.Me.Created.ToString("D"));
        //
        // // Retrieve the authenticated user's recent post history.
        // // Change "new" to "newForced" if you don't want older stickied profile posts to appear first.
        // var postHistory = r.Account.Me.GetPostHistory(sort: "new");
        //
        // // Retrieve the authenticated user's recent comment history.
        // var commentHistory = r.Account.Me.GetCommentHistory(sort: "new");
        //
        // // Create a new subreddit.
        // var mySub = r.Subreddit("MyNewSubreddit", "My subreddit's title", "Description", "Sidebar").Create();

        // Get info on another subreddit.
        var askReddit = r.Subreddit(subRedditName).About();

        // Get the top post from a subreddit.
        var topPost = askReddit.Posts.Top[0];
        Console.WriteLine(topPost.Author[0]);

        // // Create a new self post.
        // var mySelfPost = mySub.SelfPost("Self Post Title", "Self post text.").Submit();
        //
        // // Create a new link post.
        // // Use .Submit(resubmit: true) instead to force resubmission of duplicate links.
        // var myLinkPost = mySub.LinkPost("Link Post Title", "http://www.google.com").Submit();  

        // // Comment on a post.
        // var myComment = myLinkPost.Reply("This is my comment.");

        // // Reply to a comment.
        // var myCommentReply = myComment.Reply("This is my comment reply.");

        // // Create a new subreddit, then create a new link post on said subreddit,
        // // then comment on said post, then reply to said comment, then delete said comment reply.
        // // Because I said so.
        // r.Subreddit("MySub", "Title", "Desc", "Sidebar")
        // .Create()
        // .SelfPost("MyPost")
        // .Submit()
        // .Reply("My comment.")
        // .Reply("This comment will be deleted.")
        // .Delete();

        // Asynchronously monitor r/AskReddit for new posts.
        
        var posts = askReddit.Posts.GetNew();
        
        foreach (var post in posts)
        {
            result.Add(new RedditPostModel()
            {
                id = post.Id,
                title = post.Title,
                upVotes = post.UpVotes,
                userId = post.Author
            });
        }

        // askReddit.Posts.GetNew();
        // askReddit.Posts.NewUpdated += C_NewPostsUpdated;
        // askReddit.Posts.MonitorNew();
        //
        // // Stop monitoring r/AskReddit for new posts.
        // askReddit.Posts.MonitorNew();
        // askReddit.Posts.NewUpdated -= C_NewPostsUpdated;

        return await Task.FromResult(result);
    }

    public static void C_NewPostsUpdated(object sender, PostsUpdateEventArgs e)
    {
        foreach (var post in e.Added)
        {
            Console.WriteLine("New Post by " + post.Author + ": " + post.Title);
        }
    }

}