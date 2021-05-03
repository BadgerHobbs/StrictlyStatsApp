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
    [Activity(Label = "CouplesAdministrationActivity")]
    public class CouplesAdministrationActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        private List<Couple> couples;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CouplesAdministration);

            couples = uow.Couples.GetAll();

            foreach (Couple couple in uow.Couples.GetAll())
            {
                AddNewListCouple(couple);
                break;
            }
        }

        private void AddNewListCouple(Couple newCouple)
        {
        }
    }
}