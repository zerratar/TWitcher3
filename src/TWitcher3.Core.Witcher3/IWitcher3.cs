namespace TWitcher3.Core.Games.Witcher3
{
    public interface IWitcher3
    {
        void Update();
        bool IsRunning { get; }
        bool Invoke(IGameCommandReference command);
        bool Notify(string message);
    }
}