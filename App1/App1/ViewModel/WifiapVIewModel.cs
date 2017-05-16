namespace App1
{
    public class WifiapViewModel
    {
        public string Ssid { get; set; }

        public string PreSharedKey { get; set; }

        public WifiapViewModel()
        {

        }

        public WifiapViewModel(string ssid,string preShareKey)
        {
            Ssid = ssid;
            PreSharedKey = preShareKey;
        }
    }

    public class WifiApStateViewModel
    {
        public string Ip { get; set; }
        public string State { get; set; }

        public bool IsTrue => State == "0x2";
    }
}