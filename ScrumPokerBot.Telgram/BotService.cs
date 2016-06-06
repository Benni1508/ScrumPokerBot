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
                        switch (update.Type)
                        {
                            case UpdateType.UnkownUpdate:
                                break;
                            case UpdateType.MessageUpdate:
                                Console.WriteLine($"{update.Message.From.Id}: \t ({update.Message.Type})  {update.Message.Text}");
                                HandleUpdate(update.Message);
                                break;
                            case UpdateType.InlineQueryUpdate:
                                break;
                            case UpdateType.ChosenInlineResultUpdate:
                                break;
                            case UpdateType.CallbackQueryUpdate:
                                Console.WriteLine($"{update.CallbackQuery.From.Id}: \t (CallbackQuery)  {update.CallbackQuery.Data}");
                                HandleUpdate(update.CallbackQuery);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                      
                        offset = update.Id + 1;

               
                    }
                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private void HandleUpdate(CallbackQuery callbackQuery)
        {
            messageFactory.PublishMessage(callbackQuery);
        }

        private void HandleUpdate(Message message)
        {
            switch (message.Type)
            {
                case MessageType.TextMessage:
                    messageFactory.PublishMessage(message);
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
                    Console.WriteLine($"Message of Type {message.Type} received!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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