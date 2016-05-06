namespace ScrumPokerBot.Contracts
{
    public class StartSessionMessage : TelegramMessageBase
    {
        public StartSessionMessage(PokerUser user, string message) : base( user, message)
        {
        }

    }
}