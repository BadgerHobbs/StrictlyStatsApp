using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        Dictionary<String, CoupleAdministrationTextObject> coupleEditTextObjects;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CoupleAdministration);
             
            couples = uow.Couples.GetAll();

            int selectedCoupleIndex = (int)Intent.GetIntExtra("SelectedCoupleIndex", 0);

            coupleEditTextObjects = new Dictionary<String, CoupleAdministrationTextObject>() {
                { "coupleIDEditText", new CoupleAdministrationTextObject("Couple ID", FindViewById<TextView>(Resource.Id.coupleIDEditText), typeof(int), false) },
                { "celebrityFirstNameEditText", new CoupleAdministrationTextObject("Celebrity First Name", FindViewById<TextView>(Resource.Id.celebrityFirstNameEditText), typeof(string), false) },
                { "celebrityLastNameEditText", new CoupleAdministrationTextObject("Celebrity Last Name", FindViewById<TextView>(Resource.Id.celebrityLastNameEditText), typeof(string), false) },
                { "professionalFirstNameEditText", new CoupleAdministrationTextObject("Professional First Name", FindViewById<TextView>(Resource.Id.professionalFirstNameEditText), typeof(string), false) },
                { "professionalLastNameEditText", new CoupleAdministrationTextObject("Professional Last Name", FindViewById<TextView>(Resource.Id.professionalLastNameEditText), typeof(string), false) },
                { "celebrityStarRatingEditText", new CoupleAdministrationTextObject("Celebrity Star Rating", FindViewById<TextView>(Resource.Id.celebrityStarRatingEditText), typeof(int), false) },
                { "votedOffWeekNumberEditText", new CoupleAdministrationTextObject("Voted Off Week Number", FindViewById<TextView>(Resource.Id.votedOffWeekNumberEditText), typeof(int), true) }
                };

            if (selectedCoupleIndex == -1)
            {
                selectedCouple = new Couple();
                isNewCouple = true;
            }
            else
            {
                selectedCouple = uow.Couples.GetAll()[selectedCoupleIndex];

                coupleEditTextObjects["coupleIDEditText"].textView.Text = selectedCouple.CoupleID.ToString();
                coupleEditTextObjects["celebrityFirstNameEditText"].textView.Text = selectedCouple.CelebrityFirstName;
                coupleEditTextObjects["celebrityLastNameEditText"].textView.Text = selectedCouple.CelebrityLastName;
                coupleEditTextObjects["professionalFirstNameEditText"].textView.Text = selectedCouple.ProfessionalFirstName;
                coupleEditTextObjects["professionalLastNameEditText"].textView.Text = selectedCouple.ProfessionalLastName;
                coupleEditTextObjects["celebrityStarRatingEditText"].textView.Text = selectedCouple.CelebrityStarRating.ToString();
                coupleEditTextObjects["votedOffWeekNumberEditText"].textView.Text = selectedCouple.VotedOffWeekNumber.ToString();
            }

            FindViewById<TextView>(Resource.Id.editCoupleButton).Click += EditCoupleButton_Click;
            FindViewById<TextView>(Resource.Id.deleteCoupleButton).Click += DeleteCoupleButton_Click;
            FindViewById<TextView>(Resource.Id.saveCoupleChangesButton).Click += SaveCoupleButton_Click;
        }

        private void EditCoupleButton_Click(object sender, System.EventArgs e)
        {
            if (coupleEditTextObjectsEnabled)
            {
                List<CoupleAdministrationTextObject> invalidInputs = GetInvalidInputs(coupleEditTextObjects);

                if (invalidInputs.Count > 0)
                {
                    string invalidInputsString = "";
                    bool isFirstInvalidInput = true;

                    foreach (CoupleAdministrationTextObject invalidInput in invalidInputs)
                    {
                        if (!isFirstInvalidInput)
                        {
                            invalidInputsString += ", ";
                        }

                        invalidInputsString += invalidInput.label;

                        isFirstInvalidInput = false;
                    }

                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();

                    alert.SetTitle("Invalid Inputs!");
                    alert.SetMessage("The following inputs are invalid: " + invalidInputsString);
                    alert.SetButton("OK", (c, ev) => { });
                    alert.Show();

                    return;
                }

                coupleEditTextObjectsEnabled = false;
                SetCoupleEditTextObjectsEnabled(coupleEditTextObjects, coupleEditTextObjectsEnabled);

                FindViewById<TextView>(Resource.Id.saveCoupleChangesButton).Enabled = true;

                FindViewById<TextView>(Resource.Id.editCoupleButton).Text = "Edit";
            }
            else
            {
                coupleEditTextObjectsEnabled = true;
                SetCoupleEditTextObjectsEnabled(coupleEditTextObjects, coupleEditTextObjectsEnabled);

                FindViewById<TextView>(Resource.Id.saveCoupleChangesButton).Enabled = false;

                FindViewById<TextView>(Resource.Id.editCoupleButton).Text = "Finish Editing";
            }
        }

        private void SetCoupleEditTextObjectsEnabled(Dictionary<String, CoupleAdministrationTextObject> coupleEditTextObjects, bool isEnabled)
        {
            foreach (KeyValuePair<String, CoupleAdministrationTextObject> keyValuePair in coupleEditTextObjects)
            {
                keyValuePair.Value.textView.Enabled = isEnabled;
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

        private void UpdateCoupleValues(Couple couple, Dictionary<String, CoupleAdministrationTextObject> coupleEditTextObjects)
        {
            int coupleIDEditTextValue;
            Int32.TryParse(coupleEditTextObjects["coupleIDEditText"].textView.Text, out coupleIDEditTextValue);

            int celebrityStarRatingEditTextValue;
            Int32.TryParse(coupleEditTextObjects["celebrityStarRatingEditText"].textView.Text, out celebrityStarRatingEditTextValue);

            int votedOffWeekNumberEditTextValue;
            Int32.TryParse(coupleEditTextObjects["votedOffWeekNumberEditText"].textView.Text, out votedOffWeekNumberEditTextValue);

            couple.CoupleID = coupleIDEditTextValue;
            couple.CelebrityFirstName = coupleEditTextObjects["celebrityFirstNameEditText"].textView.Text;
            couple.CelebrityLastName = coupleEditTextObjects["celebrityLastNameEditText"].textView.Text;
            couple.ProfessionalFirstName = coupleEditTextObjects["professionalFirstNameEditText"].textView.Text;
            couple.ProfessionalLastName = coupleEditTextObjects["professionalLastNameEditText"].textView.Text;
            couple.CelebrityStarRating = celebrityStarRatingEditTextValue;
            couple.VotedOffWeekNumber = votedOffWeekNumberEditTextValue;
        }

        private List<CoupleAdministrationTextObject> GetInvalidInputs(Dictionary<String, CoupleAdministrationTextObject> coupleEditTextObjects)
        {
            List<CoupleAdministrationTextObject> invalidInputs = new List<CoupleAdministrationTextObject>();

            Regex regex = new Regex(@"^[a-zA-Z0-9]*$");

            foreach (KeyValuePair<String, CoupleAdministrationTextObject> keyValuePair in coupleEditTextObjects)
            {
                if (keyValuePair.Value.textView.Text == "" && keyValuePair.Value.allowNull)
                {
                    continue;
                }

                if (keyValuePair.Value.type == typeof(string) && !regex.IsMatch(keyValuePair.Value.textView.Text))
                {
                    invalidInputs.Add(keyValuePair.Value);
                }
                else if (keyValuePair.Value.type == typeof(int))
                {
                    int checkingValue;

                    if (!Int32.TryParse(keyValuePair.Value.textView.Text, out checkingValue))
                    {
                        invalidInputs.Add(keyValuePair.Value);
                    }
                }
            }

            return invalidInputs;
        }
    }
}