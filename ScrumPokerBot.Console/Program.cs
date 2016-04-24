using Ninject;
using ScrumPokerBot.Domain;
using ScrumPokerBot.Telgram;
using Topshelf;

namespace ScrumPokerBot.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Bind<IService>().To<Service>();
            kernel.Load<TelegramNinjectModule>();
            kernel.Load<DomainNinjectModule>();

            HostFactory.Run(x =>
            {
                x.Service<IService>(s =>
                {
                    s.ConstructUsing(_ => kernel.Get<IService>());
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                });
                x.RunAsLocalSystem();
            });
        }
    }
}