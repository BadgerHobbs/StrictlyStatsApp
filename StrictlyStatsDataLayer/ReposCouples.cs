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
    public class ReposCouples : Repository<Couple>, IReposCouples
    {
        public ReposCouples(SQLiteConnection con):base(con)
        {

        }

        public List<Couple> GetCouplesStillInCompetition()
        {
            return Get(c => c.VotedOffWeekNumber == null, c => c.CoupleID).ToList<Couple>();
        }

        public Couple GetCoupleVotedOffForGivenWeekNumber(int weekNumber)
        {
            return Get(c => c.VotedOffWeekNumber == weekNumber);
        }

        public int GetCurrentWeekNumber()
        {
            return (int)(con.Table<Couple>().OrderByDescending(c => c.VotedOffWeekNumber).First().VotedOffWeekNumber) + 1;
        }

        public void VoteOff(int coupleID, int weekNumber)
        {
            //Check to ensure no one else has been voted off for specified week
            Couple couple = Get(c => c.VotedOffWeekNumber == weekNumber);
            if (couple == null)
            {
                couple = this.GetById(coupleID);
                couple.VotedOffWeekNumber = weekNumber;
                this.Update(couple);
            }
            else
                throw new Exception("A couple has already been voted off for the specified week");
        }

        public Couple GetWinner(){
            List<Couple> couples = Get(c => c.VotedOffWeekNumber == null, c => c.CoupleID);
            if (couples.Count == 1)
                return couples[0];
            else
                throw new Exception("Still more than one couple in competition so GetWinner method should not have been invoked");
        }
    }
}