namespace JH.RedStats.Interfaces;

public interface IRedditApiClient
{
    Task<bool> StartMonitoring(string subRedditName);
    Task<bool> StopMonitoring(string subRedditName);
}
