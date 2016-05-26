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
using Logics.Model;

namespace activity_designs
{
    [Activity(Label = "Settings")]
    public class Settings : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);

            // Create your application here
            Init();
        }

        EditText Firstname;
        EditText Lastname;
        EditText Email;
        EditText Mobile;

        SettingsRepository repo;
        SettingsModel model;

        private void Init()
        {
            var btnSave = FindViewById<Button>(Resource.Id.btnSave);

            Firstname = FindViewById<EditText>(Resource.Id.Firstname);
            Lastname = FindViewById<EditText>(Resource.Id.Lastname);
            Email = FindViewById<EditText>(Resource.Id.Email);
            Mobile = FindViewById<EditText>(Resource.Id.Mobile);

            btnSave.Click += BtnSave_Click;

            repo = new SettingsRepository();

            LoadSettings();
        }

        private void LoadSettings()
        {
            model = repo.GetItem(1);
            if(model!=null)
            {
                Firstname.Text = model.Firstname;
                Lastname.Text = model.Lastname;
                Email.Text = model.Email;
                Mobile.Text = model.Mobile;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(model==null)
                    model = new SettingsModel();

                model.Firstname = Firstname.Text;
                model.Lastname = Lastname.Text;
                model.Email = Email.Text;
                model.Mobile = Mobile.Text;
                //model.ID = 1;
                repo.SaveItem(model);

                Toast.MakeText(this, "Settings Saved.", ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Sorry. Unable to save. >> "+ex.Message, ToastLength.Long).Show();
            }
            
        }
    }
}