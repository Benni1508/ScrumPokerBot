using System.Linq;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain.Dtos
{
    public class ScrumPokerSession : IHaveId
    {
        private readonly LockedList<PokerUser> users;

        public ScrumPokerSession(PokerUser user, int id)
        {
            users = new LockedList<PokerUser> {user};
            Id = id;
            MasterUser = user;
        }

        public PokerUser MasterUser { get; }


        public PokerUser[] AllUsers => users.ToArrayLocked();
        public bool CanStartPoker => Poker == null;
        public RunningPoker Poker { get; private set; }

        public int Id { get; }

        public void AddUser(PokerUser user)
        {
            users.Add(user);
        }

        public void RemoveUser(PokerUser user)
        {
            var foundUser = users.FirstOrDefault(u => u.ChatId == user.ChatId);
            if (foundUser != null)
            {
                users.Remove((int) foundUser.ChatId);
            }
        }

        public void StartPoker()
        {
            Poker = new RunningPoker(this);
        }

        public bool Estimate(PokerUser user, int estimation)
        {
            var userEstimation = Poker.Users.FirstOrDefault(ue => ue.User.ChatId == user.ChatId);
            if (userEstimation != null && !userEstimation.EstimationReceived)
            {
                userEstimation.SetEstimation(estimation);
            }
            return IsEstimationCompleted();
        }

        private bool IsEstimationCompleted()
        {
            return Poker.Users.All(u => u.EstimationReceived);
        }

        public void ClearPoker()
        {
            Poker = null;
        }

        public bool CanUserEstimate(PokerUser user)
        {
            var estimation = Poker.Users.FirstOrDefault(u => u.User.ChatId == user.ChatId);
            return estimation != null && !estimation.EstimationReceived;
        }
    }
}