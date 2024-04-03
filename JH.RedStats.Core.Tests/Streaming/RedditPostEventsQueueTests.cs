using JH.RedStats.Core.Streaming;
using JH.RedStats.Core.Tests.TestData;

namespace JH.RedStats.Core.Tests.Streaming;

public class RedditPostEventsQueueTests
{
    [Fact]
    public void Should_queue_events_and_return_results()
    {
        var target = new RedditPostEventsQueue();
        var data = RedditPostEventDataLoader.LoadAll();

        target.Push(data[0]);
        target.Push(data[1]);
        target.Push(data[2]);

        var lastSeqNumber = target.GetLastSeqNumber();
        Assert.Equal(2, lastSeqNumber);

        var itemIndex1 = target.GetItemAt(1);
        Assert.Equal(1, itemIndex1?.seqNumber);

        var itemOutSideSeq = target.GetItemAt(100);
        Assert.Null(itemOutSideSeq);
    }
}