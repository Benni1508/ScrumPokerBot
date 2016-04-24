using System;

namespace ScrumPokerBot.Domain
{
    public interface IIdGenerator
    {
        int GetId();
    }

    class IdGenerator : IIdGenerator
    {
        public int GetId()
        {
            var rnd = new Random();
            return rnd.Next(1, 100);
        }
    }
}