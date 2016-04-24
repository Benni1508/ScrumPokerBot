namespace ScrumPokerBot.Contracts
{
    public class UnknownCommandMessage : TelegramMessageBase
    {
        public UnknownCommandMessage(long chatId, PokerUser user, string message) : base(chatId, user, message)
        {
        }

        public override CommandType CommandType
        {
            get { return CommandType.NoOrUnknownCommand; }
        }
    }
}