using System.Collections.Generic;
using System.Linq;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public class ScrumPokerService : IScrumPokerService
    {
        private readonly IIdGenerator idGenerator;
        private readonly IMessageSender messageSender;

        public ScrumPokerService(IMessageSender messageSender, IIdGenerator idGenerator)
        {
            this.messageSender = messageSender;
            this.idGenerator = idGenerator;
        }

        public List<ScrumPokerSession> ScrumPokerSessions { get; } = new List<ScrumPokerSession>();

        public int StartNewSession(PokerUser user)
        {
            var existingSession = GetSession(user);
            if (existingSession != null)
            {
                messageSender.UserAlreadyInSession(user);
            }
            var newSession = new ScrumPokerSession(user, idGenerator.GetId());
            ScrumPokerSessions.Add(newSession);
            messageSender.SendStartSessionToMaster(user, newSession.Id);
            return newSession.Id;
        }

        public void ConnectToSession(PokerUser user, int sessionId)
        {
            var session = GetSession(sessionId);
            if (session != null && session.AllUsers.All(u => u.Username != user.Username))
            {
                session.AddUser(user);
                messageSender.InformaAddedUserAndMaster(user, session.MasterUser);
            }
            if (session == null)
            {
                messageSender.NoSessionFound(user, sessionId);
            }
        }

        public void LeaveSession(PokerUser user)
        {
            var session = GetSession(user);
            messageSender.SendUserLeaveSession(session.MasterUser, user);
            session.RemoveUser(user);
        }

        public void StartPoker(PokerUser user, string description)
        {
            var session = GetSession(user);
            if (session == null)
            {
                messageSender.NoSessionForUser(user);
                return;
            }
            if (!session.CanStartPoker)
            {
                messageSender.PokerAlreadyRunning(user.ChatId);
            }
            session.StartPoker();
            messageSender.SendPokerToUsers(session.AllUsers, description);
        }

        private ScrumPokerSession GetSession(PokerUser user)
        {
            var session = ScrumPokerSessions.FirstOrDefault(s => s.AllUsers.Any(u => u.ChatId == user.ChatId));
            return session;
        }



        private ScrumPokerSession GetSession(int sessionId)
        {
            return ScrumPokerSessions.SingleOrDefault(s => s.Id == sessionId);
        }

        public void Estimate(PokerUser user, int estimation)
        {
            var session = GetSession(user);
            if (session.CanStartPoker)
            {
                messageSender.NoPokerRunning(user);
                return;
            }
            else if (!session.CanUserEstimate(user))
            {
                this.messageSender.EstimationAlreadyCounted(user);
            }
            if (!session.Estimate(user, estimation)) return;

            this.messageSender.SendPokerResult(session);
            session.ClearPoker();
        }
    }
}