using Telegram.Bot.Types;

namespace ScrumPokerBot.Contracts
{
    public class PokerUser : IHaveId
    {
        public PokerUser()
        {
        }

        public PokerUser(Chat chat)
        {
            Username = chat.Username;
            Firstname = chat.FirstName;
            Lastname = chat.LastName;
            ChatId = chat.Id;
        }

        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public long ChatId { get; set; }

        public override string ToString()
        {
            return $"{Lastname}, {Firstname} ({Username})";
        }

        public int Id => (int) ChatId;
    }
}