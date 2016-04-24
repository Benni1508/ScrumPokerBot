using Ninject.Modules;

namespace ScrumPokerBot.Domain
{
    public class DomainNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IScrumPokerService>().To<ScrumPokerService>().InSingletonScope();
            Bind<IMessageSender>().To<MessageSender>();
            Bind<IIdGenerator>().To<IdGenerator>();
        }
    }
}