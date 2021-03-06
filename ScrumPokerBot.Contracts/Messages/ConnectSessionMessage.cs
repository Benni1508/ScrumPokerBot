using System.Text.RegularExpressions;

namespace ScrumPokerBot.Contracts.Messages
{
    public class ConnectSessionMessage : TelegramMessageBase
    {
        public ConnectSessionMessage(PokerUser user, string message, int messageId =0)
            : base(user, message)
        {

            var regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(message)) return;

            var matches = regex.Match(message);
            var x = matches.Groups[1];
            int value;
            this.IsValid = int.TryParse(x.Value, out value);
            this.Sessionid = value;
            this.MessageId = messageId;
        }

        public int MessageId { get; private set; }

        public int Sessionid { get; }
        public bool IsValid { get; }

        private readonly string regexPattern = @"^\/connect (\d{1,3}).*$";
    }
}