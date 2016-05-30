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

        CheckBox chkIncludeCompleted;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TaskList);

            Init();
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirstLoad();
        }

        TextView thisWeek_taskList;
        TextView thisMonth_taskList;
        TextView upcoming_taskList;
        TextView monthName_TaskList;
        ListView lstTasks;
        List<TaskItem> TasksList;
        TextView noTasks_TaskList;

        private void Init()
        {
            var addTask = FindViewById<ImageView>(Resource.Id.addTask);
            addTask.Click += AddTask_Click;

            var gotoHome_taskList = FindViewById<ImageView>(Resource.Id.gotoHome_taskList);
            gotoHome_taskList.Click += GotoHome_taskList_Click;

            lstTasks = FindViewById<ListView>(Resource.Id.tasklistview);

            lstTasks.ItemClick += OnListItemClick;
            lstTasks.ItemLongClick += LstTasks_ItemLongClick;

            noTasks_TaskList = FindViewById<TextView>(Resource.Id.noTasks_TaskList);

            thisWeek_taskList = FindViewById<TextView>(Resource.Id.thisWeek_taskList);
            thisMonth_taskList = FindViewById<TextView>(Resource.Id.thisMonth_taskList);
            upcoming_taskList = FindViewById<TextView>(Resource.Id.upcoming_taskList);
            monthName_TaskList = FindViewById<TextView>(Resource.Id.monthName_TaskList);
            chkIncludeCompleted = FindViewById<CheckBox>(Resource.Id.chkIncludeCompleted);

            chkIncludeCompleted.Click += ChkIncludeCompleted_Click;

            monthName_TaskList.Text = DateTime.Today.FormatToString();

            thisWeek_taskList.Click += delegate { selectedTab = 1; TabSelection(); };
            thisMonth_taskList.Click += delegate { selectedTab = 2; TabSelection(); };
            upcoming_taskList.Click += delegate { selectedTab = 3; TabSelection(); };

            FirstLoad();
        }

        private void ChkIncludeCompleted_Click(object sender, EventArgs e)
        {
            ShowList();
        }

        private void FirstLoad()
        {
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
            
            noTasks_TaskList.Visibility = ViewStates.Gone;

            if (listData.Count > 0)
            {
                TasksList = listData;
                if (!chkIncludeCompleted.Checked)
                {
                    TasksList = listData.Where(i => i.Done == false).ToList();
                }

                var completedList = listData.Where(i=>i.Done==true).ToList();

                if (completedList.Count>0)
                {
                    chkIncludeCompleted.Visibility = ViewStates.Visible;
                    chkIncludeCompleted.Text = "Include Completed ("+ completedList.Count + ")";
                }
                else
                {
                    chkIncludeCompleted.Visibility = ViewStates.Gone;
                }

                lstTasks.Visibility = ViewStates.Visible;
                lstTasks.Adapter = new TaskListAdapter(this, TasksList);

                if(TasksList.Count==0)
                {
                    noTasks_TaskList.Text = "all tasks completed";
                    noTasks_TaskList.Visibility = ViewStates.Visible;
                    lstTasks.Visibility = ViewStates.Gone;
                }
                else
                {
                    noTasks_TaskList.Visibility = ViewStates.Gone;
                    lstTasks.Visibility = ViewStates.Visible;
                }
            }
            else
            {
                lstTasks.Visibility = ViewStates.Gone;
                noTasks_TaskList.Visibility = ViewStates.Visible;
            }
        }

        void UpdateTaskItem(int pos)
        {
            try
            {
                var item = (TaskItem)TasksList[pos];
                item.Done = !item.Done;

                var task = new TaskRepository();
                task.SaveItem(item);

                ShowList();

                Toast.MakeText(this, "Saved", ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
            }
        }

        private void LstTasks_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            UpdateTaskItem(e.Position);
            //Toast.MakeText(this, lstTasks.GetItemAtPosition(e.Position).ToString(), ToastLength.Long).Show();
        }

        private void GotoHome_taskList_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        private void AddTask_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(AddTask));
            StartActivity(intent);
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Toast.MakeText(this, "i am doing it.", ToastLength.Long);
            //throw new NotImplementedException();
            var intent = new Intent(this, typeof(AddTask));
            intent.PutExtra("selectedTaskId",TasksList[e.Position].ID);
            StartActivity(intent);
        }
    }
}