using ScrumPokerBot.Contracts.Messages;

namespace ScrumPokerBot.Contracts
{
    public abstract class TelegramMessageBase : IMessage
    {
        public TelegramMessageBase(PokerUser user, string message)
        {
            User = user;
            Message = message;
        }

        public PokerUser User { get; }
        public string Message { get; }
    }
}