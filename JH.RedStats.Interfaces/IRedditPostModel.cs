namespace JH.RedStats.Interfaces;

public interface IRedditPostModel
{
    string id { get; set; }
    string title { get; set; }
    long upVotes { get; set; }
    string userId { get; set; }
}
