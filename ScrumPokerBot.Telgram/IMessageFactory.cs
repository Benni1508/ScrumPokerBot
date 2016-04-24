using System.Linq;
using System.Text.RegularExpressions;
using ScrumPokerBot.Contracts;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Telgram
{
    public interface IMessageFactory
    {
        ITelegramMessage Create(Message message);
    }

    class MessageFactory : IMessageFactory
    {
        public ITelegramMessage Create(Message message)
        {
            int value;
            var from = message.From;
            var user = new PokerUser(message.Chat);

            var regex = new Regex("^(\\d*) Story Points$");
            var matches = regex.Match(message.Text);
            if (matches.Success)
            {
                return new EstimationMessage(from.Id, user, message.Text, int.Parse(matches.Groups[1].Value));
            }

            if (message.Text.StartsWith("/"))
            {
                var command = message.Text.Split(' ');
                switch (command[0].ToLower())
                {
                    case "/startsession":
                        return new StartSessionMessage(from.Id,user, message.Text);
                    case "/connect":
                        int sessionid;
                        if (int.TryParse(command[1], out sessionid))
                        {
                            return new ConnectSessionMessage(from.Id, user, message.Text ,sessionid);
                        }
                        return new UnknownCommandMessage(from.Id, user, message.Text);
                    case "/poker":
                        string desc = "";
                        if (command.Count() > 1)
                        {
                            desc = string.Join(" ",command.Skip(1).ToArray());
                        }
                        return new StartPokerMessage(from.Id, user, message.Text, desc);
                    default:
                        return new UnknownCommandMessage(from.Id,user, message.Text);
                }
            }
            if (!message.Text.StartsWith("/") && !int.TryParse(message.Text, out value))
            {

                return new UnknownCommandMessage(from.Id, user, message.Text);
            }
            else
            {
                return null;
            }
        }
    }
}