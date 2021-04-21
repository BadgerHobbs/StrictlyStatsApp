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
    public interface IReposScores: IRepository<Score>
    {
        List<Score> GetScoresRankedForWeek(int weekNumber);
        void DeleteScoresForWeekNumber(int DanceID);
    }
}