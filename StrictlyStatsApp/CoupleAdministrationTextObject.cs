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

namespace StrictlyStats
{
    public class CoupleAdministrationTextObject
    {
        public string label;
        public TextView textView;
        public Type type;
        public bool allowNull;

        public CoupleAdministrationTextObject(string label, TextView textView, Type type, bool allowNull)
        {
            this.label = label;
            this.textView = textView;
            this.type = type;
            this.allowNull = allowNull;
        }
    }
}