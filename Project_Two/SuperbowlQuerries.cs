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
        //initializes queries
        IEnumerable<Superbowl> winners;
        IEnumerable<Superbowl> mostAttended;
        IEnumerable<Superbowl> mostHosted;
        IEnumerable<Superbowl> MVPs;
        string mostLosingCoach;
        string mostWinningCoach;
        string mostWins;
        string mostLosses;
        Superbowl biggestDiff;
        int averageAttendace;

        //class construcor
        public SuperbowlQuerries(List<Superbowl> superbowls)
        {
            this.winners = superbowls;
            this.mostAttended = superbowls.OrderByDescending(attend => attend.Attendance).Take(5);
            this.mostHosted = superbowls.GroupBy(superbowl => superbowl.State).OrderByDescending(hostGroup => hostGroup.Count()).SelectMany(host => host);
            this.MVPs = superbowls.GroupBy(superbowl => superbowl.Mvp).OrderByDescending(mvpGroup => mvpGroup.Count()).SelectMany(mvp => mvp);
            this.mostLosingCoach = superbowls.GroupBy(superbowl => superbowl.LosingCoach).OrderByDescending(losingCoach => losingCoach.Count()).SelectMany(team => team).First().LosingCoach;
            this.mostWinningCoach = superbowls.GroupBy(superbowl => superbowl.WinningCoach).OrderByDescending(winningCoach => winningCoach.Count()).SelectMany(team => team).First().WinningCoach;
            this.mostWins = superbowls.GroupBy(superbowl => superbowl.WinningTeamName).OrderByDescending(winngingTeam => winngingTeam.Count()).SelectMany(team => team).First().WinningTeamName;
            this.mostLosses = superbowls.GroupBy(superbowl => superbowl.LosingTeamName).OrderByDescending(losingTeam => losingTeam.Count()).SelectMany(team => team).First().LosingTeamName;
            this.biggestDiff = superbowls.OrderByDescending(superbowl => superbowl.WinningPoints - superbowl.LosingPoints).First();
            this.averageAttendace = superbowls.Select(superbowl => superbowl.Attendance).Sum() / superbowls.Count();
        }
        
        public void GenerateTextFile()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //asks user to name file
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Please name your text file:");
            string fileName = Console.ReadLine() + ".txt";
            if (fileName == ".txt") { fileName = "SuperbowlStats.txt"; } //if user didn't name file, gives default name
            path = Path.Combine(path, fileName);

            //writes to file
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine(new string('=', 120));
                writer.WriteLine("Winners".PadLeft(10));
                writer.WriteLine(new string('=',120));
                writer.WriteLine("Team".PadRight(25) + "Year".PadRight(15) + "Quarterback".PadRight(25) + "Coach".PadRight(25) + "MVP".PadRight(25) + "Won by");
                writer.WriteLine(new string('-', 120));
                foreach (Superbowl bowl in this.winners)
                {
                    writer.WriteLine(bowl.WinningTeamName.PadRight(25) + bowl.Year.PadRight(15) + bowl.WinningQb.PadRight(25) + bowl.WinningCoach.PadRight(25) + bowl.Mvp.PadRight(25) + (bowl.WinningPoints - bowl.LosingPoints) + " pts");
                }
                writer.WriteLine(new string('=', 120));
                writer.WriteLine("Top 5 Attended SuperBowls".PadLeft("Top 5 Attended SuperBowls".Count() + 3));
                writer.WriteLine(new string('=', 120));
                writer.WriteLine("Year".PadRight(15) + "Winning Team".PadRight(25) + "Losing Team".PadRight(25) + "City".PadRight(15) + "State".PadRight(15) + "Stadium".PadRight(15));
                writer.WriteLine(new string('-', 120));
                foreach (Superbowl bowl in this.mostAttended)
                {
                    writer.WriteLine(bowl.Year.PadRight(15) + bowl.WinningTeamName.PadRight(25) + bowl.LosingTeamName.PadRight(25) + bowl.City.PadRight(15) + bowl.State.PadRight(15) + bowl.Stadium.PadRight(15));
                }
            }


            //redirects user to the text file
            System.Diagnostics.Process openTXT = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo(path)
                {
                    UseShellExecute = true
                }
            };

            openTXT.Start();
        }

        

        public void GenerateHTMLFile()
        {
            
            Console.WriteLine();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


            //asks user to name file
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Please name your html file:");
            string fileName = Console.ReadLine() + ".html";
            if(fileName == ".html") { fileName = "SuperbowlStats.html"; }//if user didn't name file, gives default name
            path = Path.Combine(path, fileName);

            //writes to file
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.Write(CreateHTML());
                    
                }
            }

            //redirects user to the html file
            System.Diagnostics.Process openHTML = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo(path)
                {
                    UseShellExecute = true
                }
            };
            
            openHTML.Start();
        }

        private string CreateHTML()
        {
            //creates html format of the supbowl information
            string listofwinners = "";
            foreach(Superbowl bowl in winners)
            {
                listofwinners += "<tr>" + "<td>" + bowl.WinningTeamName + "</td>" + "<td>" + bowl.Year + "</td>" + "<td>" + bowl.WinningQb + "</td>" + "<td>" + bowl.WinningCoach + "</td>" + "<td>" + bowl.Mvp + "</td>" + "<td>" + (bowl.WinningPoints - bowl.LosingPoints) + " pts</td>" + "</tr>";
            }
            string listofattended = "";
            foreach (Superbowl bowl in mostAttended)
            {
                listofattended += "<tr>" + "<td>" +  bowl.Year + "</td>" + "<td>" + bowl.WinningTeamName + "</td>" + "<td>" + bowl.LosingTeamName + "</td>" + "<td>" + bowl.City + "</td>" + "<td>" + bowl.State + "</td>" + "<td>" + bowl.Stadium + "</td>" + "</tr>";
            }
            string listofhosts = "";
            foreach (Superbowl bowl in mostHosted)
            {
                listofhosts += "<tr>" + "<td>" + bowl.State + "</td>" + "<td>" + bowl.WinningTeamName + "</td>" + "<td>" + bowl.LosingTeamName+ "</td>" + "</tr>";
            }
            string listofmvps = "";
            foreach (Superbowl bowl in MVPs)
            {
                listofmvps += "<tr>" + "<td>" + bowl.Mvp + "</td>" + "<td>" + bowl.WinningTeamName + "</td>" + "<td>" + bowl.LosingTeamName + "</td>"+"</tr>";
            }

            //grabs the html template and replaces placeholders with superbowl information
            string theFile = File.ReadAllText(@"../../../template.html");
            theFile = theFile.Replace("listofwinners", listofwinners);
            theFile = theFile.Replace("listofattended", listofattended);
            theFile = theFile.Replace("listofhosts", listofhosts);
            theFile = theFile.Replace("listofmvps", listofmvps);
            theFile = theFile.Replace("mostLosingCoach", mostLosingCoach);
            return theFile;
        }
    }
}