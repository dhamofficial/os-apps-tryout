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
using Android.Graphics;
using Logics.Model;
using activity_designs.Helpers;

namespace activity_designs
{
    [Activity(Label = "My Tasks")]
    public class TaskList : Activity
    {
        private int selectedTab { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TaskList);

            Init();
        }

        TextView thisWeek_taskList;
        TextView thisMonth_taskList;
        TextView upcoming_taskList;
        TextView monthName_TaskList;

        private void Init()
        {
            var addTask = FindViewById<ImageView>(Resource.Id.addTask);
            addTask.Click += AddTask_Click;

            var gotoHome_taskList = FindViewById<ImageView>(Resource.Id.gotoHome_taskList);
            gotoHome_taskList.Click += GotoHome_taskList_Click;

            
            thisWeek_taskList= FindViewById<TextView>(Resource.Id.thisWeek_taskList);
            thisMonth_taskList = FindViewById<TextView>(Resource.Id.thisMonth_taskList);
            upcoming_taskList = FindViewById<TextView>(Resource.Id.upcoming_taskList);
            monthName_TaskList = FindViewById<TextView>(Resource.Id.monthName_TaskList);

            monthName_TaskList.Text = DateTime.Today.FormatToString();

            thisWeek_taskList.Click += delegate { selectedTab = 1; TabSelection(); };
            thisMonth_taskList.Click += delegate { selectedTab = 2; TabSelection(); };
            upcoming_taskList.Click += delegate { selectedTab = 3; TabSelection(); };

            selectedTab = 1;
            TabSelection();
        }

        private void TabSelection()
        {
            thisWeek_taskList.SetBackgroundColor(Color.ParseColor("#F44336"));
            thisMonth_taskList.SetBackgroundColor(Color.ParseColor("#F44336"));
            upcoming_taskList.SetBackgroundColor(Color.ParseColor("#F44336"));

            if (selectedTab==1)
            {
                thisWeek_taskList.SetBackgroundColor(Color.ParseColor("#D50000"));
            }
            else if (selectedTab ==2)
            {
                thisMonth_taskList.SetBackgroundColor(Color.ParseColor("#D50000"));
            }
            else if (selectedTab == 3)
            {
                upcoming_taskList.SetBackgroundColor(Color.ParseColor("#D50000"));
            }

            ShowList();
        }

        private void ShowList()
        {
            TaskRepository repo = new TaskRepository();
            var filter = new TaskFilter();
            filter.TaskListFilterType = selectedTab;

            var listData = repo.GetItems(filter);

            var lstTasks = FindViewById<ListView>(Resource.Id.tasklistview);
            lstTasks.ItemClick += OnListItemClick;

            var noTasks_TaskList = FindViewById<TextView>(Resource.Id.noTasks_TaskList);
            noTasks_TaskList.Visibility = ViewStates.Gone;

            if (listData.Count > 0)
            {
                lstTasks.Visibility = ViewStates.Visible;
                lstTasks.Adapter = new TaskListAdapter(this, listData);
            }
            else
            {
                lstTasks.Visibility = ViewStates.Gone;
                noTasks_TaskList.Visibility = ViewStates.Visible;
            }
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