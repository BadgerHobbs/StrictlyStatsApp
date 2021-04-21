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
using Android;
using StrictlyStatsDataLayer.Models;

namespace StrictlyStats
{
    [Activity(Label = "Enter Scores")]
    public class EnterScoresActivity : Activity
    {
        EditText coupleScoreEditText;
        TextView promptTextView;
        TextView weekNumberTextView;
        Spinner dancesSpinner;
        Button okButton;

        IStrictlyStatsUOW uow = Global.UOW;

        int index = -1;
        IList<Couple> couples;
        List<Dance> dances;
        bool firstTime = true;
        Couple couple;
        int weekNumber;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EnterScores);

            coupleScoreEditText = FindViewById<EditText>(Resource.Id.coupleScoreEditText);
            promptTextView = FindViewById<TextView>(Resource.Id.promptTextView);
            weekNumberTextView = FindViewById<TextView>(Resource.Id.weekNumberTextView);
            dancesSpinner = FindViewById<Spinner>(Resource.Id.dancesSpinner);
            okButton = FindViewById<Button>(Resource.Id.okButton);
            Button cancelButton = FindViewById<Button>(Resource.Id.cancelButton);

            weekNumber = Intent.GetIntExtra("WeekNumber", -1); 
            if (index == -1)
            {
                couples = uow.Couples.GetCouplesStillInCompetition();
                couple = couples[0];
                index++;
            }

            weekNumberTextView.Text = weekNumber.ToString();
            dances = uow.Dances.GetAll();
            dancesSpinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, dances);
            dancesSpinner.SetSelection(0);
            promptTextView.Text = $"Please enter the score for {couple.CelebrityFirstName} {couple.CelebrityLastName} &  {couple.ProfessionalFirstName} {couple.ProfessionalLastName}";

            okButton.Click += OKButton_Click;
            cancelButton.Click += CancelButton_Click;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {

            var dlgAlert = (new AlertDialog.Builder(this)).Create();
            dlgAlert.SetMessage($"WARNING: Cancelling now will undo any marks added for week number {weekNumber}. Are you sure you want to proceed?");
            dlgAlert.SetTitle("Cancel");
            dlgAlert.SetButton("Yes", (c, ev) =>
            {
                uow.Scores.DeleteScoresForWeekNumber(weekNumber);
                Finish();
            });
            dlgAlert.SetButton2("No", (c, ev) =>
            {
                return;
            });
            dlgAlert.Show();
            return;
        }

        private void GetNextCoupleAndUpdateUI()
        {
            index++;
            if (index == couples.Count)
            {
                //No more couples so show stats page
                Intent statsForTheWeek = new Intent(this, typeof(StatsForTheWeekActivity));
                statsForTheWeek.PutExtra("weekNumber", weekNumber);
                StartActivity(statsForTheWeek);
                Finish();
                return;
            }
            couple = couples[index];
            coupleScoreEditText.Text = string.Empty;
            dancesSpinner.SetSelection(0);
            promptTextView.Text = $"Please enter the score for {couple.CelebrityFirstName} {couple.CelebrityLastName} &  {couple.ProfessionalFirstName} {couple.ProfessionalLastName}";
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (firstTime)
            {
                uow.Scores.DeleteScoresForWeekNumber(weekNumber);
                firstTime = false;
            }

            if (string.IsNullOrEmpty(coupleScoreEditText.Text.Trim()) || Decimal.Parse(coupleScoreEditText.Text) > 40 || Decimal.Parse(coupleScoreEditText.Text) < 0)
            {
                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetMessage("Grade must lie between 0 and 40");
                dlgAlert.SetTitle("Invalid Grade");
                dlgAlert.SetButton("OK", (c, ev) =>
                {
                    // Do nothing 
                });
                dlgAlert.Show();
                return;
            }

            Score score = new Score {
                DanceID= dances[dancesSpinner.SelectedItemPosition].DanceID,
                CoupleID=couples[index].CoupleID,
                WeekNumber=weekNumber,
                Grade= int.Parse(coupleScoreEditText.Text),
                Couple = couple,
                Dance = dances[dancesSpinner.SelectedItemPosition]
            };

            uow.Scores.Insert(score);
            GetNextCoupleAndUpdateUI();
        }
    }
}