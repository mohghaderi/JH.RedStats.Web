namespace JH.RedStats.Core.Stats;

public class PostUpVoteStat
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
    public long count { get; set; }
}