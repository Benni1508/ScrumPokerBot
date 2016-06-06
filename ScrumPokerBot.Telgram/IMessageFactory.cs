using Telegram.Bot.Types;

namespace ScrumPokerBot.Telgram
{
    public interface IMessageFactory
    {
        void PublishMessage(Message message);
        void PublishMessage(CallbackQuery callbackQuery);
    }
}