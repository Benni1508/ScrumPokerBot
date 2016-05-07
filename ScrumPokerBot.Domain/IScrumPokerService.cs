using System.Collections.Concurrent;
using System.Collections.Generic;
using ScrumPokerBot.Contracts;
using ScrumPokerBot.Contracts.Messages;

namespace ScrumPokerBot.Domain
{
    public interface IScrumPokerService
    {
        MyList ScrumPokerSessions { get; }
        int StartNewSession(PokerUser user);
        void ConnectToSession(PokerUser user, int sessionId);
        void StartPoker(PokerUser user, string description);
        void LeaveSession(PokerUser user);
        void Estimate(PokerUser user, int estimation);
        void ShowAllUsers(PokerUser user);
    }
}