using System.Collections.Generic;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public interface IScrumPokerService
    {
        List<ScrumPokerSession> ScrumPokerSessions { get; }
        void OnConnectedMessageReceived(ConnectSessionMessage message);
        void OnStartPokerMessageReceived(StartPokerMessage startPokerMessage);
        void OnStartSessionMessageReceived(StartSessionMessage e);
        void OnUnknownMessageReceived(UnknownCommandMessage e);
        void OnEstimationMessageReceived(EstimationMessage estimationMessage);
        void OnLeaveSessionMessageReceived(LeaveSessionMessage e);
    }
}