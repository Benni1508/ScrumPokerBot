using System;
using System.Collections;
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
            ScrumPokerSessions = new MyList<ScrumPokerSession>();
            ;
        }

        public MyList<ScrumPokerSession> ScrumPokerSessions { get; }

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
            try
            {
                var session = GetSession(sessionId);
                if (session != null && session.AllUsers.All(u => u.ChatId != user.ChatId))
                {
                    session.AddUser(user);
                    messageSender.InformaAddedUserAndMaster(user, session.MasterUser);
                }
                if (session == null)
                {
                    messageSender.NoSessionFound(user, sessionId);
                }
            }
            catch (NullReferenceException e)
            {
            }
        }

        public void LeaveSession(PokerUser user)
        {
            var session = GetSession(user);
            if (!EnsureSession(user, session)) return;

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
            if (!EnsureSession(user, session)) return;

            if (!session.CanStartPoker)
            {
                messageSender.PokerAlreadyRunning(user.ChatId);
            }
            session.StartPoker();
            messageSender.SendPokerToUsers(session.AllUsers, description);
        }

        public void Estimate(PokerUser user, int estimation)
        {
            var session = GetSession(user);
            if (!EnsureSession(user, session))
            {
                return;
            }

            if (session.CanStartPoker)
            {
                messageSender.NoPokerRunning(user);
                return;
            }
            if (!session.CanUserEstimate(user))
            {
                messageSender.EstimationAlreadyCounted(user);
            }
            if (!session.Estimate(user, estimation)) return;

            messageSender.SendPokerResult(session, session.Poker.ToString());
            session.ClearPoker();
        }

        public void ShowAllUsers(PokerUser user)
        {
            var session = GetSession(user);
            if (!EnsureMasterUser(user, session) || !EnsureMasterUser(user, session)) return;

            messageSender.SendUsers(session.AllUsers, user);
        }

        private void CloseSession(ScrumPokerSession session)
        {
            var users = session.AllUsers.ToArray();
            messageSender.InformUserSessionEnded(session, users);
            ScrumPokerSessions.Remove(session.Id);
        }

        private bool EnsureSession(PokerUser user, ScrumPokerSession session)
        {
            if (session != null) return true;
            messageSender.NoSessionForUser(user);
            return false;
        }

        private bool EnsureMasterUser(PokerUser user, ScrumPokerSession session)
        {
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

    public class MyList<T> : IEnumerable<T> where T: IHaveId
    {
        private readonly List<T> items;
        private readonly object lockObj;

        public MyList()
        {
            items = new List<T>();
            lockObj = new object();
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (lockObj)
            {
                return items.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T session)
        {
            lock (lockObj)
            {
                if (session == null)
                {
                    
                }
                items.Add(session);
            }
        }

        public T[] ToArrayLocked()
        {
            lock (lockObj)
            {
                return this.items.ToArray();
            }
        }

        public void Remove(int sessionId)
        {
            lock (lockObj)
            {
                var session = items.FirstOrDefault(g => g.Id == sessionId);
                if (session != null)
                {
                    items.Remove(session);
                }
            }
        }
    }
    
}