namespace JH.RedStats.Interfaces;

public interface IRedditPostEventsQueue
{
    void Push(IRedditPostEvent e);
    IRedditPostEvent? GetItemAt(int seqNumber);
    int GetLastSeqNumber();
}
