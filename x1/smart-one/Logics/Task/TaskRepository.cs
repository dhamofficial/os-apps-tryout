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
using Logics.Model;
using SQLite;
using System.IO;

namespace Logics.Task
{
    public class TaskRepository
    {
        GenericActionResult result;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
        /// if the database doesn't exist, it will create the database and all the tables.
        /// </summary>
        /// <param name='path'>
        /// Path.
        /// </param>
        public TaskRepository()
        {
            result = new GenericActionResult();


            database = new SQLiteConnection(DatabasePath);
            // create the tables
            database.CreateTable<TaskItem>();
        }
         
    
        static object locker = new object();

        SQLiteConnection database;

        string DatabasePath
        {
            get
            {
                var sqliteFilename = "TodoSQLite.db3";
                #if __IOS__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				var path = Path.Combine(libraryPath, sqliteFilename);
                #else
                #if __ANDROID__
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                var path = Path.Combine(documentsPath, sqliteFilename);
                #else
				// WinPhone
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);;
                #endif
                #endif
                return path;
            }
        }

        public Dictionary<string, string> GetTasksDashboardData()
        {
            var data = new Dictionary<string, string>();

            int TodaysCount = 0;
            int PendingCount = 0;
            int UpcomingCount = 0;

            var todayList = this.GetItems(new TaskFilter { TaskListFilterType=4 });
            TodaysCount = todayList.Where(o => o.Done == false).Count();

            if (TodaysCount>0)
                data.Add("Today", TodaysCount.ToString());

            var pendingList = this.GetItems(new TaskFilter { TaskListFilterType = 2 });
            PendingCount = pendingList.Where(o => o.Done == false).Count();

            if (PendingCount > 0)
                data.Add("Pending", PendingCount.ToString());

            var upcomingList = this.GetItems(new TaskFilter { TaskListFilterType = 3 });
            UpcomingCount = upcomingList.Where(o => o.Done == false).Count();

            if (UpcomingCount > 0)
                data.Add("Upcoming", UpcomingCount.ToString());


            return data;
        }

        public List<TaskItem> GetItems(TaskFilter filter=null)
        {
            lock (locker)
            {
                var list = (from i in database.Table<TaskItem>() select i).ToList();

                if(filter!=null)
                {
                    if(filter.TaskListFilterType>0)
                    {
                        //this week
                        if (filter.TaskListFilterType == 1)
                        {
                            DateTime givenDate = DateTime.Today;
                            DateTime startOfWeek = givenDate.AddDays(-(int) givenDate.DayOfWeek);
                            DateTime endOfWeek = startOfWeek.AddDays(7);

                            list = list.Where(item => (item.ReminderTime.Date >= startOfWeek.Date && item.ReminderTime.Date <= endOfWeek.Date)).ToList();
                        }
                        //this month
                        else if (filter.TaskListFilterType == 2)
                        {
                            int CurrentYear = DateTime.Today.Year;
                            int CurrentMonth = DateTime.Today.Month;

                            DateTime startDate = new DateTime(CurrentYear, CurrentMonth, 1);
                            DateTime endDate = startDate.AddMonths(1).AddMinutes(-1);

                            list = list.Where(item => (item.ReminderTime.Date >= startDate.Date && item.ReminderTime.Date <= endDate.Date )).ToList();
                        }
                        //upcoming
                        else if (filter.TaskListFilterType == 3)
                        {
                            int CurrentYear = DateTime.Today.Year;
                            int CurrentMonth = DateTime.Today.Month;

                            DateTime startDate = new DateTime(CurrentYear, CurrentMonth, 1);
                            DateTime endDate = startDate.AddMonths(1).AddMinutes(-1);

                            list = list.Where(item => (item.ReminderTime==null || item.ReminderTime==DateTime.MinValue || item.ReminderTime.Date>= endDate.Date)).ToList();
                        }
                        //today
                        else if (filter.TaskListFilterType == 4)
                        {
                            list = list.Where(item => (item.ReminderTime.Date.Equals( DateTime.Today.Date) )).ToList();
                        }
                    }
                }

                return list;
            }
        }

        public IEnumerable<TaskItem> GetItemsNotDone()
        {
            lock (locker)
            {
                return database.Query<TaskItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
            }
        }

        public TaskItem GetItem(int id)
        {
            lock (locker)
            {
                return database.Table<TaskItem>().FirstOrDefault(x => x.ID == id);
            }
        }

        public int SaveItem(TaskItem item)
        {
            lock (locker)
            {
                if(item.ReminderTime==null || item.ReminderTime==DateTime.MinValue)
                {
                    item.ReminderTime = DateTime.Today;//.AddDays(1).AddHours(9);
                }

                if (item.ID != 0)
                {
                    database.Update(item);
                    return item.ID;
                }
                else
                {
                    return database.Insert(item);
                }
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return database.Delete<TaskItem>(id);
            }
        }
    }
}