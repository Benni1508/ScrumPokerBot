using System;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public interface IMessageReceiver
    {
        event EventHandler<ConnectEventArgs> ConnectedMessageReceived;
        event EventHandler<EstimationEventArgs> EstimationMessageReceived;
        event EventHandler<StartSessionEventArgs> StartSessionMessageReceived;
        event EventHandler<StartPokerEventArgs> StartPokerMessageReceived;
        event EventHandler<UnknownCommandEventArgs> UnknownMessageReceived;
        event EventHandler<LeaveSessionEventArgs> LeaveSessionMessageReceived;
    }

    public class LeaveSessionEventArgs :EventArgs
    {
        public LeaveSessionEventArgs(LeaveSessionMessage leaveSessionMessage)
        {
            LeaveSessionMessage = leaveSessionMessage;
        }

        public LeaveSessionMessage LeaveSessionMessage { get;}
    }
}