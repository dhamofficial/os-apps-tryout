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
using Logics.Task;

namespace activity_designs
{
    [Activity(Label = "My Tasks")]
    public class TaskList : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TaskList);

            Init();
        }
         
        private void Init()
        {

            var lstTasks = FindViewById<ListView>(Resource.Id.tasklistview);
            lstTasks.ItemClick += OnListItemClick;

            TaskRepository repo = new TaskRepository();
            var listData = repo.GetItems();

            lstTasks.Adapter = new TaskListAdapter(this, listData);


            var addTask = FindViewById<ImageView>(Resource.Id.addTask);
            addTask.Click += AddTask_Click;

            var gotoHome_taskList = FindViewById<ImageView>(Resource.Id.gotoHome_taskList);
            gotoHome_taskList.Click += GotoHome_taskList_Click;
        }

        private void GotoHome_taskList_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        private void AddTask_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(AddTask));
            Finish();
            StartActivity(intent);
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}