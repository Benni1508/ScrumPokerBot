namespace ScrumPokerBot.Contracts
{
    public class StartSessionMessage : TelegramMessageBase
    {
        public StartSessionMessage(long chatId, PokerUser user, string message) : base(chatId, user, message)
        {
        }

        public override CommandType CommandType
        {
            get { return CommandType.StartSession; }
        }

    }
}