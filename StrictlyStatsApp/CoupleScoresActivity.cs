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
    [Activity(Label = "CoupleScoresActivity")]
    public class CoupleScoresActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW; // unit of work pattern for database data

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the couple scores layout resource
            SetContentView(Resource.Layout.CoupleScores);

            // Get selected couple index from the selection activity intent
            int selectedCoupleIndex = (int)Intent.GetIntExtra("SelectedCoupleIndex", 0);

            // Set selected couple based on value from the select activity fro the availible couples
            Couple selectedCouple = uow.Couples.GetCouplesStillInCompetition()[selectedCoupleIndex];

            // Update Selected Couple Text
            FindViewById<TextView>(Resource.Id.selectedCoupleTextView).Text = $"Selected Couple: {selectedCouple.ToString()}";
        }
    }
}