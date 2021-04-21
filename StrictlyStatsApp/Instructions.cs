using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Content;
using StrictlyStatsDataLayer;
using StrictlyStatsDataLayer.Models;

namespace StrictlyStats
{
    [Activity(Label = "Instructions")]
    public class Instructions : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        List<Instruction> instructions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Instructions);
            ListView lstVwInstructions = FindViewById<ListView>(Resource.Id.lstVwInstructions);

            instructions = uow.Instructions.GetAll();

            lstVwInstructions.Adapter = new InstructionsAdapter(this, instructions);
            lstVwInstructions.ItemClick += LstVwInstructions_ItemClick;

            Button btnHome = FindViewById<Button>(Resource.Id.btnHome);
            btnHome.Click += BtnHome_Click;
        }

        private void LstVwInstructions_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(InstructionDetailActivity));
            intent.PutExtra("InstructionID", instructions[e.Position].InstructionID);
            StartActivity(intent);
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            Intent homeIntent = new Intent(this, typeof(MainActivity));

            StartActivity(homeIntent);
        }
    }
}