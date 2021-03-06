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
using Logics.Task;
using Smart_One.Helpers;

namespace activity_designs
{
    [Activity(Label = "Add Task")]
    public class AddTask : Activity
    {
        int selectedTaskId = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddTask);

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

        void InitFields()
        {
            txtTaskTitle = FindViewById<EditText>(Resource.Id.txtTaskTitle);
            txtTaskDescription = FindViewById<EditText>(Resource.Id.txtTaskDescription);

            layoutReminderTime = FindViewById<LinearLayout>(Resource.Id.layoutReminderTime);
            chkRemindeMe = FindViewById<CheckBox>(Resource.Id.chkRemindeMe);
            chkIsDone = FindViewById<CheckBox>(Resource.Id.chkIsDone);
            txtReminderDate = FindViewById<EditText>(Resource.Id.txtReminderDate);
            txtReminderTime = FindViewById<EditText>(Resource.Id.txtReminderTime);
        }

        private void Init()
        {

            InitFields();

            selectedTaskId = Intent.GetIntExtra("selectedTaskId", 0);

            var btnSave = FindViewById<Button>(Resource.Id.btnSave);
            var imgDatePicker = FindViewById<ImageView>(Resource.Id.imgDatePicker);
            var imgTimePicker = FindViewById<ImageView>(Resource.Id.imgTimePicker);

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

            imgTimePicker.Click += delegate
            {
                TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (DateTime time)
                {
                    txtReminderTime.Text = time.ToLongTimeString();
                });
                frag.Show(FragmentManager, TimePickerFragment.TAG);
            };

            if(selectedTaskId>0)
            {
                LoadData();
            }
        }

        void LoadData()
        {
            if(selectedTaskId>0)
            {
                var task = new TaskRepository();
                var model = task.GetItem(selectedTaskId);

                txtTaskTitle.Text = model.Title;
                txtTaskDescription.Text = model.Description;

                if (model.ReminderTime != DateTime.MinValue)
                {
                    txtReminderDate.Text = model.ReminderTime.ToShortDateString();
                    txtReminderTime.Text = model.ReminderTime.ToShortTimeString();
                    chkRemindeMe.Checked = true;
                }
            }
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
            Save();
        }

        void Save()
        {
            if (string.IsNullOrEmpty(txtTaskTitle.Text))
            {
                Toast.MakeText(this, "Title cannot be empty", ToastLength.Short).Show();
            }            
            else
            {
                TaskItem item = new TaskItem() { ID=selectedTaskId, Title = txtTaskTitle.Text, Description = txtTaskDescription.Text, Done = chkIsDone.Checked };

                DateTime dateOnly = DateTime.Today;
                DateTime timeOnly = DateTime.Now.AddHours(1);

                if (!string.IsNullOrEmpty(txtReminderDate.Text))
                    dateOnly = Convert.ToDateTime(txtReminderDate.Text);

                if (!string.IsNullOrEmpty(txtReminderTime.Text))
                    timeOnly = Convert.ToDateTime(txtReminderTime.Text);

                item.ReminderTime = dateOnly.Add(timeOnly.TimeOfDay);

                var task = new TaskRepository();
                task.SaveItem(item);
                Clear();

                Toast.MakeText(this, "Task Saved!!!", ToastLength.Long).Show();
            }
        }

        void Clear()
        {
            txtTaskTitle.Text = string.Empty;
            txtTaskDescription.Text = string.Empty;
            chkRemindeMe.Checked = false;
            txtReminderDate.Text = string.Empty;
            txtReminderTime.Text = string.Empty;
        }
    }
}