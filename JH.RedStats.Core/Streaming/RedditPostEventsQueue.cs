using JH.RedStats.Interfaces;

namespace JH.RedStats.Core.Streaming;

/**
 * Note: SeqNumber is just increasing in value. So, program will fail after MAX_INT
 * To avoid that, we can add a rotation or use a limited size list with replacing or dropping events
 * This class can also be extended to support storing of the events.
 * Instead of using an in-memory queue, it push the events to Kafka (or similar),
 * and also let the clients to stream these events from it.
 */
public class RedditPostEventsQueue : IRedditPostEventsQueue
{
    private IList<IRedditPostEvent> _listQueue;
    private object _lockObject = new Object();

    public RedditPostEventsQueue()
    {
        _listQueue = new List<IRedditPostEvent>();
    }

    public void Push(IRedditPostEvent e)
    {
        // let's make sure sequence numbers are not duplicated by locking the add process
        lock (_lockObject)
        {
            e.seqNumber = _listQueue.Count;
            _listQueue.Add(e);
        }
    }

    public IRedditPostEvent? GetItemAt(int seqNumber)
    {
        // we don't need thread safety in read so we can ignore locks to speed things up.
        if (seqNumber >= _listQueue.Count) return null;
        return _listQueue[seqNumber];
    }

    
    public int GetLastSeqNumber()
    {
        return _listQueue.Count - 1;
    }
}
