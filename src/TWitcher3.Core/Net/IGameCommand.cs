namespace TWitcher3.Core.Net
{
    public interface IGameCommand
    {
        string CorrelationId { get; }
        string Destination { get; }
        string Command { get; }
        string[] Args { get; }
    }
}