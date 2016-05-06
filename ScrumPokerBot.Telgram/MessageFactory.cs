using System.Linq;
using System.Text.RegularExpressions;
using ScrumPokerBot.Contracts;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Telgram
{
    public class MessageFactory : IMessageFactory
    {
        public ITelegramMessage Create(Message message)
        {
            var from = message.From;
            var user = new PokerUser(message.Chat);

            var estimation = new EstimationMessage(from.Id, user,message.Text);
            if (estimation.IsValid )
            {
                return estimation;
            }
         
            if (message.Text.StartsWith("/"))
            {
                var command = message.Text.Split(' ');
                switch (command[0].ToLower())
                {
                    case "/startsession":
                        return new StartSessionMessage(from.Id, user, message.Text);
                    case "/connect":
                        var connectMessage =  new ConnectSessionMessage(from.Id, user, message.Text);
                        if (connectMessage.IsValid )
                        {
                            return connectMessage;
                        }
                        return new UnknownCommandMessage(from.Id, user, message.Text);
                    case "/poker":
                        return new StartPokerMessage(from.Id, user, message.Text);
                    default:
                        return new UnknownCommandMessage(from.Id, user, message.Text);
                }
            }
            
            return new UnknownCommandMessage(from.Id, user, message.Text);
            
        }
    }
}