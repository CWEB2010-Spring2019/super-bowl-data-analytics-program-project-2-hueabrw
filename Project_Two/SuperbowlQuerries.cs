using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Two
{
    class SuperbowlQuerries
    {
        IEnumerable<Team> winners;
        public SuperbowlQuerries(List<Superbowl> bowls)
        {
            this.winners =
                from bowl in bowls
                from team in bowl.Teams
                where bowl.Teams[0].IsWinner
                select team;

        }

        public void GenerateTextFile()
        {
            foreach(Team team in this.winners)
            {
                Console.WriteLine(team.TeamName + team.Coach + team.Qb); 
            }
        }
        public void GenerateHTMLFile()
        {
            Console.WriteLine("\nNot available yet");
        }
    }
}
