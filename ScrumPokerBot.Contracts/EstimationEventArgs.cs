using System;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public class EstimationEventArgs : EventArgs
    {
        public EstimationEventArgs(EstimationMessage estimationMessage)
        {
            EstimationMessage = estimationMessage;
        }

        public EstimationMessage EstimationMessage { get;  }
    }
}