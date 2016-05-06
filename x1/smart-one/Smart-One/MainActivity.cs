using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace SmartOne
{
    [Activity(Label = "SmartOne", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        //int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.btnAddTask);
            button.Click += Button_Click;

            Button btnShowMyTasks = FindViewById<Button>(Resource.Id.btnShowMyTasks);
            btnShowMyTasks.Click += BtnShowMyTasks_Click;

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
        }

        private void BtnShowMyTasks_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(TaskList));
            StartActivity(intent);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(AddTask));
            StartActivity(intent);

        }
    }
}

