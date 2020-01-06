using System;

namespace TWitcher3.Core.Games
{
    public interface IGameCommand
    {
        string Name { get; }
        long InvokeCost { get; }
        float TimeToLevelReduction { get; }
        TimeSpan GlobalCooldown { get; }
        TimeSpan LocalCooldown { get; }
    }

    public interface IGameCommandReference : IGameCommand
    {
        string Sender { get; }
        string[] Arguments { get; }
    }

    public interface IGameCommandDefinition : IGameCommand
    {
        string Syntax { get; }
        string Description { get; }
    }
}