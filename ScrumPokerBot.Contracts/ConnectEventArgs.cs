using System;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public class ConnectEventArgs : EventArgs
    {
        public ConnectSessionMessage ConnectSessionMessage { get; }

        public ConnectEventArgs(ConnectSessionMessage connectSessionMessage)
        {
            ConnectSessionMessage = connectSessionMessage;
        }
    }
}