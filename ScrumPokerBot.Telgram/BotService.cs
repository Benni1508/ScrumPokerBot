using System;
using System.Collections.Generic;
using System.IO;
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

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        private async Task Runner()
        {
            var offset = 0;
            while (true)
            {
                var updates = await bot.GetUpdates(offset);
                Console.WriteLine($"{updates.Count()} Nachrichten gefunden");
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
            OnMessageReceived(new MessageReceivedEventArgs(e));
        }

        protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}