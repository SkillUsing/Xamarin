using System.Linq;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.Net.Wifi.P2p;
using Android.Widget;

namespace App1
{
    public class MyBroadcastReceiver : BroadcastReceiver
    {
        public MyBroadcastReceiver()
        {

        }



        public override void OnReceive(Context context, Intent intent)
        {
            //var msg = intent.Extras.Get("msg").ToString();
            //Toast.MakeText(context, $"{intent.Action}++++", ToastLength.Long).Show();
            var managet = (WifiManager)context.GetSystemService(Context.WifiService);
            var info = managet.ConnectionInfo;
            //var z = managet.ConfiguredNetworks;
            //if (z!=null)
            //{
            //    foreach (var item in z.ToList())
            //    {
            //        Toast.MakeText(context, $"{ item.StatusField}++++{item.Bssid}", ToastLength.Long).Show();
            //    }
            //}
            Toast.MakeText(context, $"{ info.IpAddress}++++{info.SupplicantState}", ToastLength.Long).Show();

        }
    }
}