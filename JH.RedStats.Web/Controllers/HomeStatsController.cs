using JH.RedStats.Interfaces;
using JH.RedStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace JH.RedStats.Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class HomeStatsController : ControllerBase
{
    private readonly ILogger<HomeStatsController> _logger;
    private readonly IPostUpVoteStatCounter _postUpVoteStatCounter;
    private readonly IUserPostStatCounter _userPostStatCounter;
    
    public HomeStatsController(ILogger<HomeStatsController> logger, IPostUpVoteStatCounter postUpCounter, IUserPostStatCounter userPostCounter)
    {
        _logger = logger;
        _postUpVoteStatCounter = postUpCounter;
        _userPostStatCounter = userPostCounter;
    }

    [HttpGet]
    public async Task<HomeStatsRes> Get([FromQuery] HomeStatsReq req)
    {
        _logger.Log( LogLevel.Trace, $"requested Subreddit: {req.subReddit}");
        // return await GetTestingData();
        var result = new HomeStatsRes();
        result.schemaVersion = "1";
        result.postUpVotes = _postUpVoteStatCounter.GetTopPostUpVotes();
        result.userPosts = _userPostStatCounter.GetTopUsersByPosts();
        return result;
    }
    
    private async Task<HomeStatsRes> GetTestingData()
    {
        var result = new HomeStatsRes();
        result.schemaVersion = "1";
        
        result.postUpVotes = new List<PostUpVoteStatsModel>();
        result.postUpVotes.Add(new PostUpVoteStatsModel(){ id = "123", count = 200, title = "Post 1"});
        result.postUpVotes.Add(new PostUpVoteStatsModel(){ id = "124", count = 300, title = "Post 2"});
        result.postUpVotes.Add(new PostUpVoteStatsModel(){ id = "125", count = 400, title = "Post 3"});

        result.userPosts = new List<UserPostStatModel>();
        result.userPosts.Add(new UserPostStatModel(){ id="user1", name = "User Name 1", count = 19});
        result.userPosts.Add(new UserPostStatModel(){ id="user2", name = "User Name 2", count = 20});

        return await Task.FromResult(result);
    }
}