using System.Linq;
using FluentAssertions;
using NSubstitute;
using ScrumPokerBot.Contracts;
using ScrumPokerBot.Telgram;
using Telegram.Bot.Types;
using Xunit;

namespace ScrumPokerBot.Domain.Tests
{
    public class ConnectionMessageTests
    {
        [Fact]
        public void CreateMessage()
        {
            var message = new ConnectSessionMessage(1, TestHelpers.GetTestUser(1), "/connect 12");
            message.Sessionid.Should().Be(12);
            message.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateMessage2()
        {
            var message = new ConnectSessionMessage(1, TestHelpers.GetTestUser(1), "/connect qw");
            message.Sessionid.Should().Be(0);
            message.IsValid.Should().BeFalse();
        }

        [Fact]
        public void CreateMessage3()
        {
            var message = new ConnectSessionMessage(1, TestHelpers.GetTestUser(1), "/connect qw");
            message.Sessionid.Should().Be(0);
            message.IsValid.Should().BeFalse();
        }
    }

    public static class TestHelpers
    {
        public static PokerUser GetTestUser(long chatId)
        {
            return new PokerUser {ChatId = chatId, Firstname = "Test", Lastname = "User", Username = $"Name{chatId}"};
        }
    }

    public class StartPokerMessageTests
    {
        [Fact]
        public void MessageTest1()
        {
            var message = new StartPokerMessage(1, TestHelpers.GetTestUser(1), "/poker qw");
            message.Description.Should().Be("qw");
        }

        [Fact]
        public void MessageTest2()
        {
            var message = new StartPokerMessage(1, TestHelpers.GetTestUser(1), "/poker");
            message.Description.Should().Be("");
        }

        [Fact]
        public void MessageTest3()
        {
            var message = new StartPokerMessage(1, TestHelpers.GetTestUser(1), "/poker Enter BLI here");
            message.Description.Should().Be("Enter BLI here");
        }
    }

    public class EstimationMessageTests
    {
        [Fact]
        public void MessageTest1()
        {
            var message = new EstimationMessage(1, TestHelpers.GetTestUser(1), "1 Story Points");
            message.IsValid.Should().BeTrue();
            message.Estimation.Should().Be(1);
        }

        [Fact]
        public void MessageTest2()
        {
            var message = new EstimationMessage(1, TestHelpers.GetTestUser(1), "Story Points");
            message.IsValid.Should().BeFalse();
            message.Estimation.Should().Be(0);
        }

        [Fact]
        public void MessageTest3()
        {
            var message = new EstimationMessage(1, TestHelpers.GetTestUser(1), "asd Story Points");
            message.IsValid.Should().BeFalse();
            message.Estimation.Should().Be(0);
        }
    }

    public class ScrumPokerServiceTests
    {
        private readonly IMessageReceiver messageReceiver;
        private readonly IMessageSender messageSender;
        private readonly ScrumPokerService service;

        public ScrumPokerServiceTests()
        {
            messageSender = Substitute.For<IMessageSender>();
            var idGenerator = Substitute.For<IIdGenerator>();
            messageReceiver = Substitute.For<IMessageReceiver>();
            idGenerator.GetId().Returns(12);
            service = new ScrumPokerService(messageSender, idGenerator, messageReceiver);
        }

        [Fact]
        public void StartNewSession_ShouldReturnsSessionid()
        {
            var result = service.StartNewSession(TestHelpers.GetTestUser(123));
            service.ScrumPokerSessions.Count.Should().Be(1);
            service.ScrumPokerSessions.First().MasterUser.ChatId.Should().Be(123);
            result.Should().Be(12);
        }

        [Fact]
        public void ConnectWithWrongId_InformsUser()
        {
            service.AddUserToSession(TestHelpers.GetTestUser(1), 1);
            messageSender.Received().NoSessionFound(Arg.Any<PokerUser>(), 1);
        }

        [Fact]
        public void Test1()
        {
            var sessionId = service.StartNewSession(TestHelpers.GetTestUser(2));
            service.AddUserToSession(TestHelpers.GetTestUser(1), sessionId);
            messageSender.Received()
                .InformaAddedUserAndMaster(Arg.Is<PokerUser>(p => p.ChatId == 1), Arg.Is<PokerUser>(p => p.ChatId == 2));
        }

        [Fact]
        public void EndSession_ShouldInformUsers()
        {
            var result = service.StartNewSession(TestHelpers.GetTestUser(123));
            service.EndSession(result);

            service.ScrumPokerSessions.Count.Should().Be(0);
            messageSender.Received().SendEndSession(Arg.Any<PokerUser[]>());
        }

        [Fact]
        public void AddUserToRunningSession()
        {
            var result = service.StartNewSession(TestHelpers.GetTestUser(123));
            service.AddUserToSession(TestHelpers.GetTestUser(2), result);
            service.ScrumPokerSessions.First().AllUsers.Count().Should().Be(2);

            messageSender.Received().InformaAddedUserAndMaster(Arg.Any<PokerUser>(), Arg.Any<PokerUser>());
        }
    }

}