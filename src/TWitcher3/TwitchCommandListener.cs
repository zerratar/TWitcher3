using System;
using System.Linq;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Enums;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;
using TWitcher3.Core;
using TWitcher3.Core.Chat;
using TWitcher3.Core.Extensions;
using TWitcher3.Core.Handlers;
using TWitcher3.Core.Net.WebSocket;
using TWitcher3.Core.Twitch;
using TWitcher3.Providers;

namespace TWitcher3
{
    public class TwitchCommandListener : ICommandListener, IMessageBroadcaster
    {
        private const int DiscordNotificationInterval = 1200_000;

        private readonly ILogger logger;
        private readonly IKernel kernel;
        private readonly IConnectionProvider connectionProvider;
        private readonly IMessageBus messageBus;
        private readonly ICommandHandler commandHandler;
        private readonly IChannelProvider channelProvider;
        private readonly IChatMessageWordResolver wordResolver;
        private readonly IConnectionCredentialsProvider credentialsProvider;
        private readonly ITwitchChatMessageEmoteResolverProvider emoteResolverProvider;
        private readonly ITwitchChatMessageParser chatMessageParser;
        private IMessageBusSubscription broadcastSubscription;
        private IMessageBusSubscription messageSubscription;
        private ITimeoutHandle discordBroadcast;
        private TwitchClient client;
        private bool isInitialized;
        private bool disposed;

        private bool allowDiscordNotification = true;

        public TwitchCommandListener(
            ILogger logger,
            IKernel kernel,
            IConnectionProvider connectionProvider,
            IMessageBus messageBus,
            ICommandHandler commandHandler,
            IChannelProvider channelProvider,
            IChatMessageWordResolver wordResolver,
            IConnectionCredentialsProvider credentialsProvider,
            ITwitchChatMessageEmoteResolverProvider emoteResolverProvider,
            ITwitchChatMessageParser chatMessageParser)
        {
            this.logger = logger;
            this.kernel = kernel;
            this.connectionProvider = connectionProvider;
            this.messageBus = messageBus;
            this.commandHandler = commandHandler;
            this.channelProvider = channelProvider;
            this.wordResolver = wordResolver;
            this.credentialsProvider = credentialsProvider;
            this.emoteResolverProvider = emoteResolverProvider;
            this.chatMessageParser = chatMessageParser;
            this.CreateTwitchClient();
            this.Start();
        }

        private void CreateTwitchClient()
        {
            client = new TwitchClient(new WebSocketClient(new ClientOptions
            {
                ClientType = ClientType.Chat,
                MessagesAllowedInPeriod = 100
            }));
        }

        public void Start()
        {
            if (!kernel.Started) kernel.Start();
            EnsureInitialized();
            client.OnMessageReceived += OnMessageReceived;
            client.OnChatCommandReceived += OnCommandReceived;
            client.OnConnected += OnConnected;
            client.OnDisconnected += OnDisconnected;
            client.OnUserJoined += OnUserJoined;
            client.OnUserLeft += OnUserLeft;
            client.OnGiftedSubscription += OnGiftedSub;
            client.OnCommunitySubscription += OnPrimeSub;
            client.OnNewSubscriber += OnNewSub;
            client.OnReSubscriber += OnReSub;
            //client.On
            client.Connect();
        }

        private void OnReSub(object sender, OnReSubscriberArgs e)
        {
            this.messageBus.Send(nameof(TwitchSubscription),
                new TwitchSubscription(
                    e.ReSubscriber.UserId,
                    e.ReSubscriber.Login,
                    e.ReSubscriber.DisplayName, e.ReSubscriber.Months, false));

            this.Broadcast($"Thank you {e.ReSubscriber.DisplayName} for the resub!!! <3");
        }

        private void OnNewSub(object sender, OnNewSubscriberArgs e)
        {
            this.messageBus.Send(nameof(TwitchSubscription),
                new TwitchSubscription(e.Subscriber.UserId,
                    e.Subscriber.Login,
                    e.Subscriber.DisplayName, 1, true));

            this.Broadcast($"Thank you {e.Subscriber.DisplayName} for the sub!!! <3");

        }

        private void OnPrimeSub(object sender, OnCommunitySubscriptionArgs e)
        {
            this.messageBus.Send(nameof(TwitchSubscription),
                new TwitchSubscription(
                    e.GiftedSubscription.UserId,
                   e.GiftedSubscription.Login,
                    e.GiftedSubscription.DisplayName, 1, false));

            this.Broadcast($"Thank you {e.GiftedSubscription.DisplayName} for the sub!!! <3");
        }

        private void OnGiftedSub(object sender, OnGiftedSubscriptionArgs e)
        {
            this.messageBus.Send(nameof(TwitchSubscription),
                new TwitchSubscription(e.GiftedSubscription.Id,
                   e.GiftedSubscription.Login,
                    e.GiftedSubscription.DisplayName, 1, false));

            this.Broadcast($"Thank you {e.GiftedSubscription.DisplayName} for the gifted sub!!! <3");
        }

        private void OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            client.OnMessageReceived -= OnMessageReceived;
            client.OnChatCommandReceived -= OnCommandReceived;
            client.OnConnected -= OnConnected;
            client.OnDisconnected -= OnDisconnected;
            client.OnUserJoined -= OnUserJoined;
            client.OnUserLeft -= OnUserLeft;
            client.OnGiftedSubscription -= OnGiftedSub;
            client.OnCommunitySubscription -= OnPrimeSub;
            client.OnNewSubscriber -= OnNewSub;
            client.OnReSubscriber -= OnReSub;
            isInitialized = false;

