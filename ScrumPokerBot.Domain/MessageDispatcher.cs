using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public class MessageDispatcher
    {
        private readonly IMessageReceiver messageReceiver;
        private readonly IScrumPokerService pokerService;

        public MessageDispatcher(IMessageReceiver messageReceiver, IScrumPokerService pokerService)
        {
            this.messageReceiver = messageReceiver;
            this.pokerService = pokerService;
            this.messageReceiver.MessageReceived += MessageReceiverOnMessageReceived;
        }

        private void MessageReceiverOnMessageReceived(object sender, MessageReceivedEventArgs messageReceivedEventArgs)
        {
            var message = messageReceivedEventArgs.TelegramMessage;

            if (message is StartSessionMessage)
            {
                this.pokerService.OnStartSessionMessageReceived((StartSessionMessage) message);
            }
            else if (message is ConnectSessionMessage)
            {
                this.pokerService.OnConnectedMessageReceived((ConnectSessionMessage) message);
            }
            else if (message is EstimationMessage)
            {
                this.pokerService.OnEstimationMessageReceived((EstimationMessage) message);
            }
            else if (message is LeaveSessionMessage)
            {
                this.pokerService.OnLeaveSessionMessageReceived((LeaveSessionMessage) message);
            }
            else if (message is StartPokerMessage)
            {
                this.pokerService.OnStartPokerMessageReceived((StartPokerMessage)message);
            }    
            else if (message is UnknownCommandMessage)
            {
                this.pokerService.OnUnknownMessageReceived((UnknownCommandMessage)message);
            }
        }
    }
}