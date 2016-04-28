using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScrumPokerBot.Contracts;
using ScrumPokerBot.Domain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Telgram
{
    public class BotService : IBotService, IMessageReceiver
    {
        private readonly Api bot;
        private readonly IMessageFactory messageFactory;
        private bool firstRun;

        public BotService(Api bot, IMessageFactory messageFactory)
        {
            firstRun = true;
            this.bot = bot;
            this.messageFactory = messageFactory;
        }

        public void Start()
        {
            Runner();
        }

        public void Stop()
        {
        }

        public event EventHandler<ConnectSessionMessage> ConnectedMessageReceived;
        public event EventHandler<EstimationMessage> EstimationMessageReceived;
        public event EventHandler<StartSessionMessage> StartSessionMessageReceived;
        public event EventHandler<StartPokerMessage> StartPokerMessageReceived;
        public event EventHandler<UnknownCommandMessage> UnknownMessageReceived;

        private async Task Runner()
        {
            var offset = 0;
            while (true)
            {
                var updates = await bot.GetUpdates(offset);

                if (firstRun)
                {
                    offset = updates.Last().Id + 1;
                    Console.WriteLine($"Skip {offset} Messages!");
                    firstRun = false;
                    continue;
                }
                foreach (var update in updates)
                {
                    Console.WriteLine($"{update.Message.From.Id}: \t ({update.Message.Type})  {update.Message.Text}");
                    offset = update.Id + 1;
                    if (update.Message != null && update.Message.Type == MessageType.TextMessage)
                    {
                        var message = messageFactory.Create(update.Message);
                        OnMessageReceived(message);
                    }
                }

                await Task.Delay(1000);
            }
        }

        protected virtual void OnMessageReceived(ITelegramMessage e)
        {
            if (e is ConnectSessionMessage)
            {
                OnConnectedMessageReceived(e as ConnectSessionMessage);
            }
            else if (e is EstimationMessage)
            {
                OnEstimationMessageReceived(e as EstimationMessage);
            }
            else if (e is StartSessionMessage)
            {
                OnStartSessionMessageReceived(e as StartSessionMessage);
            }
            else if (e is StartPokerMessage)
            {
                OnStartPokerMessageReceived(e as StartPokerMessage);
            }
            else if (e is UnknownCommandMessage)
            {
                OnUnknownMessageReceived(e as UnknownCommandMessage);
            }
            else
            {
                throw new KeyNotFoundException();                
            }
        }

        protected virtual void OnEstimationMessageReceived(EstimationMessage e)
        {
            EstimationMessageReceived?.Invoke(this, e);
        }

        protected virtual void OnStartSessionMessageReceived(StartSessionMessage e)
        {
            StartSessionMessageReceived?.Invoke(this, e);
        }

        protected virtual void OnStartPokerMessageReceived(StartPokerMessage e)
        {
            StartPokerMessageReceived?.Invoke(this, e);
        }

        protected virtual void OnUnknownMessageReceived(UnknownCommandMessage e)
        {
            UnknownMessageReceived?.Invoke(this, e);
        }

        protected virtual void OnConnectedMessageReceived(ConnectSessionMessage e)
        {
            ConnectedMessageReceived?.Invoke(this, e);
        }
    }
}