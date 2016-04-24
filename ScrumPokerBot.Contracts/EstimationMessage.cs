namespace ScrumPokerBot.Contracts
{
    public class EstimationMessage : TelegramMessageBase
    {
        public int Estimation { get; set; }

        public EstimationMessage(int id, PokerUser user, string text, int estimation): base(id,user, text)
        {
            Estimation = estimation;
        }

        public override CommandType CommandType => CommandType.Estimation;
    }
}