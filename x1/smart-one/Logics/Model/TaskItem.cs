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
using Java.Lang;
using SQLite;

namespace Logics.Model
{
    public class TaskItem
    {
        public TaskItem()
        {
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Done { get; set; }

        public DateTime ReminderTime { get; set; }
       
    }

    public class TaskFilter
    {
        /// <summary>
        /// 1 - This week
        /// 2 - This month
        /// 3 - Upcoming
        /// </summary>
        public int TaskListFilterType { get; set; }
    }
}