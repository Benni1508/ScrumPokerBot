namespace ScrumPokerBot.Contracts.Messages
{
    public class CancelPokerMessage : TelegramMessageBase
    {
        public CancelPokerMessage(PokerUser user, string message)
            : base( user, message)
        {
        }
    }
}