namespace JH.RedStats.Interfaces;

public interface IRedditApiClient
{
    Task<IList<IRedditPostModel>> GetSubRedditPosts(string subRedditName);
}
