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
using StrictlyStatsDataLayer.Models;

namespace StrictlyStatsDataLayer
{
    public interface IReposCouples: IRepository<Couple>
    {
        int GetCurrentWeekNumber();

        Couple GetCoupleVotedOffForGivenWeekNumber(int weekNumber);

        List<Couple> GetCouplesStillInCompetition();

        void VoteOff(int coupleID, int weekNumber);
        Couple GetWinner();

    }
}