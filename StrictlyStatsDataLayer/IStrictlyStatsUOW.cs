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
    //Using Unit Of Work Pattern (UOW)
    //UOW acts like a facade that will aggregate all the repositories initiation, calls and disposal in one place
    //it also separates (decouples) the application (Consol, Controllers, ASP.NET Web Page) from the connections and repositories.


    public interface IStrictlyStatsUOW
    {
        IReposCouples Couples { get; }
        IReposDances Dances { get; }
        IReposScores Scores { get; }
        IReposInstructions Instructions { get; }

        List<string> GetCouplesRankedForWeekNumber(int weekNumber);
    }
}