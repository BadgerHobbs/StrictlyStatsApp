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

            // Field to store the couple's total score and number of them (for average calc)
            double totalScore = 0;
            int scoreCount = 0;

            // Add individual dance scores
            
            // Iterate over the all scores
            foreach (Score score in uow.Scores.GetAll())
            {
                // If the score is for this couple
                if (score.CoupleID == selectedCouple.CoupleID)
                {
                    // Add text view with the week number, dance and score data
                    AddTextView($"Week {score.WeekNumber}:\n" +
                        $"Dance - {uow.Dances.GetById(score.DanceID).DanceName}\n" +
                        $"Score - {score.Grade}\n");

                    // Update total score and score count totals
                    totalScore += score.Grade;
                    scoreCount++;
                }
            }

            // Calculate and add average score text
            AddTextView($"Total: {totalScore}\n" +
                $"Average: {(totalScore/scoreCount).ToString()}");
        }

        private void AddTextView(string text)
        {
            // Create new textview object in the context of this activity
            TextView textView = new TextView(this);
            // Set the textview text
            textView.Text = text;
            // Set the textview style#
            textView.TextSize = 20;
            // Add the textview to the existing linear layout in the activity
            FindViewById<LinearLayout>(Resource.Id.coupleScoresLinearLayout).AddView(textView);
        }
    }
}