namespace ScrumPokerBot.Contracts
{
    public class ConnectSessionMessage : TelegramMessageBase
    {
        public ConnectSessionMessage(long chatId, PokerUser user, string message, int sessionid)
            : base(chatId, user, message)
        {
            Sessionid = sessionid;
        }

        public int Sessionid { get; }
        public override CommandType CommandType { get; }
    }
}