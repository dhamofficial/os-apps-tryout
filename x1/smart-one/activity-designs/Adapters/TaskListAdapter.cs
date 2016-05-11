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

namespace activity_designs
{
    public class TaskListAdapter : BaseAdapter<TaskItem>
    {
        Activity context;
        List<TaskItem> list;

        public TaskListAdapter(Activity _context, List<TaskItem> _list)
            : base()
        {
            this.context = _context;
            this.list = _list;
        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override TaskItem this[int index]
        {
            get { return list[index]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.TaskRow, parent, false);

            TaskItem item = this[position];
            if(item.ReminderTime!=null && item.ReminderTime!=DateTime.MinValue)
            {
                view.FindViewById<TextView>(Resource.Id.reminderTime).Text = item.ReminderTime.ToShortTimeString();
                view.FindViewById<TextView>(Resource.Id.reminderDay).Text = item.ReminderTime.ToShortDateString();
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.reminderTime).Text = string.Empty;
                view.FindViewById<TextView>(Resource.Id.reminderDay).Text = string.Empty;
            }

            view.FindViewById<TextView>(Resource.Id.title).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.caption).Text = item.Description;
            //view.FindViewById<TextView>(Resource.Id.Description).Text = item.Description;

            //using (var imageView = view.FindViewById<ImageView>(Resource.Id.Thumbnail))
            //{
            //    //string url = Android.Text.Html.FromHtml(item.thumbnail).ToString();

            //    //Download and display image
               
            //}
            return view;
        }
    }
}