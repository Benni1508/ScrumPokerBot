using System.Xml;
using ScrumPokerBot.Contracts;
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

    public class HandlerTest1  : IHandle<TestMessage1>
    {
        public void Handle(TestMessage1 message)
        {
            
        }
    }

    public interface ISuper : IMessage { }

    public class TestMessage1 : IMessage
    {
        
    }
}