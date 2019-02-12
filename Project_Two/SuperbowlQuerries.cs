using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Project_Two
{
    class SuperbowlQuerries
    {
        IEnumerable<Superbowl> winners;
        IEnumerable<Superbowl> mostAttended;
        public SuperbowlQuerries(List<Superbowl> bowls)
        {
            this.winners =
                from bowl in bowls
                from team in bowl.Teams
                where team.IsWinner
                select bowl;
            this.mostAttended =
                from bowl in bowls
                where bowl.Attendance > bowls[bowls.IndexOf(bowl) - 1].Attendance
                select bowl;

        }

        public void GenerateTextFile()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = "Superbowl";
            using (StreamWriter writer = File.CreateText(path + "\\" + fileName + ".txt")) {
                foreach (Superbowl bowl in this.winners)
                {
                    writer.WriteLine(bowl.Year.PadRight(20) + bowl.SuperbowlTitle.PadRight(20) + bowl.Teams[0].TeamName.PadRight(20) + bowl.Teams[1].TeamName.PadRight(20));
                }
            }
        }
        public void GenerateHTMLFile()
        {
            Console.WriteLine("\nNot available yet");
        }
    }
}
