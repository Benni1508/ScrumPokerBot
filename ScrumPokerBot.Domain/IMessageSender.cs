using ScrumPokerBot.Contracts;
using ScrumPokerBot.Contracts.Messages;

namespace ScrumPokerBot.Domain
{
    public interface IMessageSender
    {
        void SendEndSession(PokerUser[] allUsers);
        void InformaAddedUserAndMaster(PokerUser any, PokerUser masterUser);
        void SendStartSessionToMaster(PokerUser user, int sessionId);
        void SendUnknownCommand(UnknownCommandMessage message);
        void NoSessionFound(PokerUser user, int sessionId);
        void NoSessionForUser(PokerUser user);
        void PokerAlreadyRunning(long requesterChatId);
        void SendPokerToUsers(PokerUser[] allUsers, string description);
        void SendPokerResult(ScrumPokerSession session);
        void NoPokerRunning(PokerUser user);
        void UserAlreadyInSession(PokerUser user);
        void SendUserLeaveSession(PokerUser masterUser, PokerUser user);
        void EstimationAlreadyCounted(PokerUser user);
        void NotMasterUser(PokerUser user);
        void SendUsers(PokerUser[] allUsers, PokerUser user);
    }
}