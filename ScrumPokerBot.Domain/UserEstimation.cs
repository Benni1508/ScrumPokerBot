namespace ScrumPokerBot.Domain
{
    public class UserEstimation
    {
        public UserEstimation(long userId)
        {
            UserId = userId;
        }

        public int Estimation { get; private set; }
        public long UserId { get; }
        public bool EstimationReceived { get; private set; }

        public void SetEstimation(int estimation)
        {
            EstimationReceived = true;
            Estimation = estimation;
        }
    }
}