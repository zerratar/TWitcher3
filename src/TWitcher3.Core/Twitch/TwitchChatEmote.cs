using TWitcher3.Core.Chat;

namespace TWitcher3.Core.Twitch
{
    public class TwitchChatEmote : IChatEmote
    {
        public TwitchChatEmote(string name, string imageUrl)
        {
            this.Name = name;
            this.ImageUrl = imageUrl;
        }

        public string Name { get; }
        public string ImageUrl { get; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}