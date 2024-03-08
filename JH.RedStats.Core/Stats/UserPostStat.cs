namespace JH.RedStats.Core.Stats;

public class UserPostStat
{
    /**
     * User Identifier
     */
    public string id { get; set; }
    /**
     * User Display name
     */
    public string name { get; set; }
    /**
     * Number of posts in one subreddit
     */
    public long count { get; set; }
}