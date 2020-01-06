using System;

namespace TWitcher3.Core.Games
{
    public class GameCommandDefinition : IGameCommandDefinition
    {
        public GameCommandDefinition(
            string name, 
            string syntax,
            string description,
            long invokeCost, 
            float timeToLevelReduction, 
            TimeSpan globalCooldown, 
            TimeSpan localCooldown)
        {
            Name = name;
            Syntax = syntax;
            Description = description;
            InvokeCost = invokeCost;
            TimeToLevelReduction = timeToLevelReduction;
            GlobalCooldown = globalCooldown;
            LocalCooldown = localCooldown;
        }

        public string Name { get; }
        public string Syntax { get; }
        public string Description { get; }
        public long InvokeCost { get; }
        public float TimeToLevelReduction { get; }
        public TimeSpan GlobalCooldown { get; }
        public TimeSpan LocalCooldown { get; }
    }
}