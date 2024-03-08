namespace JH.RedStats.Interfaces;

public interface IRedditApiClient
{
    Task<bool> StartMonitoring(string subRedditName);
    IList<IRedditPostModel> GetSubredditPostsQueue();
}
