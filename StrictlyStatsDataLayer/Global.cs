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

namespace StrictlyStatsDataLayer
{
    public static class Global
    {
        public static string DBNAME = "StrictlyStats.db";
        private static IStrictlyStatsUOW uow = null;
        public static IStrictlyStatsUOW UOW {
            get {
                if (uow == null)
                    uow = new StrictlyStatsUow(DBNAME);
                return uow;
            }
        }

        public static int NUMBEROFCONTESTANTSINFINAL = 2;
    }
}