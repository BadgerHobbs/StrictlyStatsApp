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
    [Activity(Label = "SelectAdministrationCoupleActivity")]
    public class SelectAdministrationCoupleActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        private List<Couple> couples;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SelectAdministrationCouple);

            couples = uow.Couples.GetAll();

            FindViewById<ListView>(Resource.Id.coupleListView).Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, couples.ToArray<Couple>());

            FindViewById<ListView>(Resource.Id.coupleListView).ItemClick += ListView_ItemClick_AdministrateCouple;
            FindViewById<Button>(Resource.Id.addCoupleButon).Click += AddCoupleButton_ItemClick;
        }

        // Method to add to list view item for selecting a couple to administrate
        private void ListView_ItemClick_AdministrateCouple(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent coupleAdministrationActivity = new Intent(this, typeof(CoupleAdministrationActivity));

            coupleAdministrationActivity.PutExtra("SelectedCoupleIndex", e.Position);

            StartActivity(coupleAdministrationActivity);
        }

        // Method to add to list view item for selecting a couple to administrate
        private void AddCoupleButton_ItemClick(object sender, System.EventArgs e)
        {
            Intent coupleAdministrationActivity = new Intent(this, typeof(CoupleAdministrationActivity));

            coupleAdministrationActivity.PutExtra("SelectedCoupleIndex", -1);

            StartActivity(coupleAdministrationActivity);
        }
    }
}