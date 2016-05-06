using System.Collections.Generic;
using System.Linq;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public class ScrumPokerService : IScrumPokerService
    {
        private readonly IIdGenerator idGenerator;
        private readonly IMessageSender messageSender;
        private readonly List<RunningPoker> runningPokers = new List<RunningPoker>();

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

        public void StartPoker(PokerUser user, string description, long requesterChatId)
        {
            var session = GetSession(user);
            if (session == null)
            {
                messageSender.NoSessionForUser(user);
                return;
            }
            if (runningPokers.Any(r => r.SessionId == session.Id))
            {
                messageSender.PokerAlreadyRunning(requesterChatId);
            }

            runningPokers.Add(new RunningPoker(session));
            messageSender.SendPokerToUsers(session.AllUsers, description);
        }

        private ScrumPokerSession GetSession(PokerUser user)
        {
            var session = ScrumPokerSessions.FirstOrDefault(s => s.AllUsers.Any(u => u.ChatId == user.ChatId));
            return session;
        }

        private void ValidateEstimation(RunningPoker pokerSession, ScrumPokerSession session)
        {
            if (!pokerSession.Users.All(u => u.EstimationReceived)) return;

            messageSender.SendPokerResult(session, pokerSession);
            runningPokers.Remove(pokerSession);
        }

        private ScrumPokerSession GetSession(int sessionId)
        {
            return ScrumPokerSessions.SingleOrDefault(s => s.Id == sessionId);
        }

        public void Estimate(PokerUser user, int estimation)
        {
            var session = GetSession(user);
            var pokerSession = runningPokers.FirstOrDefault(p => p.SessionId == session.Id);
            if (pokerSession == null)
            {
                messageSender.NoPokerRunning(user);
                return;
            }

            var userEstimation = pokerSession.Users.FirstOrDefault(ue => ue.UserId == user.ChatId);
            if (userEstimation != null && !userEstimation.EstimationReceived)
            {
                userEstimation.SetEstimation(estimation);
            }
            ValidateEstimation(pokerSession, session);
        }
    }
}