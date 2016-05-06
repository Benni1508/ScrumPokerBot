namespace ScrumPokerBot.Contracts
{
    public class LeaveSessionMessage : TelegramMessageBase
    {
        public LeaveSessionMessage(PokerUser user, string message) 
            : base(user, message)
        {
        }

    }
}