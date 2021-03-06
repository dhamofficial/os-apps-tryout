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

namespace SmartOne
{
    [Activity(Label = "Add Task")]
    public class AddTask : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddTaskLayout);

            // Create your application here
            Init();
        }

        LinearLayout layoutReminderTime;
        EditText txtReminderTime;
        CheckBox chkRemindeMe;
        CheckBox chkIsDone;

        EditText txtTaskTitle;
        EditText txtTaskDescription;
        EditText txtReminderDate;

        private void Init()
        {
            var btnSave = FindViewById<Button>(Resource.Id.btnSave);
            layoutReminderTime = FindViewById<LinearLayout>(Resource.Id.layoutReminderTime);
            chkRemindeMe = FindViewById<CheckBox>(Resource.Id.chkRemindeMe);
            chkIsDone = FindViewById<CheckBox>(Resource.Id.chkIsDone);
            txtReminderDate = FindViewById<EditText>(Resource.Id.txtReminderDate);
            var imgDatePicker = FindViewById<ImageView>(Resource.Id.imgDatePicker);

            btnSave.Click += Button_Click;
            ReminderVisibility(false);
            chkRemindeMe.CheckedChange += ChkRemindeMe_CheckedChange;

            imgDatePicker.Click += delegate
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    txtReminderDate.Text = time.ToLongDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            txtReminderTime = FindViewById<EditText>(Resource.Id.txtReminderTime);
            var imgTimePicker = FindViewById<ImageView>(Resource.Id.imgTimePicker);

            imgTimePicker.Click += delegate
            {
                TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
                {
                    txtReminderTime.Text = time.ToLongTimeString();
                });
                frag.Show(FragmentManager, TimePickerFragment.TAG);
            };

        }

        private void ChkRemindeMe_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var chk = (CheckBox)sender;
            ReminderVisibility(chk.Checked);
        }

        void ReminderVisibility(bool show)
        {
            if(show)
            {
                if (layoutReminderTime != null)
                    layoutReminderTime.Visibility = ViewStates.Visible;
            }
            else
            {
                if (layoutReminderTime != null)
                    layoutReminderTime.Visibility = ViewStates.Gone;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            txtTaskTitle = FindViewById<EditText>(Resource.Id.txtTaskTitle);
            txtTaskDescription = FindViewById<EditText>(Resource.Id.txtTaskDescription);

            if(string.IsNullOrEmpty( txtTaskTitle.Text))
            {
                Toast.MakeText(this, "Title cannot be empty", ToastLength.Short).Show();
            }
            else if (string.IsNullOrEmpty(txtTaskDescription.Text))
            {
                Toast.MakeText(this, "Description cannot be empty", ToastLength.Short).Show();
            }
            else
            {
                TaskItem item = new TaskItem() { Title= txtTaskTitle.Text, Description=txtTaskDescription.Text, Done= chkIsDone.Checked };

                if(!string.IsNullOrEmpty(txtReminderTime.Text))
                    item.ReminderTime = Convert.ToDateTime(txtReminderTime.Text);

                TaskRepository task = new TaskRepository();
                task.SaveItem(item);
                Clear();
                Toast.MakeText(this, "Task Added!!!", ToastLength.Long).Show();
            }
        }

        void Clear()
        {
            txtTaskTitle.Text = string.Empty;
            txtTaskDescription.Text = string.Empty;
        }
    }
}