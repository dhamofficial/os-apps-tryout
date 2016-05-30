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
using activity_designs.Helpers;

namespace activity_designs
{
    [Activity(Label = "LocationActivity")]
    public class LocationActivity : Activity
    {
        TextView _locationText;
        TextView _addressText;
        TextView _remarksText;

        GPSServiceBinder _binder;
        GPSServiceConnection _gpsServiceConnection;
        Intent _gpsServiceIntent;
        private GPSServiceReciever _receiver;

        public static LocationActivity Instance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Instance = this;
            SetContentView(Resource.Layout.LocationLayout);

            _addressText = FindViewById<TextView>(Resource.Id.txtAddress);
            _locationText = FindViewById<TextView>(Resource.Id.txtLocation);
            _remarksText = FindViewById<TextView>(Resource.Id.txtRemarks);

            RegisterService();
        }

        private void RegisterService()
        {
            _gpsServiceConnection = new GPSServiceConnection(_binder);
            _gpsServiceIntent = new Intent(Android.App.Application.Context, typeof(GPSService));
            BindService(_gpsServiceIntent, _gpsServiceConnection, Bind.AutoCreate);
        }
        private void RegisterBroadcastReceiver()
        {
            IntentFilter filter = new IntentFilter(GPSServiceReciever.LOCATION_UPDATED);
            filter.AddCategory(Intent.CategoryDefault);
            _receiver = new GPSServiceReciever();
            RegisterReceiver(_receiver, filter);
        }

        private void UnRegisterBroadcastReceiver()
        {
            UnregisterReceiver(_receiver);
        }
        public void UpdateUI(Intent intent)
        {
            _locationText.Text = intent.GetStringExtra("Location");
            _addressText.Text = intent.GetStringExtra("Address");
            _remarksText.Text = intent.GetStringExtra("Remarks");
        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterBroadcastReceiver();
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnRegisterBroadcastReceiver();
        }

        [BroadcastReceiver]
        internal class GPSServiceReciever : BroadcastReceiver
        {
            public static readonly string LOCATION_UPDATED = "LOCATION_UPDATED";
            public override void OnReceive(Context context, Intent intent)
            {
                if (intent.Action.Equals(LOCATION_UPDATED))
                {
                    LocationActivity.Instance.UpdateUI(intent);
                }

            }
        }
    }
}