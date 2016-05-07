using System;

namespace ScrumPokerBot.Domain
{
    internal class IdGenerator : IIdGenerator
    {
        private int current;

        public IdGenerator()
        {
            this.current = new Random().Next(1,100);
        }

        public int GetId()
        {
            return current++;
        }
    }
}