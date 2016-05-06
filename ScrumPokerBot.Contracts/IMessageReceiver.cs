using System;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public interface IMessageReceiver
    {
        event EventHandler<ConnectMessageEventArgs> ConnectedMessageReceived;
        event EventHandler<EstimationMessage> EstimationMessageReceived;
        event EventHandler<StartSessionMessage> StartSessionMessageReceived;
        event EventHandler<StartPokerMessage> StartPokerMessageReceived;
        event EventHandler<UnknownCommandMessage> UnknownMessageReceived;
    }

    public class ConnectMessageEventArgs : EventArgs
    {
        public ConnectSessionMessage ConnectSessionMessage { get; private set; }

        public ConnectMessageEventArgs(ConnectSessionMessage connectSessionMessage)
        {
            ConnectSessionMessage = connectSessionMessage;
        }
    }
}