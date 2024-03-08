using JH.RedStats.Interfaces;

namespace JH.RedStats.RedditClient;

internal class RedditApiConnectionPool
{
    private Reddit.RedditClient _client = null;
    
    public Reddit.RedditClient GetClientInstance()
    {
        // Create a new Reddit.NET instance.
        if (_client == null)
        {
            _client = new Reddit.RedditClient(RedditConnectionSettings.AppId, RedditConnectionSettings.RefreshToken);
        }
        return _client;
    }
}