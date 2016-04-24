namespace ScrumPokerBot.Contracts
{
    public abstract class TelegramMessageBase : ITelegramMessage
    {
        public TelegramMessageBase(long chatId, PokerUser user, string message)
        {
            ChatId = chatId;
            User = user;
            Message = message;
        }

        public long ChatId { get; }
        public PokerUser User { get; }
        public string Message { get; }

        public abstract CommandType CommandType { get; }
    }

    public class UnknownCommandMessage : TelegramMessageBase
    {
        public UnknownCommandMessage(long chatId, PokerUser user, string message) : base(chatId, user, message)
        {
        }

        public override CommandType CommandType { get {return CommandType.NoOrUnknownCommand;} }
    }

    public class StartSessionMessage : TelegramMessageBase
    {
        public StartSessionMessage(long chatId, PokerUser user, string message) : base(chatId, user, message)
        {
        }

        public override CommandType CommandType { get {return CommandType.StartSession;} }
    }

    public class StartPokerMessage : TelegramMessageBase
    {
        public string Description { get; }
        public override CommandType CommandType => CommandType.StartPoker;

        public StartPokerMessage(long chatId, PokerUser user, string message, string description) : base(chatId, user, message)
        {
            Description = description;
        }
    }

    public class ConnectSessionMessage : TelegramMessageBase
    {
        public int Sessionid { get; }

        public ConnectSessionMessage(long chatId, PokerUser user, string message, int sessionid) : base(chatId, user, message)
        {
            this.Sessionid = sessionid;
        }

        public override CommandType CommandType { get; }
    }
}