using System;

namespace ScrumPokerBot.Domain
{
    public interface IMessageReceiver
    {
        event EventHandler<ConnectEventArgs> ConnectedMessageReceived;
        event EventHandler<EstimationEventArgs> EstimationMessageReceived;
        event EventHandler<StartSessionEventArgs> StartSessionMessageReceived;
        event EventHandler<StartPokerEventArgs> StartPokerMessageReceived;
        event EventHandler<UnknownCommandEventArgs> UnknownMessageReceived;
    }
}