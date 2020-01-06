using System;
using System.Linq;
using System.Threading.Tasks;
using TWitcher3.Core;
using TWitcher3.Core.Games;
using TWitcher3.Core.Games.Witcher3;
using TWitcher3.Core.Handlers;
using TWitcher3.Core.Net;
using TWitcher3.Core.Twitch;

namespace TWitcher3.Commands.Games.Witcher3
{
    public class Witcher3CommandProcessor : ICommandProcessor
    {
        private readonly IMessageBus messageBus;
        private readonly IWitcher3 witcher3;
        private readonly IGameCommandFactory commandFactory;
        private readonly ITwitchUserStore userStore;

        public Witcher3CommandProcessor(
            IMessageBus messageBus,
            IWitcher3 witcher3,
            IGameCommandFactory commandFactory,
            ITwitchUserStore userStore)
        {
            this.messageBus = messageBus;
            this.witcher3 = witcher3;
            this.commandFactory = commandFactory;
            this.userStore = userStore;
        }

        public void Dispose()
        {
        }

        public Task ProcessAsync(IMessageBroadcaster broadcaster, ICommand cmd)
        {
            var command = cmd.Arguments;
            if (string.IsNullOrEmpty(command))
            {
                return Task.CompletedTask;
            }

            if (command.Trim().StartsWith("help "))
            {
                var cmdTarget = command.Trim().Split(new string[] { "help " }, StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault();

                if (Witcher3Commands.TryGet(cmdTarget, out var cmdTargetInfo))
                {
                    broadcaster.Broadcast(cmdTargetInfo.Syntax + $", costs: {cmdTargetInfo.InvokeCost} credits, " + cmdTargetInfo.Description);
                }
                else
                {
                    broadcaster.Broadcast(cmd.Sender.Username + ", no such command exists!");
                }

                return Task.CompletedTask;
            }

            if (command.Trim().Equals("commands", StringComparison.InvariantCultureIgnoreCase) ||
                command.Trim().Equals("help", StringComparison.InvariantCultureIgnoreCase))
            {
                broadcaster.Broadcast("For a list of commands, go to https://commands.gg/witcher3");
                return Task.CompletedTask;
            }

            if (!witcher3.IsRunning)
            {
                broadcaster.Broadcast($"{cmd.Sender.Username}, Witcher 3 is not running right now.");
                return Task.CompletedTask;
            }

            if (witcher3.IsRunning)
            {
                var gameCommand = commandFactory.Create(cmd.Sender.Username, command);
                if (gameCommand == null)
                {
                    broadcaster.Broadcast($"{cmd.Sender.Username}, no such Witcher 3 command exists!");
                    return Task.CompletedTask;
                }

                var user = this.userStore.Get(cmd.Sender.Username);
                if (!user.CanAfford(gameCommand.InvokeCost))
                {
                    broadcaster.Broadcast($"{cmd.Sender.Username}, You have insufficient amount of credits to use this command. This costs {gameCommand.InvokeCost}, and you only have {(int)user.Credits} left.");
                    return Task.CompletedTask;
                }

                if (witcher3.Invoke(gameCommand))
                {
                    messageBus.Send("witcher3", gameCommand);
                    user.RemoveCredits(gameCommand.InvokeCost);
                    broadcaster.Broadcast($"{cmd.Sender.Username}, command executed! You now have {(int)user.Credits} credits left.");
                    witcher3.Notify($"{cmd.Sender.Username} used {gameCommand.Name}");
                }
                else
                {
                    broadcaster.Broadcast($"{cmd.Sender.Username}, the command failed :(");
                }
            }
            return Task.CompletedTask;
        }
    }
}
