using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

namespace ScrumPokerBot.Contracts
{
    public class EstimationMessage : TelegramMessageBase
    {
        public EstimationMessage(int id, PokerUser user, string text) : base(id, user, text)
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
        public override CommandType CommandType => CommandType.Estimation;

        public bool IsValid { get; }

        private readonly string RegexPattern = "^(\\d*) Story Points$";
    }
}