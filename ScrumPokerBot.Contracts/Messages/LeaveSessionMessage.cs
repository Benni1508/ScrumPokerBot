namespace ScrumPokerBot.Contracts.Messages
{
    public class LeaveSessionMessage : TelegramMessageBase
    {
        public LeaveSessionMessage(PokerUser user, string message) 
            : base(user, message)
        {
        }

    }
}