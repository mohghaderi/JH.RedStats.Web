using JH.RedStats.Interfaces;

namespace JH.RedStats.Core.Base;

public abstract class BaseEventProcessor
{
    private int _lastIndex = -1;
    private readonly IRedditPostEventsQueue _postsEventQueue;

    protected BaseEventProcessor(IRedditPostEventsQueue postsEventQueue)
    {
        _postsEventQueue = postsEventQueue;
    }

    public void ConsumeQueue()
    {
        var i = _lastIndex + 1; // start from the last processed event
        bool isDataChanged = false;
        for (; i <= _postsEventQueue.GetLastSeqNumber(); i++)
        {
            var postItem = _postsEventQueue.GetItemAt(i);
            var isEventProcessed = ProcessEvent(postItem);
            isDataChanged = isDataChanged || isEventProcessed;
        }

        // update the last seen index
        _lastIndex = i - 1;

        if (!isDataChanged) return; // if data didn't change we don't need to sort to recreate the lists

        AfterProcessEvent();
    }
    
    protected abstract bool ProcessEvent(IRedditPostEvent? e);
    protected abstract void AfterProcessEvent();
}
