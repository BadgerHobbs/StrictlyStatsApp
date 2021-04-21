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
using SQLite;

namespace StrictlyStatsDataLayer.Models
{
    [Serializable]
    [Table("Instructions")]
    public class Instruction
    {
        public Instruction()
        {
        }
        public Instruction(string instructionHeading, string instructionDetail)
        {
            InstructionHeading = instructionHeading;
            InstructionDetail = instructionDetail;
        }

        [PrimaryKey, AutoIncrement]
        public int InstructionID {
            get;
            set;
        }

        public string InstructionDetail { get; set; }
        public string InstructionHeading { get; set; }
    }
}