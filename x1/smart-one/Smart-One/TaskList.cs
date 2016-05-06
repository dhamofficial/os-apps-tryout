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
using Smart_One.Helpers;
using Logics.Model;
using Logics.Task;
using SmartOne.Views;

namespace SmartOne
{
    [Activity(Label = "My Tasks")]
    public class TaskList : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TaskListLayout);

            // Create your application here
            Init();
        }

        private void Init()
        {
            var lstTasks = FindViewById<ListView>(Resource.Id.lstTasks);
            lstTasks.ItemClick += OnListItemClick;

            TaskRepository repo = new TaskRepository();
            var listData = repo.GetItems();

            lstTasks.Adapter = new TaskListAdapter(this, listData);
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}