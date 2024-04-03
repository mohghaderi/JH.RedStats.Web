namespace JH.RedStats.Interfaces;

public class PostUpVoteStatsModel
{
    /**
     * Post Identifier
     */
    public string id { get; set; }
    /**
     * Post Title
     */
    public string title { get; set; }
    /**
     * Number of UpVotes in one subreddit
     */
    public int count { get; set; }
}