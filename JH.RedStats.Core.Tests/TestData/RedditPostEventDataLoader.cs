using JH.RedStats.Interfaces;

namespace JH.RedStats.Core.Tests.TestData;

public static class RedditPostEventDataLoader
{
    private static readonly List<RedditPostEvent> AllPostEvents = new List<RedditPostEvent>();
    
    public static List<RedditPostEvent> LoadAll()
    {
        if (AllPostEvents.Count > 0) return AllPostEvents; // if already initialized then return

        // 0
        AllPostEvents.Add(new RedditPostEvent()
        {
            schemaVersion = 1,
            title = "Test Post 1",
            eventType = RedditPostEventType.PostAdded,
            authorId = "author1",
            postId = "post1",
            subRedditName = "SubReddit1",
            authorName = "Author 1 Name",
            upVotes = 1
        });
        
        // 1
        AllPostEvents.Add(new RedditPostEvent()
        {
            schemaVersion = 1,
            title = "Test Post 2",
            eventType = RedditPostEventType.PostAdded,
            authorId = "author2",
            postId = "post2",
            subRedditName = "SubReddit1",
            authorName = "Author 2 Name",
            upVotes = 3
        });

        // 2
        AllPostEvents.Add(new RedditPostEvent()
        {
            schemaVersion = 1,
            title = "Test Post 1",
            eventType = RedditPostEventType.PostVotesUpdated,
            authorId = "author1",
            postId = "post1",
            subRedditName = "SubReddit1",
            authorName = "Author 1 Name",
            upVotes = 100
        });
        
        // 3
        AllPostEvents.Add(new RedditPostEvent()
        {
            schemaVersion = 1,
            title = "Test Post 3",
            eventType = RedditPostEventType.PostAdded,
            authorId = "author1",
            postId = "post3",
            subRedditName = "SubReddit1",
            authorName = "Author 1 Name",
            upVotes = 1
        });

        // 4
        AllPostEvents.Add(new RedditPostEvent()
        {
            schemaVersion = 1,
            title = "Test2",
            eventType = RedditPostEventType.PostRemoved,
            authorId = "author2",
            postId = "post2",
            subRedditName = "SubReddit1",
            authorName = "Author 2 Name",
        });

        return AllPostEvents;
    }
}