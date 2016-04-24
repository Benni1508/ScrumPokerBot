using System.Collections.Generic;
using System.Linq;

namespace ScrumPokerBot.Domain
{
    public class RunningPoker
    {
        public RunningPoker(ScrumPokerSession session)
        {
            Users = session.AllUsers.Select(u => new UserEstimation(u.ChatId)).ToList();
            SessionId = session.Id;
        }

        public int SessionId { get; }
        public List<UserEstimation> Users { get; set; }
    }
}