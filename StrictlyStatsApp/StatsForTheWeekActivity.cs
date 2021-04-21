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
using StrictlyStatsDataLayer;
using StrictlyStatsDataLayer.Models;

namespace StrictlyStats
{
    [Activity(Label = "Statistics for the Week")]
    public class StatsForTheWeekActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StatsForTheWeek);

            Button okButton = FindViewById<Button>(Resource.Id.okButton);
            TextView statsTextView = FindViewById<TextView>(Resource.Id.statsTextView);
            TextView topMarkTextView = FindViewById<TextView>(Resource.Id.topMarkTextView);
            TextView averageMarkTextView = FindViewById<TextView>(Resource.Id.averageMarkTextView);
            TextView bottomMarkTextView = FindViewById<TextView>(Resource.Id.bottomMarkTextView);

            int weekNumber = Intent.GetIntExtra("weekNumber", -1);

            IList<Score> scores = uow.Scores.GetScoresRankedForWeek(weekNumber);
            statsTextView.Text += weekNumber;
            topMarkTextView.Text = scores.First<Score>().Grade.ToString();
            averageMarkTextView.Text = ($"{((Decimal)scores.Sum<Score>(s => s.Grade)/scores.Count):0.00}");
            bottomMarkTextView.Text = scores.Last<Score>().Grade.ToString(); ;

            okButton.Click += OkButton_Click;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}