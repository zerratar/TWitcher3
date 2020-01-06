using TWitcher3.Commands.Games.Witcher3;
using TWitcher3.Commands.Misc;
using TWitcher3.Core;
using TWitcher3.Core.Games;
using TWitcher3.Core.Games.Witcher3;
using TWitcher3.Core.Handlers;
using TWitcher3.Core.Net;
using TWitcher3.Core.Twitch;
using TWitcher3.Providers;

namespace TWitcher3
{
    class Program
    {
        static void Main(string[] args)
        {
            var ioc = new IoC();
            ioc.RegisterShared<ILogger, ConsoleLogger>();
            ioc.RegisterShared<ITwitchUserStore, TwitchUserStore>();

            ioc.RegisterCustomShared<IAppSettings>(() => new AppSettingsProvider().Get());
            ioc.RegisterShared<IWitcher3, Witcher3GameIntegration>();
            ioc.RegisterShared<IGameCommandFactory, Witcher3CommandFactory>();
            ioc.RegisterShared<IGameIntegrator, Witcher3GameIntegrator>();

            ioc.Register<IGameConnection, TcpGameConnection>();
            ioc.Register<IGameClient, TcpGameClient>();

            ioc.RegisterShared<IMessageBus, MessageBus>();

            var commandHandler = new TextCommandHandler(ioc);
            commandHandler.Register<Witcher3CommandProcessor>("witcher3", "witcher", "w3");
            commandHandler.Register<CreditsCommandProcessor>("credit", "credits", "points");

            var channelProvider = new DefaultChannelProvider();
            var credentialsProvider = new DefaultConnectionCredentialsProvider(ioc.Resolve<IAppSettings>());

            var gameIntegrator = ioc.Resolve<IGameIntegrator>();
            gameIntegrator.Start();
        }
    }
}
