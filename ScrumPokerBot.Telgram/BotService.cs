using System;
using System.Linq;
using System.Threading.Tasks;
using ScrumPokerBot.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Telgram
{
    public class BotService : IBotService
    {
        private readonly Api bot;
        private readonly IMessageBus bus;
        private readonly IMessageFactory messageFactory;
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
                try
                {
                    var updates = await bot.GetUpdates(offset);
                    Console.WriteLine($"{updates.Count()} Nachrichten gefunden");
                    if (firstRun)
                    {
                        if (!updates.Any())
                        {
                            firstRun = false;
                            continue;
                        }
                        offset = updates.Last().Id + 1;
                        Console.WriteLine($"Skip {updates.Count()} Messages!");
                        firstRun = false;
                        continue;
                    }
                    foreach (var update in updates)
                    {
                        Console.WriteLine($"{update.Message.From.Id}: \t ({update.Message.Type})  {update.Message.Text}");
                        offset = update.Id + 1;
                        if (update.Message != null && update.Message.Type == MessageType.TextMessage)
                        {
                            messageFactory.PublishMessage(update.Message);
                        }
                    }

                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}