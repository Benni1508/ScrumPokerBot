using Ninject.Modules;

namespace ScrumPokerBot.Domain
{
    public class DomainNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IScrumPokerService>().To<ScrumPokerService>().InSingletonScope();
            this.Bind<IMessageSender>().To<MessageSender>();
            this.Bind<IIdGenerator>().To<IdGenerator>();
        }
    }
}