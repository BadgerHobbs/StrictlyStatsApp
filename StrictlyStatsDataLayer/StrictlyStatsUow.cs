using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using StrictlyStatsDataLayer.Models;

namespace StrictlyStatsDataLayer
{
    public class StrictlyStatsUow : IStrictlyStatsUOW
    {
        private SQLiteConnection Connection { get; set; }

        public StrictlyStatsUow() : this(Global.DBNAME)
        {

        }

        public StrictlyStatsUow(string dbName)
        {
            CreateConnection(dbName);
        }

        private void CreateConnection(string dbName)
        {
            var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPath = Path.Combine(docFolder, dbName);
            Connection = new SQLiteConnection(dbPath);
        }

        public List<string> GetCouplesRankedForWeekNumber(int weekNumber)
        {
            List<Score> scores = Scores.GetScoresRankedForWeek(weekNumber);
            List<string> couplesAndScoresForWeekNumber = GetCouplesAndScores(scores);

            return couplesAndScoresForWeekNumber;
        }

        private List<string> GetCouplesAndScores(List<Score> scores)
        {
            int i = 0;
            return scores.Select((s) =>
                    $"{++i:00}" +  ") " +
                    s.Couple.CelebrityFirstName + " " +
                    s.Couple.CelebrityLastName + " & " +
                    s.Couple.ProfessionalFirstName + " " +
                    s.Couple.ProfessionalLastName + " Score: " +
                    s.Grade + " Week No: " +
                    s.WeekNumber
                ).ToList();
        }

        //repositories
        #region Repositries
        private IReposCouples _couples;
        private IReposDances _dances;
        private IReposScores _scores;
        private IReposInstructions _instructions;

        public IReposCouples Couples {
            get {
                if (_couples == null)
                {
                    _couples = new ReposCouples(Connection);
                }
                return _couples;
            }
        }

        public IReposDances Dances {
            get {
                if (_dances == null)
                {
                    _dances = new ReposDances(Connection);
                }
                return _dances;
            }
        }

        public IReposScores Scores {
            get {
                if (_scores == null)
                {
                    _scores = new ReposScores(Connection);
                }
                return _scores;
            }
        }

        public IReposInstructions Instructions {
            get {
                if (_instructions == null)
                {
                    _instructions = new ReposInstructions(Connection);
                }
                return _instructions;
            }
        }
    }
    #endregion
}