            logger.WriteDebug("Disconnected from the Twitch IRC Server");

            CreateTwitchClient();
            Start();
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            messageBus.Send("twitch", "");
            BroadcastDiscordServer();

            logger.WriteDebug("Connected to Twitch IRC Server");
        }

        private void BroadcastDiscordServer()
        {
            if (discordBroadcast != null)
            {
                this.kernel.ClearTimeout(discordBroadcast);
                if (this.allowDiscordNotification)
                {
                    this.Broadcast(
                        "Don't forget to join our awesome Discord. https://discord.gg/W2tgTSh Stay connected and always be in the loop!");
                    this.allowDiscordNotification = false;
                }
            }

            this.discordBroadcast = kernel.SetTimeout(BroadcastDiscordServer, DiscordNotificationInterval);
        }

        public void Stop()
        {
            if (kernel.Started) kernel.Stop();
            client.OnMessageReceived -= OnMessageReceived;
            client.OnChatCommandReceived -= OnCommandReceived;
            client.OnUserJoined -= OnUserJoined;
            client.OnUserLeft -= OnUserLeft;
            client.OnGiftedSubscription -= OnGiftedSub;
            client.OnCommunitySubscription -= OnPrimeSub;
            client.OnNewSubscriber -= OnNewSub;
            client.OnReSubscriber -= OnReSub;

            if (client.IsConnected)
                client.Disconnect();


            messageSubscription?.Unsubscribe();
            broadcastSubscription?.Unsubscribe();
        }

        public void Broadcast(string message)
        {
            if (!this.client.IsConnected) return;
            var channel = this.channelProvider.Get();

            if (client.JoinedChannels.Count == 0)
            {
                client.JoinChannel(channel);
            }

            client.SendMessage(channel, message);
        }

        public void Send(string target, string message)
        {
            if (!this.client.IsConnected) return;
            client.SendMessage(this.channelProvider.Get(), $"{target}, " + message);
            return;

            //client.SendWhisper(target, message);
        }

        public void Dispose()
        {
            if (disposed) return;
            Stop();
            disposed = true;
        }

        private void OnUserLeft(object sender, OnUserLeftArgs e)
        {
            if (!e.Channel.Contains(this.channelProvider.Get(), StringComparison.InvariantCultureIgnoreCase))
                return;

            this.messageBus.Send(nameof(TwitchUserLeft), new TwitchUserLeft(e.Username));
        }

        private void OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            if (!e.Channel.Contains(this.channelProvider.Get(), StringComparison.InvariantCultureIgnoreCase))
                return;

            this.messageBus.Send(nameof(TwitchUserJoined), new TwitchUserJoined(e.Username));
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            this.allowDiscordNotification = true;

            var chat = e.ChatMessage;
            var msg = chat;
            var emoteSet = msg.EmoteSet;

            var emoteResolver = emoteResolverProvider.Get(emoteSet);
            if (emoteSet != null)
            {
                var words = msg.Message.Split(' ');
                var emoteUrls = words
                    .Select(x => emoteResolver.Resolve(x, true))
                    .Where(x => x != null)
                    .Select(x => x.ImageUrl);

                var emoteCollection = new ChatEmoteCollection(msg.Username, emoteUrls);
                var twitchChatEmoteCollection = new TwitchChatEmoteCollection(msg, emoteUrls);
                messageBus.Send(nameof(TwitchChatEmoteCollection), twitchChatEmoteCollection);
                messageBus.Send(nameof(ChatEmoteCollection), emoteCollection);
                connectionProvider.BroadcastAsync(emoteCollection);
            }

            //if (e.ChatMessage.Bits > 0)
            //{
            //    this.messageBus.Send(nameof(BitsCheered), new BitsCheered(e));
            //}

            if ((chat.Username?.Equals("ravenfallofficial", StringComparison.OrdinalIgnoreCase)).GetValueOrDefault() ||
                chat.Message.StartsWith("!", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var twitchSender = new TwitchChatSender(chat.DisplayName ?? chat.Username, chat.ColorHex);
            var message = chatMessageParser.Parse(twitchSender, chat.Message, wordResolver, emoteResolver);
            connectionProvider.BroadcastAsync(message);
        }

        private async void OnCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            await commandHandler.HandleAsync(this, new TwitchCommand(e.Command));
        }

        private void EnsureInitialized()
        {
            if (isInitialized) return;

            if (this.broadcastSubscription == null)
                this.broadcastSubscription = messageBus.Subscribe<string>(MessageBus.Broadcast, Broadcast);
            if (this.messageSubscription == null)
                this.messageSubscription = messageBus.Subscribe<string>(MessageBus.Message, MessageUser);

            client.Initialize(credentialsProvider.Get(), channelProvider.Get());
            isInitialized = true;
        }

        private void MessageUser(string message)
        {
            //var channel = this.channelProvider.Get();
            if (!this.client.IsConnected) return;
            if (string.IsNullOrEmpty(message)) return;

            client.SendMessage(this.channelProvider.Get(), message);
            return;

            if (message.IndexOf(',') == -1) return;

            var user = message.Remove(message.IndexOf(','));
            var msg = message.Substring(message.IndexOf(',') + 1);
            client.SendWhisper(user.Trim(), msg.Trim());
        }
    }
}