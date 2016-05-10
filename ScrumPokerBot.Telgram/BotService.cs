using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Telgram
{
    public class BotService : IBotService
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

        public void Start() => Runner();

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
                    if (firstRun)
                    {
                        offset = IgnoreMessages(updates);
                        firstRun = false;
                        continue;
                    }
                    foreach (var update in updates)
                    {
                        Console.WriteLine($"{update.Message.From.Id}: \t ({update.Message.Type})  {update.Message.Text}");
                        offset = update.Id + 1;

                        HandleUpdate(update);
                    }
                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private void HandleUpdate(Update update)
        {
            if (update.Message == null) return;
            switch (update.Message.Type)
            {
                case MessageType.TextMessage:
                    messageFactory.PublishMessage(update.Message);
                    break;
                case MessageType.PhotoMessage:
                case MessageType.UnknownMessage:
                case MessageType.AudioMessage:
                case MessageType.VideoMessage:
                case MessageType.VoiceMessage:
                case MessageType.DocumentMessage:
                case MessageType.StickerMessage:
                case MessageType.LocationMessage:
                case MessageType.ContactMessage:
                case MessageType.ServiceMessage:
                    Console.WriteLine($"Message of Type {update.Message.Type} received!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (update.Message != null && update.Message.Type == MessageType.TextMessage)
            {
            }
        }

        private int IgnoreMessages(Update[] updates)
        {
            var offset = 0;
            if (!updates.Any())
            {
                return offset;
            }
            offset = updates.Last().Id + 1;
            Console.WriteLine($"Skip {updates.Length} Messages!");
            return offset;
        }
    }
}