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
                //AddNewListCouple(couple);
            }
        }

        private void AddNewListCouple(Couple newCouple)
        {
            // Couple Holder
            TableRow coupleHolder = new TableRow(this);

            coupleHolder.SetMinimumWidth(25);
            coupleHolder.SetMinimumHeight(25);
            coupleHolder.SetBackgroundColor(Color.ParseColor("#5292C7"));

            TableLayout.LayoutParams coupleHolderLayoutParams = new TableLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
            coupleHolderLayoutParams.SetMargins(10, 10, 10, 10);
            coupleHolder.LayoutParameters = coupleHolderLayoutParams;

            FindViewById<LinearLayout>(Resource.Id.couplesAdministrationLinearLayout).AddView(coupleHolder);

            // Top Row Layout
            TableLayout topRow = new TableLayout(this);

            topRow.StretchAllColumns = true;

            TableLayout.LayoutParams topRowLayoutParams = new TableLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            topRow.LayoutParameters = topRowLayoutParams;

            coupleHolder.AddView(topRow);

            // Top Row
            TableRow tableRow = new TableRow(this);
            topRow.AddView(tableRow);

            // Couple Text
            TextView coupleText = new TextView(this);

            coupleText.Text = "Example Couple Name Here";
            coupleText.TextSize = 15;

            TableLayout.LayoutParams coupleTextLayoutParams = new TableLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
            coupleTextLayoutParams.LeftMargin = 10;
            coupleTextLayoutParams.Gravity = GravityFlags.CenterVertical|GravityFlags.Left;
            coupleText.LayoutParameters = coupleTextLayoutParams;
            coupleText.LayoutParameters = new TableRow.LayoutParams(1);

            tableRow.AddView(coupleText);

            // Expand Button
            Button expandButton = new Button(this);

            expandButton.Text = "Expand";

            TableLayout.LayoutParams expandButtonLayoutParams = new TableLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
            expandButtonLayoutParams.RightMargin = 10;
            expandButton.LayoutParameters = expandButtonLayoutParams;

            tableRow.AddView(expandButton);
        }
    }
}