using System;
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
            this.firstRun = true;
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

        private async Task Runner()
        {
            
            var offset = 0;
            while (true)
            {
                var updates = await bot.GetUpdates(offset);
       
                if (firstRun)
                {
                    offset = updates.Last().Id +1;
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
                        this.OnMessageReceived(message);
                    }
                }

                await Task.Delay(1000);
            }
        }

        public event EventHandler<ITelegramMessage> MessageReceived;

        protected virtual void OnMessageReceived(ITelegramMessage e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}