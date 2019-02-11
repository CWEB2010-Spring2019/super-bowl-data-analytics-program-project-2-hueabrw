using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Two
{
    public class Superbowl
    {
        public List<Team> Teams = new List<Team>();
        public string SuperbowlTitle { get; }
        public string Year { get; }
        public int Attendance { get; }
        public string Mvp { get; }
        public string Stadium { get; }
        public string City { get; }
        public string State { get; }

        public Superbowl(string[] data)
        {
            this.Year = data[0];
            this.SuperbowlTitle = data[1];
            this.Attendance = Convert.ToInt32(data[2]);
            Teams.Add(new Team(true, data[3], data[4], data[5], data[6]));
            Teams.Add(new Team(false, data[7], data[8], data[9], data[10]));
            this.Mvp = data[11];
            this.Stadium = data[12];
            this.City = data[13];
            this.State = data[14];
        }
    }
    public class Team
    {
        public string Coach { get; }
        public string Qb { get; }
        public string TeamName { get; }
        public bool IsWinner { get; }
        public int Points { get; }

        public Team(bool isWinner, string qb, string coach, string teamName, string points)
        {
            this.IsWinner = isWinner;
            this.Qb = qb;
            this.Coach = coach;
            this.TeamName = teamName;
            this.Points = Convert.ToInt32(points);
        }
    }
}
