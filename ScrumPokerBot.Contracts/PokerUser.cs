using Telegram.Bot.Types;

namespace ScrumPokerBot.Contracts
{
    public class PokerUser
    {
        public PokerUser()
        {
        }

        public PokerUser(Chat chat)
        {
            this.Username = chat.Username;
            this.Firstname = chat.FirstName;
            this.Lastname = chat.LastName;
            this.ChatId = chat.Id;
        }

        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public long ChatId { get; set; }

        public override string ToString()
        {
            return $"{Lastname}, {Firstname} ({Username})";
        }
    }
}