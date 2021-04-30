using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.IO;
using Android.Content;
using StrictlyStatsDataLayer;

namespace StrictlyStats
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button enterScoresButton;
        Button voteOffButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var docFolder = System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);
            //File name to use when copied
            var dbFile = Path.Combine(docFolder, "StrictlyStats.db");
            if (!System.IO.File.Exists(dbFile))
            {
                //Data file resource id
                var s = Resources.OpenRawResource(Resource.Raw.StrictlyStats);
                System.IO.FileStream writeStream =
                    new FileStream(dbFile,
                    FileMode.OpenOrCreate, FileAccess.Write);
                ReadWriteStream(s, writeStream);
            }

            Button instructionsButton = FindViewById<Button>(Resource.Id.instructionsButton);
            instructionsButton.Click += InstructionsButton_Click;

            enterScoresButton = FindViewById<Button>(Resource.Id.enterScoresButton);
            enterScoresButton.Click += EnterScoresButton_Click;

            Button rankingsButton = FindViewById<Button>(Resource.Id.rankingsButton);
            rankingsButton.Click += RankingsButton_Click;

            voteOffButton = FindViewById<Button>(Resource.Id.voteOffButton);
            voteOffButton.Click += VoteOffButton_Click;

            Button coupleScoresButton = FindViewById<Button>(Resource.Id.coupleScoresButton);
            coupleScoresButton.Click += CoupleScoresButton_Click;

            Button couplesAdministrationButton = FindViewById<Button>(Resource.Id.couplesAdministrationButton);
            couplesAdministrationButton.Click += CouplesAdministrationButton_Click;
        }

        private void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            byte[] buffer = new byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            //Write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }

        private void InstructionsButton_Click(object sender, System.EventArgs e)
        {
            Intent instructionIntent = new Intent(this, typeof(Instructions));

            StartActivity(instructionIntent);
        }

        private void EnterScoresButton_Click(object sender, System.EventArgs e)
        {
            Intent enterScoreIntent = new Intent(this, typeof(SelectWeekNumberActivity));
            enterScoreIntent.PutExtra("ActivityType", (int)ActivityType.EnterScores);
            StartActivity(enterScoreIntent);
        }

        private void RankingsButton_Click(object sender, System.EventArgs e)
        {
                Intent rankingsIntent = new Intent(this, typeof(SelectWeekNumberActivity));
                rankingsIntent.PutExtra("ActivityType", (int)ActivityType.Rankings);
                StartActivity(rankingsIntent);
                return;
        }

        private void VoteOffButton_Click(object sender, System.EventArgs e)
        {
            Intent selectCoupleIntent = new Intent(this, typeof(SelectCoupleActivity));
            selectCoupleIntent.PutExtra("ActivityType", (int)ActivityType.VoteOff);
            StartActivity(selectCoupleIntent);
        }

        // Function assigned to click of couples score button
        private void CoupleScoresButton_Click(object sender, System.EventArgs e)
        {
            Intent selectCoupleIntent = new Intent(this, typeof(SelectCoupleActivity));

            selectCoupleIntent.PutExtra("ActivityType", (int)ActivityType.CoupleScores);

            StartActivity(selectCoupleIntent);
        }

        // Function assigned to click of couples administration button
        private void CouplesAdministrationButton_Click(object sender, System.EventArgs e)
        {
            Intent couplesAdministrationIntent = new Intent(this, typeof(CouplesAdministrationActivity));

            couplesAdministrationIntent.PutExtra("ActivityType", (int)ActivityType.CouplesAdministration);

            StartActivity(couplesAdministrationIntent);
        }

        protected override void OnResume()
        {
            base.OnResume();

            IStrictlyStatsUOW uow = Global.UOW;
            int currentWeekNumber = uow.Couples.GetCurrentWeekNumber();
            int numberOfWeeksCompetitionWillRunFor = uow.Couples.GetAll().Count - Global.NUMBEROFCONTESTANTSINFINAL + 1;

            bool scoresHaveBeenEnteredForCurrentWeek = uow.Scores.GetScoresRankedForWeek(currentWeekNumber).Count != 0 ? true : false;
            bool coupleHasBeenVotedOffForCurrentWeek = uow.Couples.GetCoupleVotedOffForGivenWeekNumber(currentWeekNumber) != null ? true : false;

            if ((!scoresHaveBeenEnteredForCurrentWeek) && (currentWeekNumber <= numberOfWeeksCompetitionWillRunFor))
                enterScoresButton.Enabled = true;
            else
                enterScoresButton.Enabled = false;

            if (!coupleHasBeenVotedOffForCurrentWeek && scoresHaveBeenEnteredForCurrentWeek )
                voteOffButton.Enabled = true;
            else
                voteOffButton.Enabled = false;
        }
    }
}