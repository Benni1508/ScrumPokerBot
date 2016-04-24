using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumPokerBot.Contracts;
using Telegram.Bot.Types;

namespace ScrumPokerBot.Domain
{
    public class ScrumPokerSession
    {
        private readonly List<PokerUser> users;
        
        public ScrumPokerSession(PokerUser user, int id)
        {
            this.users = new List<PokerUser>();
            this.users.Add(user);
            this.Id = id;
            this.MasterUser = user;
        }

        public int Id { get; set; }

        

        public long ChatId { get; }
        public PokerUser MasterUser { get; }
        public PokerUser[] AllUsers => this.users.ToArray();

        public void AddUser(PokerUser user)
        {
            users.Add(user);
        }

    }
}

