using System.Text.RegularExpressions;

namespace ScrumPokerBot.Contracts
{
    public class StartPokerMessage : TelegramMessageBase
    {
        public StartPokerMessage(PokerUser user, string message)
            : base( user, message)
        {
            var regex = new Regex(regexPattern);
            if (regex.IsMatch(message))
            {
                var match = regex.Match(message);
                var group = match.Groups[1];
                Description = group.Value;
            }
        }

        public string Description { get; } = "";

        private string regexPattern = @"^\/poker (.*)$";
    }
}