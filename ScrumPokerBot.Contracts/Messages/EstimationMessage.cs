using System.Text.RegularExpressions;

namespace ScrumPokerBot.Contracts.Messages
{
    public class EstimationMessage : TelegramMessageBase
    {
        public EstimationMessage(PokerUser user, string text) : base(user, text)
        {
            var regex = new Regex(RegexPattern);
            if (regex.IsMatch(text))
            {
                var match = regex.Match(text);
                var gr = match.Groups[1].Value;
                Estimation = int.Parse(gr);
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        public int Estimation { get; }

        public bool IsValid { get; }

        private readonly string RegexPattern = "^(\\d*) Story Points$";

        public EstimationMessage(PokerUser pokerUser, string data, int messageId) : this(pokerUser, data)
        {
            this.MessageId = messageId;
        }

        public int MessageId { get; set; }
    }
}