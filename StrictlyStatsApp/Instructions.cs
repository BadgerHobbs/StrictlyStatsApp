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
    [Activity(Label = "Instructions", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
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

            if (IsPortrait())
            {
                SetLandscapeInstructions(null, false);
            }
            else
            {
                SetLandscapeInstructions(instructions[0], true);
            }
        }

        private void LstVwInstructions_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (IsPortrait())
            {
                Intent intent = new Intent(this, typeof(InstructionDetailActivity));
                intent.PutExtra("InstructionID", instructions[e.Position].InstructionID);
                StartActivity(intent);
            }
            else
            {
                SetLandscapeInstructions(instructions[e.Position], true);
            }
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            Intent homeIntent = new Intent(this, typeof(MainActivity));

            StartActivity(homeIntent);
        }

        private void SetLandscapeInstructions(Instruction selectedInstruction, bool isEnabled)
        {
            if (!isEnabled)
            {
                FindViewById<LinearLayout>(Resource.Id.instructionDetailLinearLayout).Visibility = ViewStates.Gone;
            }
            else
            {
                FindViewById<LinearLayout>(Resource.Id.instructionDetailLinearLayout).Visibility = ViewStates.Visible;

                FindViewById<TextView>(Resource.Id.instructionHeadingTextView).Text = selectedInstruction.InstructionHeading;
                FindViewById<TextView>(Resource.Id.instructionDetailTextView).Text = selectedInstruction.InstructionDetail;
            }
            
        }

        private bool IsPortrait()
        {
            var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;

            if (surfaceOrientation == SurfaceOrientation.Rotation0 || surfaceOrientation == SurfaceOrientation.Rotation180)
            {
                return true;
            }

            return false;
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait)
            {
                SetLandscapeInstructions(null, false);
            }
            else if (newConfig.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                SetLandscapeInstructions(instructions[0], true);
            }
        }
    }
}