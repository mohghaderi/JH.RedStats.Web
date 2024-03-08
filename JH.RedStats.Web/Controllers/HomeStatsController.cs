using JH.RedStats.Core.Stats;
using JH.RedStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace JH.RedStats.Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class HomeStatsController : ControllerBase
{
    private readonly ILogger<HomeStatsController> _logger;

    public HomeStatsController(ILogger<HomeStatsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<HomeStatsRes> Get([FromQuery] HomeStatsReq req)
    {
        _logger.Log( LogLevel.Trace, $"requested Subreddit: {req.subReddit}");
        var result = new HomeStatsRes();
        result.schemaVersion = "1";
        
        result.postUpVotes = new List<PostUpVoteStat>();
        result.postUpVotes.Add(new PostUpVoteStat(){ id = "123", count = 200, title = "Post 1"});
        result.postUpVotes.Add(new PostUpVoteStat(){ id = "124", count = 300, title = "Post 2"});
        result.postUpVotes.Add(new PostUpVoteStat(){ id = "125", count = 400, title = "Post 3"});

        result.userPosts = new List<UserPostStat>();
        result.userPosts.Add(new UserPostStat(){ id="user1", name = "User Name 1", count = 19});
        result.userPosts.Add(new UserPostStat(){ id="user2", name = "User Name 2", count = 20});

        return await Task.FromResult(result);
    }
}