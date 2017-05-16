using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Net;
using Android.Widget;
using Android.OS;

namespace App1
{
    [Activity(Label = "App1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private static TimerTaskPerform S { get; }

        private static List<string> IpList { get; set; } = new List<string>();

        static MainActivity()
        {
            S = new TimerTaskPerform();
        }

        private static int Count { get; set; }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            #region FindView
            var listView = FindViewById<ListView>(Resource.Id.listView1);
            var startBtn = FindViewById<Button>(Resource.Id.MyButton);
            var airplaneBtn = FindViewById<Button>(Resource.Id.MyButton1);
            var message = FindViewById<TextView>(Resource.Id.message);
            var systemStateTextView = FindViewById<TextView>(Resource.Id.textView2);
            var ssid = FindViewById<EditText>(Resource.Id.ssidEdit);
            var pwd = FindViewById<EditText>(Resource.Id.pwdEdit);
            var check = FindViewById<CheckBox>(Resource.Id.checkBox1);
            var timerText = FindViewById<EditText>(Resource.Id.timeEdit);
            listView.SetBackgroundColor(Color.Black);


            #endregion

            startBtn.Click += (s, e) =>
            {
                var btn = (Button)s;
                if (btn.Text == "结束工作")
                {
                    S.EndTask(() =>
                    {
                        Count = 0;
                        message.Text = $"设置WiFi热点和密码,否则以当前热点状态创建热点-{Count}";
                    });
                    btn.Text = "开始工作";
                }
                else
                {
                    var wifiState = WifiToolUtils.GetWifiState(BaseContext);
                    if (wifiState == WifiState.Enabled)
                    {
                        WifiToolUtils.SetWifiEnabled(BaseContext, false);
                    }

                    var wifiApModel = pwd.Text.Length >= 8 && !string.IsNullOrWhiteSpace(ssid.Text) && !string.IsNullOrWhiteSpace(pwd.Text) ? new WifiapViewModel(ssid.Text.Trim(), pwd.Text.Trim()) : null;
                    if (WifiToolUtils.GetWifiApState(BaseContext) != WifiApState.Enabled || WifiToolUtils.GetWifiApState(BaseContext) != WifiApState.Enabling)
                    {
                        WifiToolUtils.SetWifiApEnabled(BaseContext, true, wifiApModel);
                    }
                    S.StartTask(state =>
                    {
                        Task.Run(() =>
                        {
                            RunOnUiThread(() =>
                            {
                                wifiState = WifiToolUtils.GetWifiState(BaseContext);
                                var wifiApState = WifiToolUtils.GetWifiApState(BaseContext);
                                var airplaneModeState = AirplaneModeUtils.IsAirplaneModeOn(BaseContext);
                                Count++;
                                message.Text = $"设置WiFi热点和密码,否则以当前热点状态创建热点-{Count}";
                                systemStateTextView.Text = $"wifi状态:{wifiState}热点状态:{wifiApState}-------飞行模式:{airplaneModeState}";
                                var dataSource = WifiToolUtils.GetConnectedHotIp();
                                foreach (var item in dataSource)
                                {
                                    var ip = IpList.FirstOrDefault(c => c == item.Ip);
                                    if (ip == null && item.IsTrue)
                                    {
                                        IpList.Add(item.Ip);
                                    }else {
                                        Reset();
                                    }
                                    if (check.Checked)
                                    {
                                        int timer;
                                        var x = timerText.Text;
                                        if (string.IsNullOrWhiteSpace(x))
                                        {
                                            timer = 60;
                                        }
                                        else
                                        {
                                            timer = int.Parse(x);
                                        }
                                        if (timer <= Count)
                                        {
                                            Reset();
                                        }
                                    }
                                }
                                var listDataSource = dataSource.Select(item => $"地址:{item.Ip}---连接状态:{item.IsTrue}").ToArray();


                                listView.Adapter = new ArrayAdapter<string>(BaseContext, Android.Resource.Layout.SimpleListItem1, listDataSource);
                            });
                        });
                    });
                    btn.Text = "结束工作";
                }
            };

            airplaneBtn.Click += delegate
            {
                AirplaneModeUtils.SetAirplane(BaseContext, !AirplaneModeUtils.IsAirplaneModeOn(BaseContext));
            };
        }


        private void Reset()
        {
            Count = 0;
            WifiToolUtils.SetWifiApEnabled(BaseContext, false, null);
            Thread.Sleep(1000);
            AirplaneModeUtils.SetAirplane(BaseContext, true);
            Thread.Sleep(2000);
            AirplaneModeUtils.SetAirplane(BaseContext, false);
            Thread.Sleep(2000);
            WifiToolUtils.SetWifiApEnabled(BaseContext, true, null);
            Thread.Sleep(500);
            WifiToolUtils.SetMobileData(BaseContext, true);
        }
    }
}

