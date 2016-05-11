using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace activity_designs
{
    [Activity(Label = "activity_designs", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var showAllTasks = FindViewById<TextView>(Resource.Id.showAllTasks);

            showAllTasks.Click += ShowAllTasks_Click;
        }

        private void ShowAllTasks_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(TaskList));
            StartActivity(intent);
        }
    }
}

