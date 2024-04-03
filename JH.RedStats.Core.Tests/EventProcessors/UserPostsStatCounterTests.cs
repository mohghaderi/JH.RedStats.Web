using JH.RedStats.Core.EventProcessors;
using JH.RedStats.Core.Streaming;
using JH.RedStats.Core.Tests.TestData;

namespace JH.RedStats.Core.Tests.EventProcessors;

public class UserPostStatCounterTests
{
    private RedditPostEventsQueue GetDefaultQueue()
    {
        // has only the first 3 items
        var queue = new RedditPostEventsQueue();
        var data = RedditPostEventDataLoader.LoadAll(); 
        queue.Push(data[0]);
        queue.Push(data[1]);
        queue.Push(data[2]);
        queue.Push(data[3]);
        return queue;
    }

    private RedditPostEventsQueue GetQueueWithRemovedItem()
    {
        var queue = GetDefaultQueue();
        var data = RedditPostEventDataLoader.LoadAll(); 
        queue.Push(data[4]);
        return queue;
    }
    
    [Fact]
    public void Should_have_init_top_posts_as_empty_list_with_zero_items()
    {
        var target = new UserPostStatCounter(GetDefaultQueue());

        var initTopPostsCount = target.GetTopUsersByPosts().Count;
        
        Assert.Equal(0, initTopPostsCount);
    }
    

    [Fact]
    public void Should_have_users_with_count_of_their_articles()
    {
        var target = new UserPostStatCounter(GetDefaultQueue());

        target.ConsumeQueue();

        var topPosts = target.GetTopUsersByPosts();
        Assert.Equal(2, topPosts.Count);
        Assert.Equal("author1", topPosts[0].id);
        Assert.Equal(2, topPosts[0].count);
    }

    [Fact]
    public void Should_set_count_as_zero_when_post_is_removed()
    {
        var target = new UserPostStatCounter(GetQueueWithRemovedItem());

        target.ConsumeQueue();

        var topPosts = target.GetTopUsersByPosts();
        Assert.Equal("author2", topPosts[1].id);
        Assert.Equal(0, topPosts[1].count);
    }
}