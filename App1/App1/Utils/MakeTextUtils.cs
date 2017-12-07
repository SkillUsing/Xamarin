using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Widget;
using System;

namespace App1
{
    /// <summary>
    /// ������ʾ
    /// </summary>
    public class MakeTextUtils
    {
        public static void MakeText(Context context, string content)
        {
            Toast.MakeText(context, content, ToastLength.Short).Show();
        }

        public static void Alert(Activity activity, string message)
        {
            activity.RunOnUiThread(() =>
            {
                var dialog= new AlertDialog.Builder(activity);
                dialog.SetTitle("ȷ��").SetMessage(message).Show();
            });
        }

        public static void MessageBox(Activity activity, string message, string msgTitle, Action yesFun, Action noFun)
        {
            activity.RunOnUiThread(() =>
            {
                var msgBox=new AlertDialog.Builder(activity);
                msgBox.SetPositiveButton("ȷ��", (s, e) =>
                {
                    var d = (AlertDialog) s;
                    d.Cancel();
                    yesFun();
                });
                msgBox.SetNegativeButton("ȡ��", (s, e) => { noFun(); });
                msgBox.SetTitle(msgTitle).SetMessage(message).Show();
            });
        }
    }
}