namespace TWitcher3.Core.Games
{
    public abstract class GameIntegration
    {
        public abstract void Update();
        public abstract bool IsRunning { get; }
        public abstract bool Invoke(IGameCommandReference command);
    }
}
