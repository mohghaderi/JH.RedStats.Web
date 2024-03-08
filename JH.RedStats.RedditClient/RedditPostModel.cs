using JH.RedStats.Interfaces;

namespace JH.RedStats.RedditClient;

public class RedditPostModel : IRedditPostModel
{
    public string id { get; set; }
    public string title { get; set; }
    public long upVotes { get; set; }
    public string userId { get; set; }
}
