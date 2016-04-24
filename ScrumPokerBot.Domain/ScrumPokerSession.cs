using System.Collections.Generic;
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

        public int Id { get; set; }
        public long ChatId { get; }
        public PokerUser MasterUser { get; }
        public PokerUser[] AllUsers => users.ToArray();

        public void AddUser(PokerUser user)
        {
            users.Add(user);
        }
    }
}