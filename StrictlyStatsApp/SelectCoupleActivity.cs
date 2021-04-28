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
    [Activity(Label = "Select Couple")]
    public class SelectCoupleActivity : ListActivity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        List<Couple> couples;
        ActivityType activityType;
        int position = -1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            activityType = (ActivityType)Intent.GetIntExtra("ActivityType", -1);

            couples = uow.Couples.GetCouplesStillInCompetition();

            this.ListAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, couples.ToArray<Couple>());

            // If this activity was opened by the vote off button in the main activity
            if ((int)activityType == (int)ActivityType.VoteOff)
            {
                // Assign vote off method to listview item click
                this.ListView.ItemClick += ListView_ItemClick_VoteOff;
            }
            // If this activity was opened by the couple scores button in the main activity
            else if ((int)activityType == (int)ActivityType.CoupleScores)
            {
                // Assign select couple method to listview item click
                this.ListView.ItemClick += ListView_ItemClick_SelectCouple;
            }
        }

        private void ListView_ItemClick_VoteOff(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent;
            position = e.Position;
            var dlgAlert = (new AlertDialog.Builder(this)).Create();
            dlgAlert.SetMessage("OK to vote off " + couples[e.Position] + "?");
            dlgAlert.SetTitle("Vote Off");
            dlgAlert.SetButton("OK", ContinueButton_Click);
            dlgAlert.SetButton2("Cancel", CancelButton_Click);
            dlgAlert.Show();
            return;
        }

        // Method to add to list view item for selecting a couple
        private void ListView_ItemClick_SelectCouple(object sender, AdapterView.ItemClickEventArgs e)
        {
            // Create intent for opening the couple scores activity
            Intent coupleScoresActivity = new Intent(this, typeof(CoupleScoresActivity));
            // Add extra data to the intent to include the selected couple
            coupleScoresActivity.PutExtra("SelectedCoupleIndex", e.Position);
            // Start the couple scores activity
            StartActivity(coupleScoresActivity);
        }


        private void CancelButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            int weekNumber = uow.Couples.GetCurrentWeekNumber();
            try
            {
                uow.Couples.VoteOff(couples[position].CoupleID, weekNumber);
                //Calc number of weeks competition will run for
                int numberOfWeeksCompetitionWillRunFor = uow.Couples.GetAll().Count - Global.NUMBEROFCONTESTANTSINFINAL + 1;
                //Check for end of competiton
                if(numberOfWeeksCompetitionWillRunFor == weekNumber)
                {
                    //We have a winner
                    Couple couple = uow.Couples.GetWinner();
                    Intent intent = new Intent(this, typeof(WinnerActivity));
                    intent.PutExtra("WinningCoupleID", couple.CoupleID);
                    StartActivity(intent);
                    Finish();
                }
            }
            catch(Exception exn)
            {
                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetMessage(exn.Message);
                dlgAlert.SetTitle("Vote Off Error");
                dlgAlert.SetButton("OK", (s, ev) => { return; });
                dlgAlert.Show();
                return;
            }
            Finish();
        }
    }
}