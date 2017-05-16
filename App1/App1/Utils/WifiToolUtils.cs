using System.Collections.Generic;
using System.Text.RegularExpressions;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Java.IO;
using Java.Lang;
using Java.Net;
using Java.Util.Concurrent;

namespace App1
{
    public class WifiToolUtils
    {
        /// <summary>
        ///  获得WIFI热点状态
        /// </summary>
        public static WifiApState GetWifiApState(Context context)
        {
            var wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
            var method = wifiManager.Class.GetMethod("getWifiApState");
            var i = (int)method.Invoke(wifiManager);
            return (WifiApState)i;
        }

        /// <summary>
        /// 设置WiFi热点状态
        /// 如果没有设置那么按默认走
        /// </summary>
        public static bool SetWifiApEnabled(Context context, bool enabled, WifiapViewModel model)
        {
            var wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
            WifiConfiguration myConfig;
            if (model != null)
            {
                myConfig = new WifiConfiguration
                {
                    Ssid = model.Ssid,
                    PreSharedKey = model.PreSharedKey
                };
                myConfig.AllowedAuthAlgorithms.Set((int)AuthAlgorithmType.Open);
                myConfig.AllowedKeyManagement.Set((int)GroupCipherType.Wep40);
            }
            else
            {
                var getWifiConfig = wifiManager.Class.GetMethod("getWifiApConfiguration", null);
                myConfig = (WifiConfiguration)getWifiConfig.Invoke(wifiManager, null);
                var setWifiConfig = wifiManager.Class.GetMethod("setWifiApConfiguration", myConfig.Class);
                setWifiConfig.Invoke(wifiManager, myConfig);
            }
            var enableWifi = wifiManager.Class.GetMethod("setWifiApEnabled", myConfig.Class, Boolean.Type);
            return (bool)enableWifi.Invoke(wifiManager, myConfig, enabled);
        }

        /// <summary>
        /// 设置WiFi
        /// </summary>
        public static bool SetWifiEnabled(Context context, bool enabled)
        {
            var wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
            return wifiManager.SetWifiEnabled(enabled);
        }

        /// <summary>
        /// 获得Wifi状态
        /// </summary>
        public static WifiState GetWifiState(Context context)
        {
            var wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
            return wifiManager.WifiState;
        }

        /// <summary>
        /// 设置数据流量
        /// </summary>
        public static void SetMobileData(Context context, bool enabled)
        {
            var mobileDataManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            var mobileClass = mobileDataManager.Class;
            var argsClass = new Class[1];
            argsClass[0] = Boolean.Type;
            var method = mobileClass.GetMethod("setMobileDataEnabled", argsClass);
            method.Invoke(mobileDataManager, enabled);
        }

        /// <summary>
        /// 获取链接到当前热点的设备IP：
        /// </summary>
        public static List<WifiApStateViewModel> GetConnectedHotIp()
        {
            var result = new List<WifiApStateViewModel>();
            var fr = new FileReader("/proc/net/arp");
            var br = new BufferedReader(fr);
            string line;
            while ((line = br.ReadLine()) != null)
            {
                var splitted = Regex.Split(line, " +");
                if (splitted.Length < 4 || splitted[0] == "IP") continue;
                var es = Executors.NewCachedThreadPool();
                var model = new WifiApStateViewModel()
                {
                    Ip = splitted[0],
                    State = splitted[2]
                };
                es.Submit(new Runnable(() =>
                {
                    var ip = InetAddress.GetByName(splitted[0]);
                    if (ip.IsReachable(1000))
                    {
                        model.State = "1";
                    }
                }));
                result.Add(model);
            }
            fr.Close();
            br.Close();
            return result;
        }
    }
}