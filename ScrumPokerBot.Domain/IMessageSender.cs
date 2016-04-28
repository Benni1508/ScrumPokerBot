using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public interface IMessageSender
    {
        void SendEndSession(PokerUser[] allUsers);
        void InformaAddedUserAndMaster(PokerUser any, PokerUser masterUser);
        void SendStartSessionToMaster(PokerUser user, int sessionId);
        void SendUnknownCommand(ITelegramMessage message);
        void NoSessionFound(PokerUser user, int sessionId);
        void NoSessionForUser(PokerUser user);
        void PokerAlreadyRunning(long requesterChatId);
        void SendPokerToUsers(PokerUser[] allUsers, string description);
        void SendPokerResult(ScrumPokerSession session, RunningPoker pokerSession);
        void NoPokerRunning(PokerUser user);
    }
}