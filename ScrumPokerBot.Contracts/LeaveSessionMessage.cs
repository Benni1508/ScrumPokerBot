namespace ScrumPokerBot.Contracts
{
    public class LeaveSessionMessage : TelegramMessageBase
    {
        public LeaveSessionMessage(long chatId, PokerUser user, string message) 
            : base(chatId, user, message)
        {
        }

    }
}