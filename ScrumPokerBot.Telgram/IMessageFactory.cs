using ScrumPokerBot.Contracts;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Telgram
{
    public interface IMessageFactory
    {
        ITelegramMessage Create(Message message);
    }
}