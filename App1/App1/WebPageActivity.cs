using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace App1
{
    [Activity(Label = "WebPageActivity")]
    public class WebPageActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var webPage = FindViewById<WebView>(Resource.Id.WebViewPage);
            var startBtn = FindViewById<Button>(Resource.Id.StartBtn);
            webPage.Settings.CacheMode = CacheModes.NoCache;
            startBtn.Click += (s, e) =>
            {
                
            };
        }
    }
}