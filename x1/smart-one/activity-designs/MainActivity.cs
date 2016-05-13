using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using activity_designs.Helpers;

namespace activity_designs
{
    [Activity(Label = "activity_designs", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Init();
        }

        private void Init()
        {
            var showAllTasks = FindViewById<TextView>(Resource.Id.showAllTasks);
            var createNewTask = FindViewById<TextView>(Resource.Id.createNewTask);
            var settings = FindViewById<TextView>(Resource.Id.settings);

            showAllTasks.Click += ShowAllTasks_Click;
            createNewTask.Click += createNewTask_Click;
            settings.Click += Settings_Click;

            var txtToday_MainActivity= FindViewById<TextView>(Resource.Id.txtToday_MainActivity);
            txtToday_MainActivity.Text = DateTime.Today.FormatToString();
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

