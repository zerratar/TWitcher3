using TwitchLib.Client.Models;

namespace TWitcher3.Providers
{
    public interface IConnectionCredentialsProvider
    {
        ConnectionCredentials Get();
    }
}