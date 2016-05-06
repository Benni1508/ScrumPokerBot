using FluentAssertions;
using ScrumPokerBot.Contracts.Messages;
using Xunit;

namespace ScrumPokerBot.Domain.Tests
{
    public class ConnectionMessageTests
    {
        [Fact]
        public void CreateMessage()
        {
            var message = new ConnectSessionMessage(TestHelpers.GetTestUser(1), "/connect 12");
            message.Sessionid.Should().Be(12);
            message.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateMessage2()
        {
            var message = new ConnectSessionMessage(TestHelpers.GetTestUser(1), "/connect qw");
            message.Sessionid.Should().Be(0);
            message.IsValid.Should().BeFalse();
        }

        [Fact]
        public void CreateMessage3()
        {
            var message = new ConnectSessionMessage(TestHelpers.GetTestUser(1), "/connect qw");
            message.Sessionid.Should().Be(0);
            message.IsValid.Should().BeFalse();
        }
    }
}