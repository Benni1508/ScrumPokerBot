using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrumPokerBot.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

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
    }

    class MessageSender : IMessageSender
    {
        private readonly Api bot;

        private readonly ReplyKeyboardMarkup keyboardMarkup = new ReplyKeyboardMarkup() {Keyboard = keyboard};

        private static readonly string[][] keyboard = new[]
        {
            new[] {"0 Story Points"},
            new[] {"1 Story Points"},
            new[] {"2 Story Points"},
            new[] {"3 Story Points"},
            new[] {"5 Story Points"},
            new[] {"8 Story Points"},
            new[] {"13 Story Points"},
            new[] {"20 Story Points"},
        };

        public MessageSender(Api bot)
        {
            this.bot = bot;
        }

        public void SendEndSession(PokerUser[] allUsers)
        {
            
        }

        public void SendStartSessionToMaster(PokerUser user, int sessionId)
        {
            var message = $"Neue Session mit der ID {sessionId} wurde gestartet.";
            bot.SendTextMessage(user.ChatId, message);
        }

        public void SendUnknownCommand(ITelegramMessage message)
        {
            var text = $"Der Befehl \"{message.Message}\" ist nicht unbekannt!";
            this.bot.SendTextMessage(message.ChatId, text);
        }

        public void NoSessionFound(PokerUser user, int sessionId)
        {
            var text = $"Die Id ({sessionId}) wurde nicht gefudnen.";
            this.bot.SendTextMessage(user.ChatId, text);
        }

        public void NoSessionForUser(PokerUser user)
        {
            var text = $"Du bist in keine laufenden Session";
            this.bot.SendTextMessage(user.ChatId, text);
        }

        public void PokerAlreadyRunning(long requesterChatId)
        {
            var text = "Es läuft bereits eine Pokersitzung.";
            this.bot.SendTextMessage(requesterChatId, text);
        }

        public void SendPokerToUsers(PokerUser[] allUsers, string description)
        {
            var text = "Gib deine Schätzunng an";
            foreach (var user in allUsers)
            {
                this.bot.SendTextMessage(user.ChatId, text,false,0,keyboardMarkup);
            }
        }

        public void SendPokerResult(ScrumPokerSession session, RunningPoker pokerSession)
        {
            var x = pokerSession.Users.GroupBy(u => u.Estimation);
            var results = x.ToDictionary(k => k.Key, v => v.Count());
            var sb = new StringBuilder();
            foreach (var result in results)
            {
                sb.AppendLine($"{result.Value} x {result.Key} Story points");
            }
            foreach (var pokerUser in session.AllUsers)
            {
                this.bot.SendTextMessage(pokerUser.ChatId, sb.ToString());
            }
        }


        public void InformaAddedUserAndMaster(PokerUser any, PokerUser masterUser)
        {
            var masterText = $"Der Benutzer {any.Lastname}, {any.Firstname} wurde der Session hinzugefügt.";
            var otherText = $"Du nimmst an der Sitzung von {masterUser.Lastname}, {masterUser.Firstname} teil.";
            this.bot.SendTextMessage(any.ChatId, otherText);
            this.bot.SendTextMessage(masterUser.ChatId, masterText);
        }
    }
}