namespace JH.RedStats.Interfaces;

public class UserPostStatModel
{
    /**
     * Author Identifier
     */
    public string id { get; set; }
    /**
     * User Display name
     */
    public string name { get; set; }
    /**
     * Number of posts in one subreddit
     */
    public int count { get; set; }
}