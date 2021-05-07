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
        private List<Couple> couples;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CoupleAdministration);

            couples = uow.Couples.GetAll();

            int selectedCoupleIndex = (int)Intent.GetIntExtra("SelectedCoupleIndex", 0);

            Couple selectedCouple;

            if (selectedCoupleIndex == -1)
            {
                selectedCouple = new Couple();
                FindViewById<TextView>(Resource.Id.coupleNameTextView).Text = "New Couple";
            }
            else
            {
                selectedCouple = uow.Couples.GetAll()[selectedCoupleIndex];

                FindViewById<TextView>(Resource.Id.coupleNameTextView).Text = $"Editing: {selectedCouple.ToString()}";
            }
        }
    }
}