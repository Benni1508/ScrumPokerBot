using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ScrumPokerBot.Contracts;
using ScrumPokerBot.Contracts.Messages;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Telgram
{
    public class BotService : IBotService
    {
        private readonly Api bot;
        private readonly IMessageFactory messageFactory;
        private readonly IMessageBus bus;
        private bool firstRun;

        public BotService(Api bot, IMessageFactory messageFactory, IMessageBus bus)
        {
            firstRun = true;
            this.bot = bot;
            this.messageFactory = messageFactory;
            this.bus = bus;
        }

        public void Start()
        {
            Runner();
        }

        public void Stop()
        {
        }


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
                        bus.Publish(message);
                    }
                }

                await Task.Delay(1000);
            }
        }

    }
}