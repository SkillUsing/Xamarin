using System;
using Android.Content;
using Android.OS;
using Android.Provider;

namespace App1
{
    public class AirplaneModeUtils
    {

        public static int AllowWriteSecureSettings = 17;

        public static void SetAirplane(Context context, bool enable)
        {
            SetAirplane(context, enable, false);
        }

        public static void SetAirplane(Context context, bool enable, bool sendBroadcastMannuly)
        {
            var isEnabled = IsAirplaneModeOn(context);
            if (isEnabled == enable)
                return;
            ChangeAirplaneMode(context, enable ? 1 : 0, sendBroadcastMannuly);
        }

        public static bool IsAirplaneModeOn(Context context)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBeanMr1)
            {
                return IsAirplaneModeOnLow(context);
            }
            else
            {
                return IsAirplaneModeOnHigh(context);
            }
        }

        private static bool IsAirplaneModeOnLow(Context context)
        {
            return Settings.System.GetInt(context.ContentResolver, Settings.Global.AirplaneModeOn, 0) != 0;
        }

        private static void ChangeAirplaneMode(Context context, int value, bool sendBroadcastMannuly = false)
        {
            if (!(Build.VERSION.SdkInt < BuildVersionCodes.JellyBeanMr1))
            {
                SetSettingsOnHigh(context, value);
            }
            else if (sendBroadcastMannuly)
            {
                SetSettingsOnLow(context, value);
                try
                {
                    var intent = new Intent(Intent.ActionAirplaneModeChanged);
                    intent.PutExtra("state", value == 1);
                    context.SendBroadcast(intent);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }
            }
        }
        private static bool IsAirplaneModeOnHigh(Context context)
        {
            return Settings.Global.GetInt(context.ContentResolver,
                    Settings.Global.AirplaneModeOn, 0) != 0;
        }

        private static void SetSettingsOnHigh(Context context, int value)
        {
            var commond = HigherAirplaneModePref1 + value + ";";
            if (value == 1)
                commond += HigherAirplaneModePref2 + "true";
            else
                commond += HigherAirplaneModePref2 + "false";
            ShellUtil.RunRootCmd(commond);
        }

        private static void SetSettingsOnLow(Context context, int value)
        {
            Settings.System.PutInt(context.ContentResolver,
                    Settings.Global.AirplaneModeOn, value);
        }

        public static string HigherAirplaneModePref1 = "settings put global airplane_mode_on ";
        public static string HigherAirplaneModePref2 = "am broadcast -a android.intent.action.AIRPLANE_MODE --ez state ";
    }
}