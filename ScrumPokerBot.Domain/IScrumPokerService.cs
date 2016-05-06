using System.Collections.Generic;
using ScrumPokerBot.Contracts;
using ScrumPokerBot.Contracts.Messages;

namespace ScrumPokerBot.Domain
{
    public interface IScrumPokerService
    {
        List<ScrumPokerSession> ScrumPokerSessions { get; }
        void OnUnknownMessageReceived(UnknownCommandMessage e);
        int StartNewSession(PokerUser user);
        void ConnectToSession(PokerUser user, int sessionId);
        void StartPoker(PokerUser user, string description, long requesterChatId);
        void LeaveSession(PokerUser user);
        void Estimate(PokerUser user, int estimation);
    }
}