using System.Collections.Generic;
using System.Linq;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public class ScrumPokerSession
    {
        private readonly List<PokerUser> users;

        public ScrumPokerSession(PokerUser user, int id)
        {
            users = new List<PokerUser>();
            users.Add(user);
            Id = id;
            MasterUser = user;
        }

        public int Id { get; }
        public PokerUser MasterUser { get; }


        public PokerUser[] AllUsers => users.ToArray();
        public bool CanStartPoker { get { return this.Poker == null; } }
        public RunningPoker Poker { get; private set; }

        public void AddUser(PokerUser user)
        {
            users.Add(user);
        }

        public void RemoveUser(PokerUser user)
        {
            var foundUser = users.FirstOrDefault(u => u.ChatId == user.ChatId);
            if (foundUser != null )
            {
                users.Remove(foundUser);
            }
        }

        public void StartPoker()
        {
            this.Poker = new RunningPoker(this);
        }

        public bool Estimate(PokerUser user, int estimation)
        {
            var userEstimation = this.Poker.Users.FirstOrDefault(ue => ue.ChatId == user.ChatId);
            if (userEstimation != null && !userEstimation.EstimationReceived)
            {
                userEstimation.SetEstimation(estimation);
            }
            return IsEstimationCompleted();
        }

        private bool IsEstimationCompleted()
        {
            return this.Poker.Users.All(u => u.EstimationReceived);
        }

        public void ClearPoker()
        {
            this.Poker = null;
        }

        public bool CanUserEstimate(PokerUser user)
        {
            var estimation = this.Poker.Users.FirstOrDefault(u => u.ChatId == user.ChatId);
            return !estimation.EstimationReceived;
        }
    }
}