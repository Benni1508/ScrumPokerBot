using System.Collections.Generic;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public interface IScrumPokerService
    {
        List<ScrumPokerSession> ScrumPokerSessions { get; }
        void StartService();
        void LeaveSession(PokerUser user);
        void ShowResult(int sessionId);
        void EndPoker(int sessionId);
        int StartNewSession(PokerUser user);
        void AddUserToSession(PokerUser user, int sessionId);
        void StartPoker(int sessionId, string description, long requesterChatId);
        ScrumPokerSession GetSession(PokerUser user);
    }
}