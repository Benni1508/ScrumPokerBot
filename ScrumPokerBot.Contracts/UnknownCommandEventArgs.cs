using System;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public class UnknownCommandEventArgs : EventArgs
    {
        public UnknownCommandEventArgs(UnknownCommandMessage unknownCommandMessage)
        {
            UnknownCommandMessage = unknownCommandMessage;
        }

        public UnknownCommandMessage UnknownCommandMessage { get; }
    }
}