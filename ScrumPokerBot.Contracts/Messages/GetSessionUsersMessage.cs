namespace ScrumPokerBot.Contracts.Messages
{
    public class GetSessionUsersMessage : TelegramMessageBase
    {
        public GetSessionUsersMessage(PokerUser user, string message) : base(user, message)
        {
        }
    }
}