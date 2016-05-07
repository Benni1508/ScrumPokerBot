using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ScrumPokerBot.Domain.Interfaces;
using Xunit;

namespace ScrumPokerBot.Domain.Tests
{
    public class MultiThread
    {
        private IMessageSender messageSender;
        private ScrumPokerService service;

        public MultiThread()
        {
        Reset();
        }

        private void Reset()
        {
            var q = new Queue<int>(new[] { 1, 2, 3, 4, 5, 6 });
            messageSender = Substitute.For<IMessageSender>();
            var idGenerator = Substitute.For<IIdGenerator>();
            idGenerator.GetId().Returns((i) => q.Dequeue());
            service = new ScrumPokerService(messageSender, idGenerator);
        }

        [Fact]
        public async Task MM()
        {
            for (int i = 0; i < 100; i++)
            {
                await Test();
                Reset();
            }
        }

        [Fact]
        public async Task Test()
        {
            await Task.Run(() => Task1());
            var l = new List<Task>();
            l.Add(Task.Run(() => TaksAddUsers()));
            l.Add(Task.Run(() => TaksAddUsers2()));
            l.Add(Task.Run(() => TaksAddUsers3()));

            Task.WaitAll(l.ToArray());
            this.service.ScrumPokerSessions.Count().Should().Be(3);
            this.service.ScrumPokerSessions.Single(s => s.Id == 1).AllUsers.Count().Should().Be(7);
            this.service.ScrumPokerSessions.Single(s => s.Id == 2).AllUsers.Count().Should().Be(7);
            this.service.ScrumPokerSessions.Single(s => s.Id == 3).AllUsers.Count().Should().Be(7);
        }

        private void Task1()
        {
            this.service.StartNewSession(TestHelpers.GetTestUser(10));
            this.service.StartNewSession(TestHelpers.GetTestUser(20));
            this.service.StartNewSession(TestHelpers.GetTestUser(30));
        }

        private void TaksAddUsers()
        {
            this.service.ConnectToSession(TestHelpers.GetTestUser(11), 1);
            this.service.ConnectToSession(TestHelpers.GetTestUser(12), 1);
            this.service.ConnectToSession(TestHelpers.GetTestUser(21), 2);
            this.service.ConnectToSession(TestHelpers.GetTestUser(22), 2);
            this.service.ConnectToSession(TestHelpers.GetTestUser(31), 3);
            this.service.ConnectToSession(TestHelpers.GetTestUser(32), 3);
        }
        private void TaksAddUsers2()
        {
            this.service.ConnectToSession(TestHelpers.GetTestUser(23), 2);
            this.service.ConnectToSession(TestHelpers.GetTestUser(24), 2);
            this.service.ConnectToSession(TestHelpers.GetTestUser(13), 1);
            this.service.ConnectToSession(TestHelpers.GetTestUser(14), 1);
            this.service.ConnectToSession(TestHelpers.GetTestUser(33), 3);
            this.service.ConnectToSession(TestHelpers.GetTestUser(34), 3);
        }
        private void TaksAddUsers3()
        {
            this.service.ConnectToSession(TestHelpers.GetTestUser(35), 3);
            this.service.ConnectToSession(TestHelpers.GetTestUser(36), 3);
            this.service.ConnectToSession(TestHelpers.GetTestUser(15), 1);
            this.service.ConnectToSession(TestHelpers.GetTestUser(16), 1);
            this.service.ConnectToSession(TestHelpers.GetTestUser(25), 2);
            this.service.ConnectToSession(TestHelpers.GetTestUser(26), 2);
        }
        
    }
}