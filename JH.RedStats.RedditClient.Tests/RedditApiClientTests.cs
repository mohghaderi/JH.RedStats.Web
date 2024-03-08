using JH.RedStats.Interfaces;

namespace JH.RedStats.RedditClient.Tests;

public class RedditApiClientTests
{
    [Fact]
    public async Task Should_Get_List_of_SubReddit_Posts()
    {
        var redditClient = new RedditApiClient();
        var results = await redditClient.GetSubRedditPosts(RedditConnectionSettings.DefaultSubReddit);
        Assert.True(results.Count > 0); // just ensure we have more than one post
    }
}