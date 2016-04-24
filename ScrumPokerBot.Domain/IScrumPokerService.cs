using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public interface IScrumPokerService
    {
        void StartService();

        void EndSession(int sessionId);


        void ShowResult(int sessionId);

        void EndPoker(int sessionId);

        int StartNewSession(PokerUser user);

        void AddUserToSession(PokerUser user, int sessionId);
        List<ScrumPokerSession> ScrumPokerSessions { get; }
        void StartPoker(int sessionId, string description, long requesterChatId);
        ScrumPokerSession GetSessionForUser(PokerUser user);
    }
}