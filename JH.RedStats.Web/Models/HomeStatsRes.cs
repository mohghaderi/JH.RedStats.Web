using JH.RedStats.Core.Stats;

namespace JH.RedStats.Web.Models;

public class HomeStatsRes
{
    public string schemaVersion { get; set; }
    public IList<PostUpVoteStat> postUpVotes { get; set; }
    public IList<UserPostStat> userPosts { get; set; }
}
