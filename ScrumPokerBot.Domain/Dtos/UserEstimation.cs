using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain.Dtos
{
    public class UserEstimation
    {
        public PokerUser User { get; private set; }

        public UserEstimation(PokerUser user)
        {
            User = user;
        }

        public int Estimation { get; private set; }
        public bool EstimationReceived { get; private set; }

        public void SetEstimation(int estimation)
        {
            EstimationReceived = true;
            Estimation = estimation;
        }
    }
}