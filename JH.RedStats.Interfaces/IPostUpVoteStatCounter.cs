namespace JH.RedStats.Interfaces;

public interface IPostUpVoteStatCounter
{
    void ConsumeQueue();
    List<PostUpVoteStatsModel> GetTopPostUpVotes();
}