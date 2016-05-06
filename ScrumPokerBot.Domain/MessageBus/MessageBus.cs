using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly IDictionary<Type, IList> subscriptions = new Dictionary<Type, IList>();

        public void Publish<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            if (message == null) throw new ArgumentNullException("message");

            Type messageType = typeof(TMessage);
            if (!subscriptions.ContainsKey(messageType)) return;

            var subscriptionList = new List<ISubscription<TMessage>>(subscriptions[messageType].Cast<ISubscription<TMessage>>());
            foreach (var subscription in subscriptionList)
                subscription.Action.Handle(message);
        }

        public ISubscription<TMessage> Subscribe<TMessage>(IHandle<TMessage> handler)
            where TMessage : IMessage
        {
            Type messageType = typeof(TMessage);
            var subscription = new Subscription<TMessage>(handler);

            if (subscriptions.ContainsKey(messageType))
                subscriptions[messageType].Add(subscription);
            else
                subscriptions.Add(messageType, new List<ISubscription<TMessage>> { subscription });

            return subscription;
        }
        


    }
}