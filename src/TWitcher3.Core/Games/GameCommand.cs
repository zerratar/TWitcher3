using System;

namespace TWitcher3.Core.Games
{
    public class GameCommand : IGameCommandReference
    {
        public GameCommand(
            string sender,
            string name,
            string[] arguments,
            long invokeCost, 
            float timeToLevelReduction,
            TimeSpan globalCooldown, 
            TimeSpan localCooldown)
        {
            Sender = sender;
            Arguments = arguments;
            Name = name;
            InvokeCost = invokeCost;
            TimeToLevelReduction = timeToLevelReduction;
            GlobalCooldown = globalCooldown;
            LocalCooldown = localCooldown;
        }

        public string Sender { get; }
        public string[] Arguments { get; }
        public string Name { get; }
        public long InvokeCost { get; }
        public float TimeToLevelReduction { get; }
        public TimeSpan GlobalCooldown { get; }
        public TimeSpan LocalCooldown { get; }
    }
}