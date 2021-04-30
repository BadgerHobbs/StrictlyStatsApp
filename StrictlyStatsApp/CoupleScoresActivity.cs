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
        IStrictlyStatsUOW uow = Global.UOW;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CoupleScores);

            int selectedCoupleIndex = (int)Intent.GetIntExtra("SelectedCoupleIndex", 0);

            Couple selectedCouple = uow.Couples.GetAll()[selectedCoupleIndex];

            FindViewById<TextView>(Resource.Id.selectedCoupleTextView).Text = $"Selected Couple: {selectedCouple.ToString()}";

            double totalScore = 0;
            int scoreCount = 0;

            foreach (Score score in uow.Scores.GetAll())
            {
                if (score.CoupleID == selectedCouple.CoupleID)
                {
                    AddTextView($"Week {score.WeekNumber}:\n" +
                        $"Dance - {uow.Dances.GetById(score.DanceID).DanceName}\n" +
                        $"Score - {score.Grade}\n");

                    totalScore += score.Grade;
                    scoreCount++;
                }
            }

            AddTextView($"Total: {totalScore}\n" +
                $"Average: {(totalScore/scoreCount).ToString()}");
        }

        private void AddTextView(string text)
        {
            TextView textView = new TextView(this);

            textView.Text = text;
            textView.TextSize = 20;

            FindViewById<LinearLayout>(Resource.Id.coupleScoresLinearLayout).AddView(textView);
        }
    }
}