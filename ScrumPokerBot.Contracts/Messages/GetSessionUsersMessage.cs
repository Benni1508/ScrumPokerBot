namespace ScrumPokerBot.Contracts.Messages
{
    public class GetSessionUsersMessage : TelegramMessageBase
    {
        public GetSessionUsersMessage(PokerUser user, string message) : base(user, message)
        {
        }
    }

    public class ShowSessionsMessage : TelegramMessageBase {
        public ShowSessionsMessage(PokerUser user, string message) 
            : base(user, message)
        {
        }
    }
}