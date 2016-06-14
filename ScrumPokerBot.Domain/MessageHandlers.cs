using ScrumPokerBot.Contracts;
using ScrumPokerBot.Contracts.Messages;
using ScrumPokerBot.Domain.Interfaces;

namespace ScrumPokerBot.Domain
{
    public class MessageHandlers : IHandle<StartSessionMessage>, IHandle<ConnectSessionMessage>,
        IHandle<StartPokerMessage>, IHandle<EstimationMessage>, IHandle<LeaveSessionMessage>, IHandle<CancelPokerMessage>,
        IHandle<UnknownCommandMessage>, IHandle<GetSessionUsersMessage>, IHandle<ShowSessionsMessage>
    {
        private readonly IScrumPokerService service;
        private readonly IMessageSender messageSender;

        public MessageHandlers(IScrumPokerService service, IMessageBus bus, IMessageSender messageSender)
        {
            this.service = service;
            this.messageSender = messageSender;
            bus.Subscribe<StartSessionMessage>(this);
            bus.Subscribe<GetSessionUsersMessage>(this);
            bus.Subscribe<ShowSessionsMessage>(this);
            bus.Subscribe<ConnectSessionMessage>(this);
            bus.Subscribe<StartPokerMessage>(this);
            bus.Subscribe<EstimationMessage>(this);
            bus.Subscribe<LeaveSessionMessage>(this);
            bus.Subscribe<UnknownCommandMessage>(this);
            bus.Subscribe<CancelPokerMessage>(this);
        }

        public void Handle(StartSessionMessage message)
        {
            service.StartNewSession(message.User);
        }

        public void Handle(ConnectSessionMessage message)
        {
            if (message.IsValid)
            {
                service.ConnectToSession(message.User, message.Sessionid, message.MessageId);
            }
            else
            {
                service.SendConnections(message.User);
            }
        }

        public void Handle(StartPokerMessage message)
        {

            service.StartPoker(message.User, message.Description);
        }

        public void Handle(EstimationMessage message)
        {
            service.Estimate(message.User, message.Estimation, message.MessageId);
        }


        public void Handle(LeaveSessionMessage message)
        {
            service.LeaveSession(message.User);
        }

        public void Handle(UnknownCommandMessage message)
        {
            this.messageSender.SendUnknownCommand(message);
        }

        public void Handle(GetSessionUsersMessage message)
        {
            this.service.ShowAllUsers(message.User);
        }

        public void Handle(CancelPokerMessage message)
        {
            this.service.CancelPoker(message.User);
        }

        public void Handle(ShowSessionsMessage message)
        {
            this.service.ShowSessions(message.User);
        }
    }
}