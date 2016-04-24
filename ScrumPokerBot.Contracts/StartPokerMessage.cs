namespace ScrumPokerBot.Contracts
{
    public class StartPokerMessage : TelegramMessageBase
    {
        public StartPokerMessage(long chatId, PokerUser user, string message, string description)
            : base(chatId, user, message)
        {
            Description = description;
        }

        public string Description { get; }
        public override CommandType CommandType => CommandType.StartPoker;
    }
}