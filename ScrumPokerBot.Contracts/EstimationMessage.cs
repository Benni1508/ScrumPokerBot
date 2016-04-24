namespace ScrumPokerBot.Contracts
{
    public class EstimationMessage : TelegramMessageBase
    {
        public EstimationMessage(int id, PokerUser user, string text, int estimation) : base(id, user, text)
        {
            Estimation = estimation;
        }

        public int Estimation { get; set; }
        public override CommandType CommandType => CommandType.Estimation;
    }
}