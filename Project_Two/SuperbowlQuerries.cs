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
        int averageAttendence;

        //class construcor
        public SuperbowlQuerries(List<Superbowl> superbowls)
        {
            //tabled info
            this.winners = superbowls;
            this.mostAttended = superbowls.OrderByDescending(attend => attend.Attendance).Take(5);
            this.mostHosted = superbowls.GroupBy(superbowl => superbowl.State).OrderByDescending(hostGroup => hostGroup.Count()).SelectMany(host => host);
            this.MVPs = superbowls.GroupBy(superbowl => superbowl.Mvp).OrderByDescending(mvpGroup => mvpGroup.Count()).SelectMany(mvp => mvp);
            

            //other info
            this.mostLosingCoach = superbowls.GroupBy(superbowl => superbowl.LosingCoach).OrderByDescending(losingCoach => losingCoach.Count()).SelectMany(team => team).First().LosingCoach;
            this.mostWinningCoach = superbowls.GroupBy(superbowl => superbowl.WinningCoach).OrderByDescending(winningCoach => winningCoach.Count()).SelectMany(team => team).First().WinningCoach;
            this.mostWins = superbowls.GroupBy(superbowl => superbowl.WinningTeamName).OrderByDescending(winngingTeam => winngingTeam.Count()).SelectMany(team => team).First().WinningTeamName;
            this.mostLosses = superbowls.GroupBy(superbowl => superbowl.LosingTeamName).OrderByDescending(losingTeam => losingTeam.Count()).SelectMany(team => team).First().LosingTeamName;
            this.biggestDiff = superbowls.OrderByDescending(superbowl => superbowl.WinningPoints - superbowl.LosingPoints).First();
            this.averageAttendence = superbowls.Select(superbowl => superbowl.Attendance).Sum() / superbowls.Count();
        }
        
        public void GenerateTextFile()
        {
             

            //asks user to name file
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Please enter the path you would like to put your file (desktop by default): ");
            Console.Write(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\" + Console.ReadLine();
            if(path == Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\")
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Please name your text file:");
            string fileName = Console.ReadLine() + ".txt";
            if (fileName == ".txt") { fileName = "SuperbowlStats.txt"; } //if user didn't name file, gives default name
            path = Path.Combine(path, fileName);

            //writes to file
            try
            {
                WriteTextFile(path);

                //redirects user to the text file
                System.Diagnostics.Process openTXT = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo(path)
                    {
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized,
                        UseShellExecute = true
                    }
                };

                openTXT.Start();
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("The path you entered does not exist");
                Console.WriteLine("\nPress any key to exit");
                Console.ReadKey();
            }
            

            /*
            void CreateTitle(string title,params string[] subtitles)
            {
                writer.WriteLine(new string('=', 189));
                writer.WriteLine(title.PadLeft(90 + "Most Common MVPs".Length / 2));
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Most Valuable Player".PadRight(15) + "Winning Team".PadRight(25) + "Losing Team".PadRight(25));
                writer.WriteLine(new string('-', 189));
            }*/
        }

        

        public void GenerateHTMLFile()
        {
            
            Console.WriteLine();
            
            //asks user to name file
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Please enter the path you would like to put your file (desktop by default): ");
            Console.Write(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\" + Console.ReadLine();
            if (path == Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\")
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Please name your html file:");
            string fileName = Console.ReadLine() + ".html";
            if(fileName == ".html") { fileName = "SuperbowlStats.html"; }//if user didn't name file, gives default name
            path = Path.Combine(path, fileName);

            //writes to file
            try
            {
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
            catch
            {
                Console.Clear();
                Console.WriteLine("The path you entered does not exist");
                Console.WriteLine("\nPress any key to exit");
                Console.ReadKey();
            }
        }

        private void WriteTextFile(string path)
        {
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Winners".PadLeft(90 + "Winners".Length / 2));
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Team".PadRight(25) + "Year".PadRight(15) + "Quarterback".PadRight(30) + "Coach".PadRight(25) + "MVP".PadRight(30) + "Won by");
                writer.WriteLine(new string('-', 189));
                foreach (Superbowl bowl in this.winners)
                {
                    writer.WriteLine(bowl.WinningTeamName.PadRight(25) + bowl.Year.PadRight(15) + bowl.WinningQb.PadRight(30) + bowl.WinningCoach.PadRight(25) + bowl.Mvp.PadRight(30) + (bowl.WinningPoints - bowl.LosingPoints) + " pts");
                }
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Top 5 Attended SuperBowls".PadLeft(90 + "Top 5 Attended SuperBowls".Length / 2));
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Year".PadRight(15) + "Winning Team".PadRight(25) + "Losing Team".PadRight(25) + "City".PadRight(15) + "State".PadRight(15) + "Stadium".PadRight(15));
                writer.WriteLine(new string('-', 189));
                foreach (Superbowl bowl in this.mostAttended)
                {
                    writer.WriteLine(bowl.Year.PadRight(15) + bowl.WinningTeamName.PadRight(25) + bowl.LosingTeamName.PadRight(25) + bowl.City.PadRight(15) + bowl.State.PadRight(15) + bowl.Stadium.PadRight(15));
                }
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("States that Hosted the most".PadLeft(90 + "States that Hosted the most".Length / 2));
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("City".PadRight(25) + "State".PadRight(25) + "Stadium".PadRight(25));
                writer.WriteLine(new string('-', 189));
                foreach (Superbowl bowl in this.mostHosted)
                {
                    writer.WriteLine(bowl.City.PadRight(25) + bowl.State.PadRight(25) + bowl.Stadium.PadRight(15));
                }
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Most Common MVPs".PadLeft(90 + "Most Common MVPs".Length / 2));
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Most Valuable Player".PadRight(30) + "Winning Team".PadRight(25) + "Losing Team".PadRight(25));
                writer.WriteLine(new string('-', 189));
                foreach (Superbowl bowl in this.MVPs)
                {
                    writer.WriteLine(bowl.Mvp.PadRight(30) + bowl.WinningTeamName.PadRight(25) + bowl.LosingTeamName.PadRight(25));
                }
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Other Statistics".PadLeft(90 + "Other Statistics".Length / 2));
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Coach with the most Losses:");
                writer.WriteLine(new string('-', 189));
                writer.WriteLine(mostLosingCoach);
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Coach with the most wins:");
                writer.WriteLine(new string('-', 189));
                writer.WriteLine(mostWinningCoach);
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Team with the most Losses:");
                writer.WriteLine(new string('-', 189));
                writer.WriteLine(mostLosses);
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Team with the most wins:");
                writer.WriteLine(new string('-', 189));
                writer.WriteLine(mostWins);
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Greatest point difference in a Superbowl:");
                writer.WriteLine(new string('-', 189));
                writer.WriteLine("Superbowl " + biggestDiff.SuperbowlTitle + " with a " + (biggestDiff.WinningPoints - biggestDiff.LosingPoints) + " point difference");
                writer.WriteLine(new string('=', 189));
                writer.WriteLine("Average Attendence of Superbowls:");
                writer.WriteLine(new string('-', 189));
                writer.WriteLine(averageAttendence);
            }
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
                listofhosts += "<tr>" + "<td>" + bowl.City + "</td>" + "<td>" + bowl.State + "</td>" + "<td>" + bowl.Stadium+ "</td>" + "</tr>";
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
            theFile = theFile.Replace("mostWinningCoach", mostWinningCoach);
            theFile = theFile.Replace("mostLosses", mostLosses);
            theFile = theFile.Replace("mostWins", mostWins);
            theFile = theFile.Replace("biggestDiff", "Superbowl " + biggestDiff.SuperbowlTitle + " with a " + (biggestDiff.WinningPoints - biggestDiff.LosingPoints).ToString() + " point difference");
            theFile = theFile.Replace("averageAttendence", averageAttendence.ToString());
            return theFile;
        }
    }
}