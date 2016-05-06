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

namespace Logics.Model
{
    public class TaskItem
    {
        public TaskItem()
        {
        }

        //[PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Done { get; set; }

        public DateTime ReminderTime { get; set; }
    }
}