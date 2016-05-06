using ScrumPokerBot.Contracts;
using ScrumPokerBot.Contracts.Messages;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Telgram
{
    public interface IMessageFactory
    {
        ITelegramMessage Create(Message message);
    }
}