using System;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public class StartPokerEventArgs : EventArgs
    {
        public StartPokerEventArgs(StartPokerMessage startPokerMessage)
        {
            StartPokerMessage = startPokerMessage;
        }

        public StartPokerMessage StartPokerMessage { get; }
    }
}