namespace JH.RedStats.Interfaces;

public class RedditPostEvent : IRedditPostEvent
{
    public int schemaVersion { get; set; }
    public int seqNumber { get; set; }
    public string postId { get; set; }
    public string title { get; set; }
    public int upVotes { get; set; }
    public string authorId { get; set; }
    public string authorName { get; set; }
    public string subRedditName { get; set; }
    public RedditPostEventType eventType { get; set; }
}
