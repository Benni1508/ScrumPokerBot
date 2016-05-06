namespace ScrumPokerBot.Domain
{
    public class UserEstimation
    {
        public UserEstimation(long chatId)
        {
            ChatId = chatId;
        }

        public int Estimation { get; private set; }
        public long ChatId { get; }
        public bool EstimationReceived { get; private set; }

        public void SetEstimation(int estimation)
        {
            EstimationReceived = true;
            Estimation = estimation;
        }
    }
}