using FluentAssertions;
using ScrumPokerBot.Contracts.Messages;
using Xunit;

namespace ScrumPokerBot.Domain.Tests
{
    public class EstimationMessageTests
    {
        [Fact]
        public void MessageTest1()
        {
            var message = new EstimationMessage(TestHelpers.GetTestUser(1), "1 Story Points");
            message.IsValid.Should().BeTrue();
            message.Estimation.Should().Be(1);
        }

        [Fact]
        public void MessageTest2()
        {
            var message = new EstimationMessage(TestHelpers.GetTestUser(1), "Story Points");
            message.IsValid.Should().BeFalse();
            message.Estimation.Should().Be(0);
        }

        [Fact]
        public void MessageTest3()
        {
            var message = new EstimationMessage(TestHelpers.GetTestUser(1), "asd Story Points");
            message.IsValid.Should().BeFalse();
            message.Estimation.Should().Be(0);
        }
    }
}