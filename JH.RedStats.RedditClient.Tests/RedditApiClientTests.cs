using JH.RedStats.Core.Streaming;
using JH.RedStats.Interfaces;

namespace JH.RedStats.RedditClient.Tests;

public class RedditApiClientTests
{
    [Fact]
    public async Task Should_Get_List_of_SubReddit_Posts()
    {
        var subRedditName = RedditConnectionSettings.DefaultSubReddit;
        var eventsQueue = new RedditPostEventsQueue();

        var redditClient = new RedditApiClient(eventsQueue);
        var isMonitoringStarted = await redditClient.StartMonitoring(subRedditName);
        await Task.Delay(2000); // wait two seconds for posts to arrive
        await redditClient.StopMonitoring(subRedditName);

        Assert.True(isMonitoringStarted);

        var lastSeqNumber = eventsQueue.GetLastSeqNumber();
        
        Assert.True(lastSeqNumber > 0); // just ensure we have more than one post
    }
}