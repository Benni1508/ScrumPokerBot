using ScrumPokerBot.Contracts;
using ScrumPokerBot.Domain.Dtos;

namespace ScrumPokerBot.Domain.Interfaces
{
    public interface IScrumPokerService
    {
        MyList<ScrumPokerSession> ScrumPokerSessions { get; }
        int StartNewSession(PokerUser user);
        void ConnectToSession(PokerUser user, int sessionId);
        void StartPoker(PokerUser user, string description);
        void LeaveSession(PokerUser user);
        void Estimate(PokerUser user, int estimation);
        void ShowAllUsers(PokerUser user);
    }
}