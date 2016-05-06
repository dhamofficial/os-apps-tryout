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

namespace Logics.Model
{
    public class GenericActionResult
    {
        public bool Execution { get; set; }
        public string Response { get; set; }
    }
}