namespace JH.RedStats.Interfaces;

public interface IRedditPostEvent
{
    int schemaVersion { get; set; }
    int seqNumber { get; set; }
    string postId { get; set; }
    string title { get; set; }
    int upVotes { get; set; }
    string authorId { get; set; }
    string authorName { get; set; }
    string subRedditName { get; set; }
    RedditPostEventType eventType { get; set; }
}
