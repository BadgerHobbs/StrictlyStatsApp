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
using StrictlyStatsDataLayer.Models;

namespace StrictlyStats
{
    class InstructionsAdapter : ArrayAdapter<Instruction>
    {
        IList<Instruction> items;
        Activity context;
        public InstructionsAdapter(Activity context, IList<Instruction> objects) : base(context, Android.Resource.Id.Text1, objects)
        {
            this.context = context;
            items = objects;
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.TwoLineListItem, null);
            var item = GetItem(position);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.InstructionHeading;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = item.InstructionDetail.Substring(0, 20) + "...";
            return view;
        }
    }
}