using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
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
            // Couple Holder
            TableRow coupleHolder = new TableRow(new ContextThemeWrapper(this, Resource.Style.CoupleTableRow));

            TableRow.LayoutParams coupleHolderLayoutParams = new TableRow.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
            coupleHolder.LayoutParameters = coupleHolderLayoutParams;

            FindViewById<LinearLayout>(Resource.Id.couplesLinearLayout).AddView(coupleHolder);

            // Top Row Layout
            LinearLayout topRow = new TableLayout(new ContextThemeWrapper(this, Resource.Style.CoupleLinearLayout));

            coupleHolder.AddView(topRow);

            // Couple Text
            TextView coupleText = new TextView(new ContextThemeWrapper(this, Resource.Style.CoupleTextView));

            topRow.AddView(coupleText);

            // Expand Button
            Button expandButton = new Button(new ContextThemeWrapper(this, Resource.Style.CoupleButton));

            topRow.AddView(expandButton);
        }
    }
}