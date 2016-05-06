namespace ScrumPokerBot.Contracts
{
    public class UnknownCommandMessage : TelegramMessageBase
    {
        public UnknownCommandMessage(PokerUser user, string message) : base( user, message)
        {
        }

    }
}