namespace JH.RedStats.Interfaces;

public interface IUserPostStatCounter
{
    void ConsumeQueue();
    List<UserPostStatModel> GetTopUsersByPosts();
}
