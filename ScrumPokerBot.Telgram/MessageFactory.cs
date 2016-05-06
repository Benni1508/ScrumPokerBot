using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using ScrumPokerBot.Contracts;
using ScrumPokerBot.Contracts.Messages;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Telgram
{
    public class MessageFactory : IMessageFactory
    {
        private readonly IMessageBus bus;

        public MessageFactory(IMessageBus bus)
        {
            this.bus = bus;
        }

        public void PublishMessage(Message message)
        {
            var from = message.From;
            var user = new PokerUser(message.Chat);

            var estimation = new EstimationMessage(user,message.Text);
            if (estimation.IsValid )
            {
                bus.Publish(estimation);
            }
         
            if (message.Text.StartsWith("/"))
            {
                var command = message.Text.Split(' ');
                switch (command[0].ToLower())
                {
                    case "/startsession":
                        bus.Publish(new StartSessionMessage(user, message.Text));
                        break;
                    case "/connect":
                        var connectMessage =  new ConnectSessionMessage(user, message.Text);
                        if (connectMessage.IsValid )
                        {
                            bus.Publish(connectMessage);
                            break;
                        }
                        bus.Publish(new UnknownCommandMessage(user, message.Text));
                        break;
                    case "/poker":
                        bus.Publish(new StartPokerMessage(user, message.Text));
                        break;
                    case "/leavesession":
                        bus.Publish(new LeaveSessionMessage(user, message.Text));
                        break;
                    default:
                        bus.Publish(new UnknownCommandMessage(user, message.Text));
                        break;
                }
            }
            
         bus.Publish(new UnknownCommandMessage(user, message.Text));
            
        }
    }
}