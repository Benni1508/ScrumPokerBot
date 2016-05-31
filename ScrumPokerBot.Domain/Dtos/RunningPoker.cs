using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrumPokerBot.Domain.Dtos
{
    public class RunningPoker
    {
        public RunningPoker(ScrumPokerSession session)
        {
            Users = session.AllUsers.Select(u => new UserEstimation(u)).ToList();
         }

        
        public List<UserEstimation> Users { get;}

        public override string ToString()
        {
            var groupedEstimations = this.Users.GroupBy(u => u.Estimation).OrderBy(e => e.Key);
            var results = groupedEstimations.Where(e => e.Key >= 0).ToDictionary(k => k.Key, v => v.Count());
            var sb = new StringBuilder();
            foreach (var result in results)
            {
                sb.AppendLine($"{result.Value} x {result.Key} Story points");
            }

            if (results.Count > 1)
            {
                sb.AppendLine(AppendHighestAndLowest(results));
            }

            var canceldUsers = GetCancelUsers();
            if (!string.IsNullOrEmpty(canceldUsers))
            {
                sb.AppendLine("Benutzer ohne Schätzung");
                sb.AppendLine(this.GetCancelUsers());
            }

            return sb.ToString();
        }

        private string GetCancelUsers()
        {
            var sb = new StringBuilder();
            foreach (var user in Users.Where(u => u.Estimation == -1))
            {
                sb.AppendLine(user.User.ToString());
            }

            return sb.ToString();
        }

        private string AppendHighestAndLowest(Dictionary<int, int> results)
        {
            var sb = new StringBuilder();
            var lowestEstimation = results.First().Key;
            var highestEstimation = results.Last().Key;

            var highest = this.Users.Where(u => u.Estimation == highestEstimation).OrderBy(e => Guid.NewGuid()).First();
            var lowest = this.Users.Where(u => u.Estimation == lowestEstimation).OrderBy(e => Guid.NewGuid()).First();

            sb.AppendLine();
            sb.AppendLine($"Niedrigste Schätzung {lowest.Estimation} von {lowest.User}");
            sb.AppendLine($"Höchste Schätzung {highest.Estimation} von {highest.User}");

            return sb.ToString();
        }

        public void Complete()
        {
            foreach (var userEstimation in Users.Where(u => !u.EstimationReceived))
            {
                userEstimation.SetEstimation(-1);
            }
        }
    }
}