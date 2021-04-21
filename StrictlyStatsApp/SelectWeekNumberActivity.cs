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

namespace StrictlyStats
{
    [Activity(Label = "Select Week Number")]
    public class SelectWeekNumberActivity : Activity
    {
        Spinner weekNumberSpinner;
        int selectedWeekNumber;
        IStrictlyStatsUOW uow = Global.UOW;
        ActivityType activityType;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectWeekNumber);

            weekNumberSpinner = FindViewById<Spinner>(Resource.Id.weekNumberSpinner);
            Button okButton = FindViewById<Button>(Resource.Id.okButton);
            Button cancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            activityType = (ActivityType)Intent.GetIntExtra("ActivityType", -1);

            //Calc number of weeks competition will run for
            int numberOfWeeksCompetitionWillRunFor = uow.Couples.GetAll().Count - Global.NUMBEROFCONTESTANTSINFINAL + 1;
            //Add code here that creates weekNos array with same number of entries as couples still in competition
            int[] weekNos = new int[numberOfWeeksCompetitionWillRunFor];
            for (int i = 0; i < numberOfWeeksCompetitionWillRunFor; i++) {
                weekNos[i] = i + 1;
            }

            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, weekNos);
            weekNumberSpinner.Adapter = adapter;
            int currentWeekNumber = uow.Couples.GetCurrentWeekNumber() - 1;

            if (currentWeekNumber >= numberOfWeeksCompetitionWillRunFor)
                //competition has ended
                currentWeekNumber = numberOfWeeksCompetitionWillRunFor - 1;
            weekNumberSpinner.SetSelection(currentWeekNumber);

            okButton.Click += OkButton_Click;
            cancelButton.Click += CancelButton_Click;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            int position = weekNumberSpinner.SelectedItemPosition;
            selectedWeekNumber = Convert.ToInt32(weekNumberSpinner.GetItemAtPosition(position));
            if (activityType == ActivityType.EnterScores && uow.Couples.GetCurrentWeekNumber() > selectedWeekNumber)
            {
                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetMessage("OK to overwrite existing scores for " + selectedWeekNumber + "?");
                dlgAlert.SetTitle("Selected Week already has Scores");
                dlgAlert.SetButton("OK", ContinueButton_Click);
                dlgAlert.SetButton2("Cancel", CancelButton_Click);
                dlgAlert.Show();
                return;
            }
            else if (activityType == ActivityType.Rankings)
            {
                Intent rankCouplesIntent = new Intent(this, typeof(RankCouplesActivity));
                rankCouplesIntent.PutExtra("WeekNumber", selectedWeekNumber);
                rankCouplesIntent.PutExtra("ActivityType", (int)activityType);
                StartActivity(rankCouplesIntent);
                if (uow.Couples.GetCurrentWeekNumber() >= selectedWeekNumber)
                    Finish();
                return;
            }
            ContinueButton_Click(sender, e);

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            Intent enterScoreIntent = new Intent(this, typeof(EnterScoresActivity));
            enterScoreIntent.PutExtra("WeekNumber", selectedWeekNumber);
            StartActivity(enterScoreIntent);
            Finish();
        }
    }
}