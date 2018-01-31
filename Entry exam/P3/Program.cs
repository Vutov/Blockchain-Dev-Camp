using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = new List<string>();
            var inputLine = Console.ReadLine();
            while (inputLine.ToLower() != "stop")
            {
                lines.Add(inputLine);
                inputLine = Console.ReadLine();
            }
            //{
            //    "Montenegro | Cyprus | 0:0 | 1:1",
            //    "Montenegro | Bosnia | 0:0 | 1:1",
            //    "Montenegro | South Africa | 0:0 | 1:1",
            //    //"Brazil | Germany | 1:1 | 7:0"
            //    //"Denmark | Belgium | 0:0 | 1:1",
            //    //"Belgium | Austria | 2:0 | 0:2",
            //    //"Latvia | Monaco | 2:0 | 0:0",
            //    //"Bulgaria | Italy | 2:1 | 3:2"
            //};

            var allData = new Dictionary<string, Team>();
            foreach (var line in lines)
            {
                var data = line.Split('|').Select(d => d.Trim()).ToList();
                var team1Name = data[0];
                var team2Name = data[1];
                var score1 = data[2].Split(':').Select(i => int.Parse(i.Trim())).ToList();
                var score2 = data[3].Split(':').Select(i => int.Parse(i.Trim())).ToList();
                var team1AwayGoals = score2[1];
                var team1HomeGoals = score1[0];
                var team2AwayGoals = score1[1];
                var team2HomeGoals = score2[0];
                var team1Winner = false;
                if (team1AwayGoals + team1HomeGoals == team2AwayGoals + team2HomeGoals)
                {
                    if (team1AwayGoals > team2AwayGoals)
                    {
                        team1Winner = true;
                    }
                }
                else
                {
                    team1Winner = team1AwayGoals + team1HomeGoals > team2AwayGoals + team2HomeGoals;
                }

                if (allData.ContainsKey(team1Name))
                {
                    var team1 = allData[team1Name];
                    team1.Opponents.Add(team2Name);
                    if (team1Winner == true)
                    {
                        team1.TotalWins++;
                    }
                }
                else
                {
                    var team1 = new Team()
                    {
                        Name = team1Name,
                        Opponents = new HashSet<string>() { team2Name }
                    };

                    if (team1Winner == true)
                    {
                        team1.TotalWins++;
                    }

                    allData.Add(team1Name, team1);
                }

                if (allData.ContainsKey(team2Name))
                {
                    var team2 = allData[team2Name];
                    team2.Opponents.Add(team1Name);

                    if (team1Winner == false)
                    {
                        team2.TotalWins++;
                    }
                }
                else
                {
                    var team2 = new Team()
                    {
                        Name = team2Name,
                        Opponents = new HashSet<string>()
                        {
                            team1Name
                        }
                    };

                    if (team1Winner == false)
                    {
                        team2.TotalWins++;
                    }

                    allData.Add(team2Name, team2);
                }
            }

            var result = allData.OrderByDescending(d => d.Value.TotalWins).ThenBy(d => d.Value.Name);
            foreach (var team in result)
            {
                Console.WriteLine(team.Key);
                Console.WriteLine($"- Wins: {team.Value.TotalWins}");
                var opponenets = team.Value.Opponents.OrderBy(o => o);
                Console.WriteLine($"- Opponents: {string.Join(", ", opponenets)}");
            }
        }
    }

    class Team
    {
        public string Name { get; set; }
        public int TotalWins { get; set; }
        public HashSet<string> Opponents { get; set; }
    }
}
