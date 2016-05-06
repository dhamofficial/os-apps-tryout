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

namespace SmartOne.Views
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
                view = context.LayoutInflater.Inflate(Resource.Layout.TaskListRow, parent, false);

            TaskItem item = this[position];
            view.FindViewById<TextView>(Resource.Id.Title).Text = item.Title;
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