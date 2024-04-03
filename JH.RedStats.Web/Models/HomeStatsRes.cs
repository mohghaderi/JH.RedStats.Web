using JH.RedStats.Interfaces;

namespace JH.RedStats.Web.Models;

public class HomeStatsRes
{
    public string schemaVersion { get; set; }
    public IList<PostUpVoteStatsModel> postUpVotes { get; set; }
    public IList<UserPostStatModel> userPosts { get; set; }
}
