using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace StrictlyStatsDataLayer.Models
{
    [Table("Scores")]
    public class Score:IComparable<Score>
    {
        public Score()
        {

        }

        public Score(int danceID, int coupleID, int weekNumber, int grade)
        {
            DanceID = danceID;
            CoupleID = coupleID;
            WeekNumber = WeekNumber;
            Grade = grade;
        }

        [PrimaryKey, AutoIncrement]
        public int ScoreID { get; set; }
        public int DanceID { get; set; }
        public int CoupleID { get; set; }
        public int WeekNumber { get; set; }
        [Column("Score")]
        public int Grade { get; set; }

        public override string ToString()
        {
            return $"ScoreID: {ScoreID}, WeekNumber: {WeekNumber}, Grade: {Grade}, CoupleID: {CoupleID}, DanceID: {DanceID}";
        }

        public int CompareTo(Score other)
        {
            return this.Grade.CompareTo(other.Grade);
        }

        [ManyToOne]
        public Couple Couple { get; set; }

        [ManyToOne]
        public Dance Dance { get; set; }
    }
}