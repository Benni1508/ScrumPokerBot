using ScrumPokerBot.Contracts;
using ScrumPokerBot.Contracts.Messages;

namespace ScrumPokerBot.Domain
{
    public class MessageHandlers : IHandle<StartSessionMessage>, IHandle<ConnectSessionMessage>,
        IHandle<StartPokerMessage>, IHandle<EstimationMessage>, IHandle<LeaveSessionMessage>, IHandle<UnknownCommandMessage>
    {
        private readonly IScrumPokerService service;
        private readonly IMessageBus bus;
        private readonly IMessageSender messageSender;

        public MessageHandlers(IScrumPokerService service, IMessageBus bus, IMessageSender messageSender)
        {
            this.service = service;
            this.bus = bus;
            this.messageSender = messageSender;
            bus.Subscribe<StartSessionMessage>(this);
            bus.Subscribe<ConnectSessionMessage>(this);
            bus.Subscribe<StartPokerMessage>(this);
            bus.Subscribe<EstimationMessage>(this);
            bus.Subscribe<LeaveSessionMessage>(this);
            bus.Subscribe<UnknownCommandMessage>(this);
        }

        public void Handle(StartSessionMessage message)
        {
            service.StartNewSession(message.User);
        }

        public void Handle(ConnectSessionMessage message)
        {
            service.ConnectToSession(message.User, message.Sessionid);
        }

        public void Handle(StartPokerMessage message)
        {

            service.StartPoker(message.User, message.Description, message.User.ChatId);
        }

        public void Handle(EstimationMessage message)
        {
            service.Estimate(message.User, message.Estimation);
        }


        public void Handle(LeaveSessionMessage message)
        {
            service.LeaveSession(message.User);
        }

        public void Handle(UnknownCommandMessage message)
        {
            this.messageSender.SendUnknownCommand(message);
        }
    }
}