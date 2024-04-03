using System.Diagnostics;
using JH.RedStats.Core.Base;
using JH.RedStats.Interfaces;

namespace JH.RedStats.Core.EventProcessors;

public class UserPostStatCounter: BaseEventProcessor, IUserPostStatCounter
{
    private const int NUMBER_OF_TOP_ITEMS = 50;
    private readonly List<UserPostStatModel> _userPostStats = new List<UserPostStatModel>();
    private List<UserPostStatModel> _topUserPosts = new List<UserPostStatModel>();

    public UserPostStatCounter(IRedditPostEventsQueue postsEventQueue)
    : base(postsEventQueue)
    {
        
    }

    protected override void AfterProcessEvent()
    {
        // we could use a Heap data structure and avoid sorting after processing,
        // however, this needed coding our own heap which is time-consuming
        // this should do the trick of counting for small amount of data
        _userPostStats.Sort((a,b) => b.count - a.count);
        _topUserPosts = CreateTopList();
    }
    

    protected override bool ProcessEvent(IRedditPostEvent? e)
    {
        bool isDataChanged = false;
        if (e == null) return isDataChanged;
        var item = _userPostStats.FirstOrDefault(x => x.id == e.authorId);
 
        // update counts
        switch (e.eventType)
        {
            case RedditPostEventType.PostAdded:
                if (item == null) 
                { 
                    // if we didn't have the item add it as count 1
                    item = new UserPostStatModel()
                    {
                        id = e.authorId,
                        name = e.authorName,
                        count = 1
                    };
                    // add the item to the list
                    _userPostStats.Add(item);
                }
                else
                {
                    item.count++;
                }
                isDataChanged = true;
                break;
            case RedditPostEventType.PostRemoved:
                if (item == null) break; // if post already removed ignore it
                item.count--; // reduce number of posts user made because the post is removed
                isDataChanged = true;
                break;
            case RedditPostEventType.PostVotesUpdated:
                // no change is needed when only post votes changed
                // (doesn't change the number of posts by one user)
                break;
            default:
                Debug.Print($"Processing for event type is {e.eventType} is not defined");
            break;
        }

        return isDataChanged;
    }

    private List<UserPostStatModel> CreateTopList()
    {
        var itemsCount = Math.Max(Math.Min(_userPostStats.Count, NUMBER_OF_TOP_ITEMS), NUMBER_OF_TOP_ITEMS);
        var result = new List<UserPostStatModel>();
        for (var i = 0; i < itemsCount; i++)
        {
            result.Add(_userPostStats[i]);
        }

        return result;
    }

    public List<UserPostStatModel> GetTopUsersByPosts()
    {
        return _topUserPosts;
    }
}