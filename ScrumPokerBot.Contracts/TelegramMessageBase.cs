namespace ScrumPokerBot.Contracts
{
    public abstract class TelegramMessageBase : ITelegramMessage
    {
        public TelegramMessageBase(long chatId, PokerUser user, string message)
        {
            ChatId = chatId;
            User = user;
            Message = message;
        }

        public abstract CommandType CommandType { get; }
        public long ChatId { get; }
        public PokerUser User { get; }
        public string Message { get; }
    }
}