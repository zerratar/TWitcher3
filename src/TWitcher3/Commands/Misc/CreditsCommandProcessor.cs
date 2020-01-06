using System.Threading.Tasks;
using TWitcher3.Core;
using TWitcher3.Core.Handlers;
using TWitcher3.Core.Net;
using TWitcher3.Core.Twitch;

namespace TWitcher3.Commands.Misc
{
    public class CreditsCommandProcessor : ICommandProcessor
    {
        private readonly ITwitchUserStore userStore;

        public CreditsCommandProcessor(ITwitchUserStore userStore)
        {
            this.userStore = userStore;
        }

        public void Dispose()
        {
        }

        public Task ProcessAsync(IMessageBroadcaster broadcaster, ICommand cmd)
        {
            //if (string.IsNullOrEmpty(cmd.Arguments))
            //{
            //    return Task.CompletedTask;
            //}

            var command = cmd.Arguments.Trim().ToLower();
            if (command.StartsWith("add"))
            {
                if (!cmd.Sender.IsModerator && !cmd.Sender.IsBroadcaster)
                {
                    broadcaster.Send(cmd.Sender.Username,
                    //broadcaster.Broadcast(
                        "Sorry, this command is restricted to mods and broadcaster.");
                    return Task.CompletedTask;
                }

                var data = command.Split(' ');
                if (data.Length <= 1)
                {
                    broadcaster.Broadcast("Insufficient parameters, use: !credits add <user> <amount>");
                    return Task.CompletedTask;
                }

                var name = data[1];
                var amountStr = data[2];
                if (int.TryParse(amountStr, out var val))
                {
                    var user = this.userStore.Get(name);
                    user.AddCredits(val);
                    broadcaster.Broadcast($"{val} credits was added to {name}'s stash.");
                }
            }
            else if (command.StartsWith("remove"))
            {
                if (!cmd.Sender.IsModerator && !cmd.Sender.IsBroadcaster)
                {
                    //broadcaster.Broadcast(
                    broadcaster.Send(cmd.Sender.Username,
                        "Sorry, this command is restricted to mods and broadcaster.");
                    return Task.CompletedTask;
                }

                var data = command.Split(' ');
                if (data.Length <= 1)
                {
                    broadcaster.Broadcast("Insufficient parameters, use: !credits remove <user> <amount>");
                    return Task.CompletedTask;
                }
                var name = data[1];
                var amountStr = data[2];
                if (int.TryParse(amountStr, out var val))
                {
                    var user = this.userStore.Get(name);
                    user.RemoveCredits(val);
                    broadcaster.Broadcast($"{val} credits was removed from {name}'s stash.");
                }
            }
            else
            {
                var user = this.userStore.Get(cmd.Sender.Username);
                broadcaster.Send(cmd.Sender.Username,
                //broadcaster.Broadcast(
                    $"{cmd.Sender.Username}, you have {user.Credits} credits left.");
            }
            return Task.CompletedTask;
        }
    }
}