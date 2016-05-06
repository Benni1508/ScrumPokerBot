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

        public int Estimation { get; set; }

        public bool IsValid { get; }

        private readonly string RegexPattern = "^(\\d*) Story Points$";
    }
}