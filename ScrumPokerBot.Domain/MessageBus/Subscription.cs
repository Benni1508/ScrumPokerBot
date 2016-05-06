using System;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain.MessageBus
{
    public class Subscription<TMessage> : ISubscription<TMessage>
        where TMessage : IMessage
    {
        public IHandle<TMessage> Action { get; private set; }

        public Subscription(IHandle<TMessage> action)
        {
            if (action == null) throw new ArgumentNullException("action");
            Action = action;
        }
    }
}