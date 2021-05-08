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
    [Activity(Label = "CoupleAdministrationActivity")]
    public class CoupleAdministrationActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        List<Couple> couples;

        Couple selectedCouple;

        bool isNewCouple = false;
        bool coupleEditTextObjectsEnabled = false;
        Dictionary<String, TextView> coupleEditTextObjects;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CoupleAdministration);

            couples = uow.Couples.GetAll();

            int selectedCoupleIndex = (int)Intent.GetIntExtra("SelectedCoupleIndex", 0);

            coupleEditTextObjects = new Dictionary<String, TextView>() {
                { "coupleIDEditText", FindViewById<TextView>(Resource.Id.coupleIDEditText) },
                { "celebrityFirstNameEditText", FindViewById<TextView>(Resource.Id.celebrityFirstNameEditText) },
                { "celebrityLastNameEditText", FindViewById<TextView>(Resource.Id.celebrityLastNameEditText) },
                { "professionalFirstNameEditText", FindViewById<TextView>(Resource.Id.professionalFirstNameEditText) },
                { "professionalLastNameEditText", FindViewById<TextView>(Resource.Id.professionalLastNameEditText) },
                { "celebrityStarRatingEditText", FindViewById<TextView>(Resource.Id.celebrityStarRatingEditText) },
                { "votedOffWeekNumberEditText", FindViewById<TextView>(Resource.Id.votedOffWeekNumberEditText) }
                };

            if (selectedCoupleIndex == -1)
            {
                selectedCouple = new Couple();
                isNewCouple = true;
            }
            else
            {
                selectedCouple = uow.Couples.GetAll()[selectedCoupleIndex];

                coupleEditTextObjects["coupleIDEditText"].Text = selectedCouple.CoupleID.ToString();
                coupleEditTextObjects["celebrityFirstNameEditText"].Text = selectedCouple.CelebrityFirstName;
                coupleEditTextObjects["celebrityLastNameEditText"].Text = selectedCouple.CelebrityLastName;
                coupleEditTextObjects["professionalFirstNameEditText"].Text = selectedCouple.ProfessionalFirstName;
                coupleEditTextObjects["professionalLastNameEditText"].Text = selectedCouple.ProfessionalLastName;
                coupleEditTextObjects["celebrityStarRatingEditText"].Text = selectedCouple.CelebrityStarRating.ToString();
                coupleEditTextObjects["votedOffWeekNumberEditText"].Text = selectedCouple.VotedOffWeekNumber.ToString();
            }

            FindViewById<TextView>(Resource.Id.editCoupleButton).Click += EditCoupleButton_Click;
            FindViewById<TextView>(Resource.Id.deleteCoupleButton).Click += DeleteCoupleButton_Click;
            FindViewById<TextView>(Resource.Id.saveCoupleChangesButton).Click += SaveCoupleButton_Click;
        }

        private void EditCoupleButton_Click(object sender, System.EventArgs e)
        {
            if (coupleEditTextObjectsEnabled)
            {
                coupleEditTextObjectsEnabled = false;
                SetCoupleEditTextObjectsEnabled(coupleEditTextObjects, coupleEditTextObjectsEnabled);

                FindViewById<TextView>(Resource.Id.editCoupleButton).Text = "Edit";
            }
            else
            {
                coupleEditTextObjectsEnabled = true;
                SetCoupleEditTextObjectsEnabled(coupleEditTextObjects, coupleEditTextObjectsEnabled);

                FindViewById<TextView>(Resource.Id.editCoupleButton).Text = "Finish Editing";
            }
        }

        private void SetCoupleEditTextObjectsEnabled(Dictionary<String, TextView> coupleEditTextObjects, bool isEnabled)
        {
            foreach (KeyValuePair<String, TextView> keyValuePair in coupleEditTextObjects)
            {
                keyValuePair.Value.Enabled = isEnabled;
            }
        }

        private void DeleteCoupleButton_Click(object sender, System.EventArgs e)
        {
            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog alert = dialog.Create();

            alert.SetTitle("Delete Notificaiton");
            alert.SetMessage("Are you sure you want to delete this couple?");
            alert.SetButton("Yes", (c, ev) => {

                if (!isNewCouple)
                {
                    uow.Couples.Delete(selectedCouple);
                }

                Finish();
            });
            alert.SetButton2("No", (c, ev) => {  });
            alert.Show();
        }

        private void SaveCoupleButton_Click(object sender, System.EventArgs e)
        {
            UpdateCoupleValues(selectedCouple, coupleEditTextObjects);

            if (isNewCouple)
            {
                uow.Couples.Insert(selectedCouple);
                isNewCouple = false;
            }
            else
            {
                uow.Couples.Update(selectedCouple);
            }

            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog alert = dialog.Create();

            alert.SetTitle("Save Notificaiton");
            alert.SetMessage("Your changes have been saved!");
            alert.SetButton("OK", (c, ev) => { });
            alert.Show();
        }

        private void UpdateCoupleValues(Couple couple, Dictionary<String, TextView> coupleEditTextObjects)
        {
            int coupleIDEditTextValue = 0;
            Int32.TryParse(coupleEditTextObjects["coupleIDEditText"].Text, out coupleIDEditTextValue);

            int celebrityStarRatingEditTextValue = 0;
            Int32.TryParse(coupleEditTextObjects["celebrityStarRatingEditText"].Text, out celebrityStarRatingEditTextValue);

            int votedOffWeekNumberEditTextValue = 0;
            Int32.TryParse(coupleEditTextObjects["votedOffWeekNumberEditText"].Text, out votedOffWeekNumberEditTextValue);

            couple.CoupleID = coupleIDEditTextValue;
            couple.CelebrityFirstName = coupleEditTextObjects["celebrityFirstNameEditText"].Text;
            couple.CelebrityLastName = coupleEditTextObjects["celebrityLastNameEditText"].Text;
            couple.ProfessionalFirstName = coupleEditTextObjects["professionalFirstNameEditText"].Text;
            couple.ProfessionalLastName = coupleEditTextObjects["professionalLastNameEditText"].Text;
            couple.CelebrityStarRating = celebrityStarRatingEditTextValue;
            couple.VotedOffWeekNumber = votedOffWeekNumberEditTextValue;
        }
    }
}