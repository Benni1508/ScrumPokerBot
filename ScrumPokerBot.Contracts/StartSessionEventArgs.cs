using System;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public class StartSessionEventArgs : EventArgs
    {
        public StartSessionMessage StartSessionMessage { get; }

        public StartSessionEventArgs(StartSessionMessage startSessionMessage)
        {
            StartSessionMessage = startSessionMessage;
        }
    }
}