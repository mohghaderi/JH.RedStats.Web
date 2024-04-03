using System.Diagnostics;
using JH.RedStats.Core.Base;
using JH.RedStats.Interfaces;

namespace JH.RedStats.Core.EventProcessors;

public class PostUpVoteStatCounter : BaseEventProcessor, IPostUpVoteStatCounter
{
    private const int NUMBER_OF_TOP_ITEMS = 50;
    private readonly List<PostUpVoteStatsModel> _upVoteStats = new List<PostUpVoteStatsModel>();
    private List<PostUpVoteStatsModel> _topVoteStats = new List<PostUpVoteStatsModel>();

    public PostUpVoteStatCounter(IRedditPostEventsQueue postsEventQueue)
        : base(postsEventQueue)
    {
        
    }

    protected override void AfterProcessEvent()
    {
        _upVoteStats.Sort((a,b) => b.count - a.count);
        _topVoteStats = CreateTopList();
    }

    protected override bool ProcessEvent(IRedditPostEvent? e)
    {
        bool isDataChanged = false;
        if (e == null) return isDataChanged;
        
        switch (e.eventType)
        {
            case RedditPostEventType.PostAdded:
                AddOrUpdatePostItem(e.postId, e.title, e.upVotes);
                isDataChanged = true;
                break;
            case RedditPostEventType.PostRemoved:
                AddOrUpdatePostItem(e.postId, e.title, 0);
                isDataChanged = true;
                break;
            case RedditPostEventType.PostVotesUpdated:
                AddOrUpdatePostItem(e.postId, e.title, e.upVotes);
                isDataChanged = true;
                break;
            default:
                Debug.Print($"Processing for event type is {e.eventType} is not defined");
                break;
        }

        return isDataChanged;
    }

    private void AddOrUpdatePostItem(string postId, string title, int count)
    {
        var item = _upVoteStats.FirstOrDefault(x => x.id == postId);
        if (item == null)
            _upVoteStats.Add(new PostUpVoteStatsModel()
            {
                id = postId,
                title = title,
                count = count
            });
        else
        {
            // update UpVotes
            item.count = count;
        }
    }

    private List<PostUpVoteStatsModel> CreateTopList()
    {
        var itemsCount =  Math.Max(Math.Min(_upVoteStats.Count, NUMBER_OF_TOP_ITEMS), NUMBER_OF_TOP_ITEMS);
        var result = new List<PostUpVoteStatsModel>();
        for (var i = 0; i < itemsCount; i++)
        {
            result.Add(_upVoteStats[i]);
        }

        return result;
    }

    public List<PostUpVoteStatsModel> GetTopPostUpVotes()
    {
        return _topVoteStats;
    }
}