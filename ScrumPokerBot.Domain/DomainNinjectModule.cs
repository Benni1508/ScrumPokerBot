﻿using Ninject;
using Ninject.Modules;
using ScrumPokerBot.Contracts;
using ScrumPokerBot.Domain.Interfaces;
using ScrumPokerBot.Domain.MessageBus;

namespace ScrumPokerBot.Domain
{
    public class DomainNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IScrumPokerService>().To<ScrumPokerService>().InSingletonScope();
            Bind<IMessageSender>().To<MessageSender>();
            Bind<IIdGenerator>().To<IdGenerator>();
            Bind<IMessageBus>().To<MessageBus.MessageBus>().InSingletonScope();

            Bind<MessageHandlers>().ToSelf();

            
            this.Kernel.Get<MessageHandlers>();
        }
    }
}