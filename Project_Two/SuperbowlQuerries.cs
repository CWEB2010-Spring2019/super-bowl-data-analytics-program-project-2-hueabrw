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
            
        //class construcor
        public SuperbowlQuerries(List<Superbowl> superbowls)
        {
            this.winners = superbowls.OrderBy(title => title.SuperbowlTitle);
            this.mostAttended = superbowls.OrderByDescending(attend => attend.Attendance).Take(5);
            this.mostHosted = superbowls.GroupBy(superbowl => superbowl.State).OrderByDescending(hostGroup => hostGroup.Count()).SelectMany(host => host);
            this.MVPs = superbowls.GroupBy(superbowl => superbowl.Mvp).OrderByDescending(mvpGroup => mvpGroup.Count()).SelectMany(mvp => mvp);
            
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
                writer.WriteLine(new string('=', 100));
                writer.WriteLine("Winners".PadLeft(10));
                writer.WriteLine(new string('=',100));
                writer.WriteLine("Year".PadRight(15) + "Title".PadRight(15) + "Winning Team".PadRight(25) + "Losing Team".PadRight(25));
                writer.WriteLine(new string('-', 100));
                foreach (Superbowl bowl in this.winners)
                {
                    writer.WriteLine(bowl.Year.PadRight(15) + bowl.SuperbowlTitle.PadRight(15) + bowl.Teams[0].TeamName.PadRight(25) + bowl.Teams[1].TeamName.PadRight(25));
                }
                writer.WriteLine(new string('=', 100));
                writer.WriteLine("Top 5 Attended SuperBowls".PadLeft(10));
                writer.WriteLine(new string('=', 100));
                writer.WriteLine("Year".PadRight(15) + "Winning Team".PadRight(25) + "Losing Team".PadRight(25) + "City".PadRight(15) + "State".PadRight(15) + "Stadium".PadRight(15));
                writer.WriteLine(new string('-', 100));
                foreach (Superbowl bowl in this.mostAttended)
                {
                    writer.WriteLine(bowl.Year.PadRight(15) + bowl.Teams[0].TeamName.PadRight(25) + bowl.Teams[1].TeamName.PadRight(25) + bowl.City.PadRight(15) + bowl.State.PadRight(15) + bowl.Stadium.PadRight(15));
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
                listofwinners += "<p>" + bowl.Year + ", " + bowl.SuperbowlTitle + ", " + bowl.Teams[0].TeamName + ", " + bowl.Teams[1].TeamName + "</p>";
            }
            string listofattended = "";
            foreach (Superbowl bowl in mostAttended)
            {
                listofattended += "<p>" + bowl.Year + ", " + bowl.Teams[0].TeamName + ", " + bowl.Teams[1].TeamName + ", " + bowl.City + ", " + bowl.State + ", " + bowl.Stadium + "</p>";
            }
            string listofhosts = "";
            foreach (Superbowl bowl in mostHosted)
            {
                listofhosts += "<p>" + bowl.State + ", " + bowl.Teams[0].TeamName + ", " + bowl.Teams[1].TeamName + "</p>";
            }
            string listofmvps = "";
            foreach (Superbowl bowl in MVPs)
            {
                listofmvps += "<p>" +bowl.Mvp + ", " + bowl.Teams[0].TeamName + ", " + bowl.Teams[1].TeamName + "</p> ";
            }

            //grabs the html template and replaces placeholders with superbowl information
            string theFile = File.ReadAllText(@"../../../template.html");
            theFile = theFile.Replace("<p>listofwinners</p>", listofwinners);
            theFile = theFile.Replace("<p>listofattended</p>", listofattended);
            theFile = theFile.Replace("<p>listofhosts</p>", listofhosts);
            theFile = theFile.Replace("<p>listofmvps</p>", listofmvps);
            return theFile;
        }
    }
}