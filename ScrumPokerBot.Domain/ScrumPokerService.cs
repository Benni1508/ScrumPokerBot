using System.Linq;
using ScrumPokerBot.Contracts;
using ScrumPokerBot.Domain.Dtos;
using ScrumPokerBot.Domain.Interfaces;

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
            ScrumPokerSessions = new LockedList<ScrumPokerSession>();
        }

        internal LockedList<ScrumPokerSession> ScrumPokerSessions { get; }

        public int StartNewSession(PokerUser user)
        {
            var existingSession = GetSession(user);
            if (existingSession != null)
            {
                messageSender.UserAlreadyInSession(user);
                return 0;
            }
            var newSession = new ScrumPokerSession(user, idGenerator.GetId());
            ScrumPokerSessions.Add(newSession);
            messageSender.SendStartSessionToMaster(user, newSession.Id);
            return newSession.Id;
        }

        public void ConnectToSession(PokerUser user, int sessionId, int messageId =0)
        {
            var session = GetSession(sessionId);
            
            if (GetSession(user) != null)
            {
                messageSender.AllreadyConnected(user);
                return;
            }

            if (session != null && session.AllUsers.All(u => u.ChatId != user.ChatId))
            {
                session.AddUser(user);
                messageSender.InformaAddedUserAndMaster(user, session.MasterUser, messageId);
            }
            if (session == null)
            {
                messageSender.NoSessionFound(user, sessionId);
            }
        }

        public void LeaveSession(PokerUser user)
        {
            var session = GetSession(user);
            if (!EnsureSession(user)) return;

            if (user.ChatId == session.MasterUser.ChatId)
            {
                CloseSession(session);
                return;
            }

            messageSender.SendUserLeaveSession(session.MasterUser, user);
            session.RemoveUser(user);
        }

        public void StartPoker(PokerUser user, string description)
        {
            var session = GetSession(user);
            if (!EnsureSession(user)) return;
            if (!EnsureMasterUser(user))
                return;
            if (!session.CanStartPoker)
            {
                messageSender.PokerAlreadyRunning(user.ChatId);
            }
            session.StartPoker();
            messageSender.SendPokerToUsers(session.AllUsers, description);
        }

        public void Estimate(PokerUser user, int estimation, int messageId = 0)
        {
            var session = GetSession(user);
            if (!EnsureSession(user)) return;

            if (session.Poker == null)
            {
                messageSender.NoPokerRunning(user);
                return;
            }

            if (!session.CanUserEstimate(user))
            {
                messageSender.EstimationAlreadyCounted(user);
            }

            session.Estimate(user, estimation, messageId);
            if (messageId != 0)
            {
                messageSender.UpdateEstimation(user, estimation, messageId);
            }

            if (!session.IsEstimationCompleted()) return;

            messageSender.SendPokerResult(session, session.Poker.ToString());
            session.ClearPoker();
        }

        public void ShowAllUsers(PokerUser user)
        {
            var session = GetSession(user);
            
            if (!EnsureSession(user)) return;
            if (!EnsureMasterUser(user)) return;

            messageSender.SendUsers(session.AllUsers, user);
        }

        public void SendConnections(PokerUser user)
        {
            if (GetSession(user) != null)
            {
                messageSender.AllreadyConnected(user);
                return;
            }

            if (ScrumPokerSessions.Any())
            {
                messageSender.SendConnections(user, ScrumPokerSessions.ToArrayLocked());
            }
            else
            {
                messageSender.NoRunningSession(user);
            }
        }

        public void CancelPoker(PokerUser user)
        {
            if (!EnsureSession(user) || !EnsureMasterUser(user))
                return;

            var session = GetSession(user);
            if (session.CanStartPoker)
            {
                messageSender.NoPokerRunning(user);
                return;
            }
            session.Poker.Complete();
            messageSender.SendPokerResult(session, session.Poker.ToString());
            session.ClearPoker();
        }

        private void CloseSession(ScrumPokerSession session)
        {
            var users = session.AllUsers.ToArray();
            messageSender.InformUserSessionEnded(users);
            ScrumPokerSessions.Remove(session.Id);
        }

        private bool EnsureSession(PokerUser user)
        {
            var session = GetSession(user);
            if (session != null) return true;
            messageSender.NoSessionForUser(user);
            return false;
        }

        private bool EnsureMasterUser(PokerUser user)
        {
            var session = GetSession(user);
            if (session.MasterUser.ChatId == user.ChatId) return true;

            messageSender.NotMasterUser(user);
            return false;
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
    }
}
