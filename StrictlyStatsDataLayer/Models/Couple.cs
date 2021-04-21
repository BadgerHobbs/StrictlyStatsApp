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
    [Serializable]
    [Table("Couples")]
    public class Couple
    {
        public Couple()
        {

        }

        public Couple(int coupleID, string celebrityLastName, string celebrityFirstName, string professionalLastName, string professionalFirstName, int celebrityStarRating, int? votedOffWeekNumber)
        {
            CoupleID = coupleID;
            CelebrityLastName = celebrityLastName;
            CelebrityFirstName = celebrityFirstName;
            ProfessionalLastName = professionalLastName;
            ProfessionalFirstName = professionalFirstName;
            CelebrityStarRating = celebrityStarRating;
            VotedOffWeekNumber = votedOffWeekNumber;
        }

        [PrimaryKey, AutoIncrement]
        public int CoupleID { get; set; }
        public string CelebrityLastName { get; set; }
        public string CelebrityFirstName { get; set; }
        public string ProfessionalLastName { get; set; }
        public string ProfessionalFirstName { get; set; }
        public int CelebrityStarRating { get; set; }
        public int? VotedOffWeekNumber { get; set; }

        public override string ToString()
        {
            return CelebrityFirstName + " " + CelebrityLastName + " and " + ProfessionalFirstName + " " + ProfessionalLastName;
        }

        [OneToMany(CascadeOperations = CascadeOperation.None)]
        public List<Score> Scores { get; set; }
    }
}