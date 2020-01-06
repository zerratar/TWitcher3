using System;
using System.Collections.Concurrent;

namespace TWitcher3.Core.Games.Witcher3
{
    public class Witcher3Commands
    {
        private static readonly ConcurrentDictionary<string, IGameCommandDefinition> commandList
            = new ConcurrentDictionary<string, IGameCommandDefinition>();

        static Witcher3Commands()
        {
            // custom
            AddCommand(50, 5f, "notify", "notify('message')", "This console command will display a notification ingame.");
            AddCommand(50, 5f, "fall", "fall(optional Distance : float)", "This console command will make the player fall from the given distance, default is 6");
            AddCommand(50, 5f, "setplayerscale", "setplayerscale(x: float, y: float, z: float)", "This console command will scale the player character.");
            AddCommand(50, 5f, "setscale", "setscale(x: float, y: float, z: float)", "This console command will scale the target entity.");
            AddCommand(50, 5f, "autowalk", "autowalk(speed: float)", "This console command will make the player automatically walk forward with the target speed.");

            // default
            AddCommand(50, 5f, "addkeys", "addkeys", "This console command will give you all keys required to open all doors.");
            AddCommand(50, 5f, "addmoney", "addmoney(Amount)", "This console command will add the specified amount of money (Crowns) to your balance.");
            AddCommand(50, 5f, "removemoney", "removemoney(Amount)", "This command will remove the specified amount of money (Crowns) from your balance.");
            AddCommand(50, 5f, "additem", "additem('Item Code', Amount)", "This command adds the item with the specified item code to your inventory. The item code should be surrounded by apostrophes: additem('Dye Red'), not additem(Dye Red). You can optionally specify an amount - if not specified, 1 of the item with the specified item code will be added to your inventory. Find a list of all item codes at https://commands.gg/witcher3/items");
            AddCommand(50, 5f, "removeitem", "removeitem('Item Code')", "This command removes the specified item from your character's inventory. Find a list of all item codes at https://commands.gg/witcher3/items");
            AddCommand(50, 5f, "god", "god", "This command will toggle god mode for your character. When in god mode, you are invincible and will not take any damage.");
            AddCommand(50, 5f, "healme", "healme", "This console command will set your health to full.");
            AddCommand(50, 5f, "likeaboss", "likeaboss", "This is a toggle command (typing it again will turn it off). When likeaboss mode is enabled, all damage you deal will be 40% of the recipient's maximum health level. If the NPC you damage has a maximum health of 10,000, with this mode enabled you would deal 4,000 damage with each hit.");
            AddCommand(50, 5f, "Ciri", "Ciri", "This command will switch your character to Cirilla.");
            AddCommand(50, 5f, "Geralt", "Geralt", "This command will switch your character to Geralt.");
            AddCommand(50, 5f, "setlevel", "setlevel(Level)", "This command sets your level to the specified number.");
            AddCommand(50, 5f, "levelup", "levelup", "This command levels you up one level (use the setlevel command to level up faster).");
            AddCommand(50, 5f, "addexp", "addexp(Amount)", "This command gives you the specified amount of experience.");
            AddCommand(50, 5f, "learnskill", "learnskill('Skill ID')", "This command will make Geralt learn the skill with the specified skill ID (also known as a talent code). You need to surround the skill ID with apostrophes: learnskill('sword_s3') is correct, learnskill(sword_s3) is not correct. Find a list of all skill IDs at https://commands.gg/witcher3/skills");
            AddCommand(50, 5f, "Cat", "Cat(0 / 1)", "This command will enable and disable the ability to see in the dark: cat(1) to enable cat vision, cat(0) to return to normal.");
            AddCommand(50, 5f, "Drunk", "Drunk(0 / 1)", "This command will enable and disable 'Drunk Mode', in which your vision is distorted and dialogue between some characters (e.g. Shani, Triss) is changed (and quite funny!).");
            AddCommand(50, 5f, "shave", "shave", "This command will shave your beard. Note that this isn't a toggle command - you can't run this command again to re-grow your beard, you will have to use another command.");
            AddCommand(50, 5f, "settattoo", "settattoo(0 / 1)", "This command can be used to show and hide the tattoo from the Witcher 2 quest Hung Over that remains on the neck of Geralt.");
            AddCommand(50, 5f, "spawn", "spawn('NPC ID', Amount, Distance, true / false)", "This command will spawn the NPC with the specified NPC ID (also known as an NPC code, or character code). You can optionally specify an amount of the NPC to spawn, a distance (away from you) and whether the NPC should be hostile (true) or friendly (false). Find a list of all NPC spawn codes at https://commands.gg/witcher3/npcs");
            AddCommand(50, 5f, "killall", "killall(Distance)", "This command will kill all nearby enemies. If a distance is specified, all enemies within that distance from your character will be killed.");
            AddCommand(50, 5f, "makeitrain", "makeitrain", "This command starts a storm. Use the stoprain command to stop rain.");
            AddCommand(50, 5f, "stoprain", "stoprain", "This command stops any ongoing storms or rain. Use the makeitrain command to start a storm.");
            AddCommand(50, 5f, "ShowAllFT", "ShowAllFT(0 / 1)", "This command will show all Fast Travel pins on the map. It is recommended that you save your game before running this command, as some players have been unable to undo the effects of this command.");
            AddCommand(50, 5f, "ShowPins", "ShowPins(0 / 1)", "This command will show all pins on the map. It is recommended that you save your game before running this command, as some players have been unable to undo the effects of this command.");
            AddCommand(50, 5f, "secretgwint", "secretgwint", "This command will start a Gwent Game.");
            AddCommand(50, 5f, "winGwint", "winGwint(true / false)", "This console command will either instantly win your current Gwent Game (true), or instantly lose your current Gwent Game (false).");
            AddCommand(50, 5f, "addgwintcards", "addgwintcards", "This console gives you one of each Gwent Card, aside from the Vampire: Katakan Card. You can add the Vampire: Katakan Card with the additem('gwint_card_katakan') command.");
            AddCommand(50, 5f, "addabl", "addabl('Buff ID')", "This command will give you the buff with the specified buff ID. The buff ID should be in apostrophes: addabl('ForceCriticalHits') is correct, addabl(ForceCriticalHits) is not. Use rmvabl to remove a buff. Find a list of all buff IDs at https://commands.gg/witcher3/buffs");
            AddCommand(50, 5f, "rmvabl", "rmvabl('Buff ID')", "This command will remove the buff with the specified buff ID. The buff ID should be surrounded by apostrophes: rmvabl('ForceCriticalHits') is correct, rmvabl(ForceCriticalHits) is not.Use addabl to add a buff. Find a list of all buff IDs at https://commands.gg/witcher3/buffs");
            AddCommand(50, 5f, "cleardevelop", "cleardevelop", "This command will reset Geralt completely, clearing your inventory and resetting your level to 1. You will also be given starter gear.");
            AddCommand(50, 5f, "witchcraft", "witchcraft", "NOTE: This command could crash your game, or take a few minutes to fully execute. This command will give you one of each item in the game.");
            AddCommand(50, 5f, "addskillpoints", "addskillpoints(Amount)", "This command will give you the specified amount of skill points.");
            AddCommand(50, 5f, "buffme", "buffme('Effect Type ID', Seconds)", "This console command will give your character the specified effect for the specified duration (seconds). These effects are not the same as those from the addabl command. Find a list of all codes at https://commands.gg/witcher3/effect-types");
            AddCommand(50, 5f, "activateAllGlossaryCharacters", "activateAllGlossaryCharacters", "This command enables (shows) all characters in the glossary.");
            AddCommand(50, 5f, "activateAllGlossaryBeastiary", "activateAllGlossaryBeastiary", "This command enables (shows) all monsters in the glossary.");
            AddCommand(50, 5f, "addHair1", "addHair1", "This command sets your hairstyle to the default hairstyle.");
            AddCommand(50, 5f, "addHair2", "addHair2", "This command sets your hairstyle to a ponytail.");
            AddCommand(50, 5f, "addHair3", "addHair3", "This command sets your hairstyle to long (shoulder length), loose hair.");
            AddCommand(50, 5f, "addHairDLC1", "addHairDLC1", "This command sets your hairstyle to a loose, short haircut.");
            AddCommand(50, 5f, "addHairDLC2", "addHairDLC2", "This command sets your hairstyle to a mohawk with a pony tail.");
            AddCommand(50, 5f, "addHairDLC3", "addHairDLC3", "This command sets your hairstyle to Elven Rebel's hairstyle - short, slicked back hair.");
            AddCommand(50, 5f, "setbeard", "setbeard(#, #)", "This command will set your beard's beard style. See examples (on command page) for beard types");
            AddCommand(50, 5f, "WitcherHairstyle", "WitcherHairstyle(1 / 2 / 3)", "This command sets your character's hairstyle to the specified hairstyle number. The hairstyle number should be in apostrophes. Hairstyle numbers are '1', '2', or '3'.");
            AddCommand(50, 5f, "setcustomhead", "setcustomhead('Head ID')", "This console command will set your character's head to the head with the specified ID. Find a list of head IDs at https://commands.gg/witcher3/heads Use the removecustomhead command to revert to your character's default head.");
            AddCommand(50, 5f, "removecustomhead", "removecustomhead", "This command will remove any custom head you have applied to your character with the setcustomhead command.");
            AddCommand(50, 5f, "staminapony", "staminapony", "This console command spawns a horse with unlimited stamina.");
            AddCommand(50, 5f, "instantMount", "instantMount('NPC ID')", "This command will spawn and instantly mount your character to the NPC with the specified ID. The NPC ID should be surrounded by apostrophes: instantMount('horse'), not instantMount(horse). Find a list of all NPC spawn codes at https://commands.gg/witcher3/npcs");
            AddCommand(50, 5f, "dismember", "dismember", "This command dismembers your current targeted NPC.");
            AddCommand(50, 5f, "appearance", "appearance('Appearance ID')", "This command will change the appearance of your targeted NPC (or character if no target) to the appearance with the specified ID. The appearance ID should have apostrophes on either side: appearance('ciri_winter'), not appearance(ciri_winter).");
            AddCommand(50, 5f, "ShowKnownPins", "ShowKnownPins(0 / 1)", "This console command will reveal (1) or hide (0) all locations on the map currently that should display as a question mark (?).");
            AddCommand(50, 5f, "AllowFT", "AllowFT(0 / 1)", "This command can be used to enable (1) or disable (0) the ability to Fast Travel from any location.");
            AddCommand(50, 5f, "gotoWyzima", "gotoWyzima", "This command will teleport your character to Wyzima.");
            AddCommand(50, 5f, "gotoNovigrad", "gotoNovigrad", "This command will teleport your character to Novigrad.");
            AddCommand(50, 5f, "gotoSkellige", "gotoSkellige", "This command will teleport your character to Skellige.");
            AddCommand(50, 5f, "gotoKaerMohren", "gotoKaerMohren", "This command will teleport your character to Kaer Morhen.");
            AddCommand(50, 5f, "gotoProlog", "gotoProlog", "This command will teleport your character to Prolog.");
            AddCommand(50, 5f, "gotoPrologWinter", "gotoPrologWinter", "This command will teleport your character to Prolog Winter.");
            AddCommand(50, 5f, "xy", "xy(X, Y)", "This command will teleport your character to the specified X and Y coordinates.");
            AddCommand(50, 5f, "SpawnHorse", "SpawnHorse", "This console command spawns.. a horse! No surprise there.");
            AddCommand(50, 5f, "spawnBoatAndMount", "spawnBoatAndMount", "This console command, as the name would suggest, spawns a boat and mounts you to it.");
            AddCommand(50, 5f, "changeweather", "changeweather('Weather ID')", "This command will change the world's weather to the weather type with the specified ID. Apostrophes might need to be on each side of the weather ID, usually if there are spaces in the name: changeweather('Winter Epilog') is correct, whereas changeweather(Winter Epilog) is not.");
            AddCommand(50, 5f, "settime", "settime(Day, Hour, Minute, Seconds)", "This command sets the time of day to the specified time. Time should be given as the number of days that have passed since the game started, followed by the hour, minute and second of the day.");
            AddCommand(50, 5f, "TM", "TM(Multiplier)", "This command sets the time multiplier (what TM stands for) to the specified number. A multiplier of 0.5 would make time go by at half the usual speed, a multiplier of 2 would make time go by twice as fast. The default multiplier is 1.");
            AddCommand(50, 5f, "fadeout", "fadeout", "This console command will fade out the game using the same effect that is used for a cut scene. Use fadein to fade the screen back.");
            AddCommand(50, 5f, "fadein", "fadein", "This command fades the screen in after having previously been faded out. The fade effect is the same as that used for cut scenes.");
            AddCommand(50, 5f, "dlgshow", "dlgshow", "Contrary to its name, this command will hide the game's GUI (HUD). Use dlghide to show it again.");
            AddCommand(50, 5f, "testpause", "testpause", "This command will pause the game.");
            AddCommand(50, 5f, "testunpause", "testunpause", "This command will unpause the game.");
            AddCommand(50, 5f, "ToggleCameraAutoRotation", "ToggleCameraAutoRotation", "This console command enables and disables the automatic rotation of the camera that follows your character.");
        }

        private static void AddCommand(long cost, float ttl, string command, string syntax, string description)
        {
            commandList[command.ToLower()] = CreateCommandDefinition(command, syntax, description, cost, ttl);
        }

        private static IGameCommandDefinition CreateCommandDefinition(string command, string syntax, string description, long cost, float ttl = 0f)
        {
            return new GameCommandDefinition(
                command,
                syntax,
                description,
                cost,
                ttl,
                TimeSpan.MinValue,
                TimeSpan.MinValue
            );
        }

        public static bool TryGet(string command, out IGameCommandDefinition o)
        {
            return commandList.TryGetValue(command.ToLower(), out o);
        }
    }
}