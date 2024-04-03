using JH.RedStats.Core.EventProcessors;
using JH.RedStats.Core.Streaming;
using JH.RedStats.Core.Tests.TestData;

namespace JH.RedStats.Core.Tests.EventProcessors;

public class PostUpVoteStatCounterTests
{
    private RedditPostEventsQueue GetDefaultQueue()
    {
        // has only the first 3 items
        var queue = new RedditPostEventsQueue();
        var data = RedditPostEventDataLoader.LoadAll(); 
        queue.Push(data[0]);
        queue.Push(data[1]);
        queue.Push(data[2]);
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
        var target = new PostUpVoteStatCounter(GetDefaultQueue());

        var initTopPostsCount = target.GetTopPostUpVotes().Count;
        
        Assert.Equal(0, initTopPostsCount);
    }
    

    [Fact]
    public void Should_have_posts_with_top_votes_count()
    {
        var target = new PostUpVoteStatCounter(GetDefaultQueue());

        target.ConsumeQueue();

        var topPosts = target.GetTopPostUpVotes();
        Assert.Equal(2, topPosts.Count);
        Assert.Equal("post1", topPosts[0].id);
        Assert.Equal(100, topPosts[0].count);
    }

    [Fact]
    public void Should_set_count_as_zero_when_post_is_removed()
    {
        var target = new PostUpVoteStatCounter(GetQueueWithRemovedItem());

        target.ConsumeQueue();

        var topPosts = target.GetTopPostUpVotes();
        Assert.Equal("post2", topPosts[1].id);
        Assert.Equal(0, topPosts[1].count);
    }
}