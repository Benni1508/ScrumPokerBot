using System.Text;
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
        public int Id => (int) ChatId;

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Lastname))
            {
                sb.Append(Lastname);
                if (!string.IsNullOrEmpty(Firstname))
                {
                    sb.Append($", {Firstname}");
                }
                if (!string.IsNullOrEmpty(Username))
                {
                    sb.Append($" (@{Username})");
                }
                return sb.ToString();
            }

            if (!string.IsNullOrEmpty(Firstname))
            {
                sb.Append(Firstname);

                if (!string.IsNullOrEmpty(Username))
                {
                    sb.Append($" (@{Username})");
                }
                return sb.ToString();
            }

            if (!string.IsNullOrEmpty(Username))
            {
                sb.Append($"@{Username}");
            }
            return sb.ToString();
        }
    }
}