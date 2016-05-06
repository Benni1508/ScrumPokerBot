using System.Linq;
using System.Text;
using ScrumPokerBot.Contracts;
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

        public void SendUnknownCommand(ITelegramMessage message)
        {
            var text = $"Der Befehl \"{message.Message}\" ist nicht unbekannt!";
            bot.SendTextMessage(message.User.ChatId, text);
        }

        public void NoSessionFound(PokerUser user, int sessionId)
        {
            var text = $"Die Id ({sessionId}) wurde nicht gefudnen.";
            bot.SendTextMessage(user.ChatId, text);
        }

        public void NoSessionForUser(PokerUser user)
        {
            var text = $"Du bist in keine laufenden Session";
            bot.SendTextMessage(user.ChatId, text);
        }

        public void PokerAlreadyRunning(long requesterChatId)
        {
            var text = "Es l�uft bereits eine Pokersitzung.";
            bot.SendTextMessage(requesterChatId, text);
        }

        public void SendPokerToUsers(PokerUser[] allUsers, string description)
        {
            var text = "Gib deine Sch�tzunng an";
            foreach (var user in allUsers)
            {
                bot.SendTextMessage(user.ChatId, text, false, 0, keyboardMarkup);
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
                bot.SendTextMessage(pokerUser.ChatId, sb.ToString());
            }
        }

        public void NoPokerRunning(PokerUser user)
        {
            var text = "Aktuell l�uft keine Pokersession.";
            bot.SendTextMessage(user.ChatId, text);
        }

        public void UserAlreadyInSession(PokerUser user)
        {
            var text = "Du bist bereits in einer Session!";
            bot.SendTextMessage(user.ChatId, text);
        }

        public void SendUserLeaveSession(PokerUser masterUser, PokerUser user)
        {
            var masterText = $"Der User {user.ToString()} hat die Session verlassen!";
            var userText = "Du hast die Session verlassen";
            bot.SendTextMessage(masterUser.ChatId, masterText);
            bot.SendTextMessage(user.ChatId, userText);
        }

        public void InformaAddedUserAndMaster(PokerUser any, PokerUser masterUser)
        {
            var masterText = $"Der Benutzer {any.Lastname}, {any.Firstname} wurde der Session hinzugef�gt.";
            var otherText = $"Du nimmst an der Sitzung von {masterUser.Lastname}, {masterUser.Firstname} teil.";
            bot.SendTextMessage(any.ChatId, otherText);
            bot.SendTextMessage(masterUser.ChatId, masterText);
        }
    }
}