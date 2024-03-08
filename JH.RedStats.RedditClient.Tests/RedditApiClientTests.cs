using JH.RedStats.Interfaces;

namespace JH.RedStats.RedditClient.Tests;

public class RedditApiClientTests
{
    [Fact]
    public async Task Should_Get_List_of_SubReddit_Posts()
    {
        var subRedditName = RedditConnectionSettings.DefaultSubReddit;

        var redditClient = new RedditApiClient();
        var isMonitoringStarted = await redditClient.StartMonitoring(subRedditName);
        await Task.Delay(2000); // wait two seconds for posts to arrive
        redditClient.StopMonitoring(subRedditName);

        Assert.True(isMonitoringStarted);

        var results = redditClient.GetSubredditPostsQueue();
        
        Assert.True(results.Count > 0); // just ensure we have more than one post
    }
}