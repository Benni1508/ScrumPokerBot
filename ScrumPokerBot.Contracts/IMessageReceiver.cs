using System;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public interface IMessageReceiver
    {
        event EventHandler<ConnectSessionMessage> ConnectedMessageReceived;
        event EventHandler<EstimationMessage> EstimationMessageReceived;
        event EventHandler<StartSessionMessage> StartSessionMessageReceived;
        event EventHandler<StartPokerMessage> StartPokerMessageReceived;
        event EventHandler<UnknownCommandMessage> UnknownMessageReceived;
    }
}