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
using StrictlyStatsDataLayer.Models;

namespace StrictlyStatsDataLayer
{
    public class ReposScores : Repository<Score>, IReposScores
    {
        public ReposScores(SQLiteConnection con) : base(con)
        {

        }

        public List<Score> GetScoresRankedForWeek(int weekNumber)
        {
            List<Score> scores = Get(s => s.WeekNumber == weekNumber, s => s.Grade).ToList();
            scores.Reverse();
            foreach (Score score in scores)
            {
                score.Couple = con.Table<Couple>().Single(c => c.CoupleID == score.CoupleID);
            }

            return scores;
        }

        public void DeleteScoresForWeekNumber(int weekNumber)
        {
            con.Table<Score>().Delete(s => s.WeekNumber == weekNumber);
        }
    }
}