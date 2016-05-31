using System.Linq;
using System.Text;
using ScrumPokerBot.Contracts;
using ScrumPokerBot.Contracts.Messages;
using ScrumPokerBot.Domain.Dtos;
using ScrumPokerBot.Domain.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Domain
{
    internal class MessageSender : IMessageSender
    {
        private static readonly string[][] keyboard =
        {
            new[] {"0 Story Points"},
            new[] {"1 Story Points"},
            new[] {"2 Story Points"},
            new[] {"3 Story Points"},
            new[] {"5 Story Points"},
            new[] {"8 Story Points"},
            new[] {"13 Story Points"},
            new[] {"20 Story Points"}
        };

        private static readonly string Connection = "/connect {0} von {1}";

        private readonly Api bot;
        private readonly ReplyKeyboardMarkup keyboardMarkup = new ReplyKeyboardMarkup {Keyboard = keyboard};

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

        public void SendUnknownCommand(UnknownCommandMessage message)
        {
            var text = $"Der Befehl \"{message.Message}\" ist unbekannt!";
            bot.SendTextMessage(message.User.ChatId, text);
        }

        public void NoSessionFound(PokerUser user, int sessionId)
        {
            var text = $"Die Id ({sessionId}) wurde nicht gefudnen.";
            bot.SendTextMessage(user.ChatId, text);
        }

        public void NoSessionForUser(PokerUser user)
        {
            var text = "Du bist in keiner laufenden Session.";
            bot.SendTextMessage(user.ChatId, text);
        }

        public void PokerAlreadyRunning(long requesterChatId)
        {
            var text = "Es läuft bereits eine Pokersitzung.";
            bot.SendTextMessage(requesterChatId, text);
        }

        public void SendPokerToUsers(PokerUser[] allUsers, string description)
        {
            string text;
            text = string.IsNullOrEmpty(description) 
                ? "Gib deine Schätzunng ab:" 
                : $"Gib deine Schätzung für {description} ab.";
            foreach (var user in allUsers)
            {
                bot.SendTextMessage(user.ChatId, text, false, 0, keyboardMarkup);
            }
        }

        public void SendPokerResult(ScrumPokerSession session, string result)
        {
            foreach (var pokerUser in session.AllUsers)
            {
                bot.SendTextMessage(pokerUser.ChatId, result);
            }
        }

        public void NoPokerRunning(PokerUser user)
        {
            var text = "Aktuell läuft keine Pokersession.";
            bot.SendTextMessage(user.ChatId, text);
        }

        public void UserAlreadyInSession(PokerUser user)
        {
            var text = "Du bist bereits in einer Session!";
            bot.SendTextMessage(user.ChatId, text);
        }

        public void SendUserLeaveSession(PokerUser masterUser, PokerUser user)
        {
            var masterText = $"Der User {user} hat die Session verlassen!";
            var userText = "Du hast die Session verlassen";
            bot.SendTextMessage(masterUser.ChatId, masterText);
            bot.SendTextMessage(user.ChatId, userText);
        }

        public void EstimationAlreadyCounted(PokerUser user)
        {
            var text = "Du hast bereits eine Schätzung abgegeben!";
            this.bot.SendTextMessage(user.ChatId, text);
        }

        public void NotMasterUser(PokerUser user)
        {
            var text = "Für diese Funktion musst du Masteruser sein.";
            this.bot.SendTextMessage(user.ChatId, text);
        }

        public void SendUsers(PokerUser[] allUsers, PokerUser user)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Folgede Benutzer nehmen teil:");
            foreach (var pokerUser in allUsers)
            {
                sb.AppendLine($"- {pokerUser}");
            }

            this.bot.SendTextMessage(user.ChatId, sb.ToString());
        }

        public void InformUserSessionEnded(PokerUser[] users)
        {
            var text = "Die Session wurde beendet.";
            foreach (var pokerUser in users)
            {
                bot.SendTextMessage(pokerUser.ChatId, text);
            }
        }

        public void SendConnections(PokerUser user, ScrumPokerSession[] sessions)
        {
            var keyboardLayout = sessions.Select(s => new [] {string.Format(Connection, s.Id, s.MasterUser)}).ToList();
            var keyboardMarkupInternal = new ReplyKeyboardMarkup {Keyboard = keyboardLayout.ToArray()};
            var text = "Wähle die Session!";
            bot.SendTextMessage(user.ChatId, text, false, 0, keyboardMarkupInternal);
        }

        public void NoRunningSession(PokerUser user)
        {
            var text = "Aktuell läuft keine Session, du kannst mit '\\startsession' eine Session starten";
            bot.SendTextMessage(user.ChatId, text);
        }

        public void AllreadyConnected(PokerUser user)
        {
            var text = "Du bist bereits verbunden, mit \\leaveSession kannst du deine aktuelle Session verlassen!";
            bot.SendTextMessage(user.ChatId, text);
        }


        public void InformaAddedUserAndMaster(PokerUser any, PokerUser masterUser)
        {
            var masterText = $"Der Benutzer {any} wurde der Session hinzugefügt.";
            var otherText = $"Du nimmst an der Sitzung von {masterUser} teil.";
            bot.SendTextMessage(any.ChatId, otherText);
            bot.SendTextMessage(masterUser.ChatId, masterText);
        }
    }
}