using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using activity_designs.Helpers;
using Logics.Task;
using Android.Provider;
using Android.Accounts;
using System.Collections.Generic;

namespace activity_designs
{
    //[Activity(Label = "activity_designs", MainLauncher = true, Icon = "@drawable/icon")]
    [Activity(Label = "SimpleOne - Smart Launcher", MainLauncher = true)]
    [IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { Intent.CategoryHome, Intent.CategoryDefault })]
    public class MainActivity : Activity
    {
        TextView txtFirstname_MainActivity;
        TextView txtLastname_MainActivity;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Init();
        }

        protected override void OnResume()
        {   
            base.OnResume();
            LoadSettings();
            LoadTasksBlock();
        }

        private void Init()
        {
            var showAllTasks = FindViewById<TextView>(Resource.Id.showAllTasks);
            var createNewTask = FindViewById<TextView>(Resource.Id.createNewTask);
            var settings = FindViewById<TextView>(Resource.Id.settings);

            txtFirstname_MainActivity = FindViewById<TextView>(Resource.Id.txtFirstname_MainActivity);
            txtLastname_MainActivity = FindViewById<TextView>(Resource.Id.txtLastname_MainActivity);

            showAllTasks.Click += ShowAllTasks_Click;
            createNewTask.Click += createNewTask_Click;
            settings.Click += Settings_Click;

            LoadSettings();
            LoadTasksBlock();
        }
       
        private void LoadTasksBlock()
        {
            var repo = new TaskRepository();
            var data = repo.GetTasksDashboardData();
            if(data!=null && data.Count>0)
            {
                List<TextView> controls = new List<TextView>();

                controls.Add(FindViewById<TextView>(Resource.Id.stat1Key));
                controls.Add(FindViewById<TextView>(Resource.Id.stat1Val));
                //controls.Add(FindViewById<LinearLayout>(Resource.Id.divStat1));

                controls.Add(FindViewById<TextView>(Resource.Id.stat2Key));
                controls.Add(FindViewById<TextView>(Resource.Id.stat2Val));
                //controls.Add(FindViewById<LinearLayout>(Resource.Id.divStat2));

                controls.Add(FindViewById<TextView>(Resource.Id.stat3Key));
                controls.Add(FindViewById<TextView>(Resource.Id.stat3Val));
                //controls.Add(FindViewById<LinearLayout>(Resource.Id.divStat3));

                FindViewById<LinearLayout>(Resource.Id.divStat1).Visibility = ViewStates.Gone;
                FindViewById<LinearLayout>(Resource.Id.divStat2).Visibility = ViewStates.Gone;
                FindViewById<LinearLayout>(Resource.Id.divStat3).Visibility = ViewStates.Gone;

                int index = 0;
                foreach (var item in data)
                {
                    if (index == 0)
                    {
                        controls[0].Text = item.Key;
                        controls[1].Text = item.Value;
                        FindViewById<LinearLayout>(Resource.Id.divStat1).Visibility = ViewStates.Visible;
                    }
                    else if (index == 1)
                    {
                        controls[2].Text = item.Key;
                        controls[3].Text = item.Value;
                        FindViewById<LinearLayout>(Resource.Id.divStat2).Visibility = ViewStates.Visible;
                    }
                    else if (index == 2)
                    {
                        controls[4].Text = item.Key;
                        controls[5].Text = item.Value;
                        FindViewById<LinearLayout>(Resource.Id.divStat3).Visibility = ViewStates.Visible;
                    }
                    index++;
                }

            }
        }

        private void LoadSettings()
        {
            var settings = new SettingsRepository();
            var model = settings.GetItem();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.Firstname))
                {
                    txtFirstname_MainActivity.Text = model.Firstname;
                    txtLastname_MainActivity.Text = model.Lastname;
                }
                else
                {
                    SetDefaultName();
                }
            }
            else
            {
                SetDefaultName();
            }

            var txtToday_MainActivity = FindViewById<TextView>(Resource.Id.txtToday_MainActivity);
            txtToday_MainActivity.Text = DateTime.Today.FormatToString();
        }

        
        private void SetDefaultName()
        {
            txtFirstname_MainActivity.Text = "Hello";
            txtLastname_MainActivity.Text = "";
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(Settings));
            StartActivity(intent);
        }

        private void createNewTask_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(AddTask));
            StartActivity(intent);
        }

        private void ShowAllTasks_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(TaskList));
            StartActivity(intent);
        }
    }
}

