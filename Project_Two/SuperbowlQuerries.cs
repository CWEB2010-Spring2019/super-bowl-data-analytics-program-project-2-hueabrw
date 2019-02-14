using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Project_Two
{
    class SuperbowlQuerries
    {
        IEnumerable<Superbowl> winners;
        IEnumerable<Superbowl> mostAttended;
        IEnumerable<Superbowl> mostHosted;
        IEnumerable<Superbowl> MVPs;
        public SuperbowlQuerries(List<Superbowl> bowls)
        {
            this.winners =
                from bowl in bowls
                from team in bowl.Teams
                where team.IsWinner
                select bowl;
            this.mostAttended = bowls.OrderByDescending(at => at.Attendance).Take(5);
            this.mostHosted = bowls.OrderByDescending(host => host.State);
            
        }

        public void GenerateTextFile()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Please name your text file:");
            string fileName = Console.ReadLine() + ".txt";
            if (fileName == ".txt") { fileName = "SuperbowlStats.txt"; }
            path = Path.Combine(path, fileName);
            using (StreamWriter writer = File.CreateText(path)) {
                foreach (Superbowl bowl in this.winners)
                {
                    writer.WriteLine(bowl.Year.PadRight(20) + bowl.SuperbowlTitle.PadRight(20) + bowl.Teams[0].TeamName.PadRight(20) + bowl.Teams[1].TeamName.PadRight(20));
                }
            }
            System.Diagnostics.Process openTXT = new System.Diagnostics.Process();
            openTXT.StartInfo = new System.Diagnostics.ProcessStartInfo(path)
            {
                UseShellExecute = true
            };

            openTXT.Start();
        }
        public void GenerateHTMLFile()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Please name your html file:");
            string fileName = Console.ReadLine() + ".html";
            if(fileName == ".html") { fileName = "SuperbowlStats.html"; }
            path = Path.Combine(path, fileName);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.Write(createHTML());
                    
                }
            }

            System.Diagnostics.Process openHTML = new System.Diagnostics.Process();
            openHTML.StartInfo = new System.Diagnostics.ProcessStartInfo(path)
            {
                UseShellExecute = true
            };

            openHTML.Start();
        }

        private string createHTML()
        {
            
            string listofwinners = "";
            foreach(Superbowl bowl in winners)
            {
                listofwinners += "<p>" + bowl.Year.PadRight(20) + bowl.SuperbowlTitle.PadRight(20) + bowl.Teams[0].TeamName.PadRight(20) + bowl.Teams[1].TeamName.PadRight(20) + "</p>";
            }
            string listofattended = "";
            foreach (Superbowl bowl in mostAttended)
            {
                listofattended += "<p>" + bowl.Year.PadRight(20) + bowl.Teams[0].TeamName.PadRight(20) + bowl.Teams[1].TeamName.PadRight(20) + bowl.City.PadRight(20) + bowl.State.PadRight(20) + bowl.Stadium.PadRight(20) + "</p>";
            }
            string theFile = File.ReadAllText(@"../../../template.html");
            theFile = theFile.Replace("<p>listofwinners</p>", listofwinners);
            theFile = theFile.Replace("<p>listofattended</p>", listofattended);
            return theFile;
        }
    }
}