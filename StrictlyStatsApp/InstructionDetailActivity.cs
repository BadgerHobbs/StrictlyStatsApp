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
    [Activity(Label = "InstructionDetailActivity")]
    public class InstructionDetailActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InstructionDetail);
            int instructionID = Intent.GetIntExtra("InstructionID", -1);

            Instruction instruction = uow.Instructions.GetById(instructionID);

            TextView txtHeading = FindViewById<TextView>(Resource.Id.txtHeading);
            TextView txtDetail = FindViewById<TextView>(Resource.Id.txtDetail);

            txtHeading.Text = instruction.InstructionHeading;
            txtDetail.Text = instruction.InstructionDetail;
        }
    }
}