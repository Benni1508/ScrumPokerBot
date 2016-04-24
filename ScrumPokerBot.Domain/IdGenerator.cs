using System;

namespace ScrumPokerBot.Domain
{
    internal class IdGenerator : IIdGenerator
    {
        public int GetId()
        {
            var rnd = new Random();
            return rnd.Next(1, 100);
        }
    }
}