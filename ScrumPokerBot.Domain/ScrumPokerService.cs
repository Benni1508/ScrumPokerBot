using System;
using System.Collections.Generic;
using System.Linq;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public class ScrumPokerService : IScrumPokerService
    {
        private readonly IIdGenerator idGenerator;
        private readonly IMessageReceiver messageReceiver;
        private readonly IMessageSender messageSender;
        private readonly List<RunningPoker> runningPokers = new List<RunningPoker>();

        public ScrumPokerService(IMessageSender messageSender, IIdGenerator idGenerator,
            IMessageReceiver messageReceiver)
        {
            this.messageSender = messageSender;
            this.idGenerator = idGenerator;
            this.messageReceiver = messageReceiver;
        }

        public List<ScrumPokerSession> ScrumPokerSessions { get; } = new List<ScrumPokerSession>();

        public int StartNewSession(PokerUser user)
        {
            var newSession = new ScrumPokerSession(user, idGenerator.GetId());
            ScrumPokerSessions.Add(newSession);
            messageSender.SendStartSessionToMaster(user, newSession.Id);
            return newSession.Id;
        }

        public void AddUserToSession(PokerUser user, int sessionId)
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

        public void StartService()
        {
            messageReceiver.MessageReceived += MessageReceiverOnMessageReceived;
        }

        public void EndSession(int sessionId)
        {
            var session = GetSession(sessionId);
            messageSender.SendEndSession(session.AllUsers);
            ScrumPokerSessions.Remove(session);
        }

        public void StartPoker(int sessionId, string description, long requesterChatId)
        {
            var session = GetSession(sessionId);
            if (runningPokers.Any(r => r.SessionId == sessionId))
            {
                messageSender.PokerAlreadyRunning(requesterChatId);
            }

            runningPokers.Add(new RunningPoker(session));
            messageSender.SendPokerToUsers(session.AllUsers, description);
        }

        public void ShowResult(int sessionId)
        {
            throw new NotImplementedException();
        }

        public void EndPoker(int sessionId)
        {
            throw new NotImplementedException();
        }

        public ScrumPokerSession GetSessionForUser(PokerUser user)
        {
            var session = ScrumPokerSessions.FirstOrDefault(s => s.AllUsers.Any(u => u.ChatId == user.ChatId));
            return session;
        }

        private void MessageReceiverOnMessageReceived(object sender, ITelegramMessage telegramMessage)
        {
            try
            {
                if (telegramMessage is StartSessionMessage)
                {
                    StartNewSession(telegramMessage.User);
                }
                if (telegramMessage is ConnectSessionMessage)
                {
                    var connectedMessage = (ConnectSessionMessage) telegramMessage;
                    AddUserToSession(telegramMessage.User, connectedMessage.Sessionid);
                }
                if (telegramMessage is StartPokerMessage)
                {
                    var session = GetSessionForUser(telegramMessage.User);
                    if (session == null)
                    {
                        messageSender.NoSessionForUser(telegramMessage.User);
                        return;
                    }
                    var startMessage = telegramMessage as StartPokerMessage;
                    StartPoker(session.Id, startMessage.Description, startMessage.ChatId);
                }

                if (telegramMessage is EstimationMessage)
                {
                    var estimationMessage = telegramMessage as EstimationMessage;
                    var session = GetSessionForUser(telegramMessage.User);
                    var pokerSession = runningPokers.FirstOrDefault(p => p.SessionId == session.Id);
                    var userEstimation =
                        pokerSession.Users.FirstOrDefault(ue => ue.UserId == telegramMessage.User.ChatId);
                    if (userEstimation != null && !userEstimation.EstimationReceived)
                    {
                        userEstimation.SetEstimation(estimationMessage.Estimation);
                    }
                    ValidateEstimation(pokerSession, session);
                }
                if (telegramMessage is UnknownCommandMessage)
                {
                    messageSender.SendUnknownCommand(telegramMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ValidateEstimation(RunningPoker pokerSession, ScrumPokerSession session)
        {
            if (pokerSession.Users.All(u => u.EstimationReceived))
            {
                messageSender.SendPokerResult(session, pokerSession);
                runningPokers.Remove(pokerSession);
            }
        }

        private ScrumPokerSession GetSession(int sessionId)
        {
            return ScrumPokerSessions.SingleOrDefault(s => s.Id == sessionId);
        }
    }
}