using System.Xml;
using ScrumPokerBot.Domain.MessageBus;
using Xunit;

namespace ScrumPokerBot.Domain.Tests
{
    public class MessageBusTests
    {
        [Fact]
        public void Test1()
        {
            var bus = new MessageBus.MessageBus();
            var handler = new HandlerTest1();
            bus.Subscribe(handler);

            bus.Publish(new TestMessage1());
        }
            

    }
}