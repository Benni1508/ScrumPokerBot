using System;
using System.Collections.Generic;
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
            var user = new PokerUser(message.Chat);

            var estimation = new EstimationMessage(user,message.Text);
            if (estimation.IsValid)
            {
                bus.Publish(estimation);
                return;
            }
         
            if (message.Text.StartsWith("/"))
            {
                var command = message.Text.Split(' ');
                switch (command[0].ToLower())
                {
                    case "/startsession":
                        bus.Publish(new StartSessionMessage(user, message.Text));
                        return;
                    case "/connect":
                        var connectMessage = new ConnectSessionMessage(user, message.Text);

                        bus.Publish(connectMessage);
                        return;
                    case "/poker":
                        bus.Publish(new StartPokerMessage(user, message.Text));
                        return;
                    case "/endpoker":
                        bus.Publish(new CancelPokerMessage(user, message.Text));
                        return;
                    case "/leavesession":
                        bus.Publish(new LeaveSessionMessage(user, message.Text));
                        return;
                    case "/showusers":
                        bus.Publish(new GetSessionUsersMessage(user, message.Text));
                        return;
                    default:
                        bus.Publish(new UnknownCommandMessage(user, message.Text));
                        return;
                }
            }
            
         bus.Publish(new UnknownCommandMessage(user, message.Text));
            
        }

        public void PublishMessage(CallbackQuery callbackQuery)
        {
            PokerUser pokerUser = new PokerUser(callbackQuery.From);
           
            var estimation = new EstimationMessage(pokerUser, callbackQuery.Data);
            if (estimation.IsValid)
            {
                bus.Publish(estimation);
            }
        }
    }
}