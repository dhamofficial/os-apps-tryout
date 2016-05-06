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

         

        public List<TaskItem> GetItems()
        {
            lock (locker)
            {
                return (from i in database.Table<TaskItem>() select i).ToList();
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