using FluentAssertions;
using ScrumPokerBot.Contracts.Messages;
using Xunit;

namespace ScrumPokerBot.Domain.Tests
{
    public class StartPokerMessageTests
    {
        [Fact]
        public void MessageTest1()
        {
            var message = new StartPokerMessage(TestHelpers.GetTestUser(1), "/poker qw");
            message.Description.Should().Be("qw");
        }

        [Fact]
        public void MessageTest2()
        {
            var message = new StartPokerMessage(TestHelpers.GetTestUser(1), "/poker");
            message.Description.Should().Be("");
        }

        [Fact]
        public void MessageTest3()
        {
            var message = new StartPokerMessage(TestHelpers.GetTestUser(1), "/poker Enter BLI here");
            message.Description.Should().Be("Enter BLI here");
        }
    }
}