using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Two
{
    public class Superbowl
    {
        public string SuperbowlTitle { get; }
        public string Year { get; }
        public int Attendance { get; }
        public string Mvp { get; }
        public string Stadium { get; }
        public string City { get; }
        public string State { get; }
        public string WinningCoach { get; }
        public string WinningQb { get; }
        public string WinningTeamName { get; }
        public int WinningPoints { get; }
        public string LosingCoach { get; }
        public string LosingQb { get; }
        public string LosingTeamName { get; }
        public int LosingPoints { get; }

        //Constructor receives one line from the csv file as a list of strings.
        //knowing the order its in, it creates an instance of the superbowl class.
        public Superbowl(string[] data)
        {
            //These lines of code handle multiple names (i.e. two mvp winners in one year)
            for(int i = 0; i < data.Length; i++)
            {
                string[] tempStrings = data[i].Split(" ");
                if (tempStrings.Length > 3 && i < 12)
                {
                    data[i] = tempStrings[0] + " " + tempStrings[1] + " & " + tempStrings[2] + " " + tempStrings[3];
                }
            }

            this.Year = data[0];
            this.SuperbowlTitle = data[1];
            this.Attendance = Convert.ToInt32(data[2]);
            this.WinningQb = data[3];
            this.WinningCoach = data[4];
            this.WinningTeamName = data[5];
            this.WinningPoints = Convert.ToInt32(data[6]);
            this.LosingQb = data[7];
            this.LosingCoach = data[8];
            this.LosingTeamName = data[9];
            this.LosingPoints = Convert.ToInt32(data[10]);
            this.Mvp = data[11];
            this.Stadium = data[12];
            this.City = data[13];
            this.State = data[14];
        }
    }
}
