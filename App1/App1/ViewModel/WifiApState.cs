using System.ComponentModel;

namespace App1
{
    /// <summary>
    /// Wifi状态枚举
    /// </summary>
    public enum WifiApState
    {
        [Description("禁用中")]
        Disabling = 10,
        [Description("禁用")]
        Disabled = 11,
        [Description("启用中")]
        Enabling = 12,
        [Description("启用")]
        Enabled = 13,
        [Description("失败")]
        Failed = 14,
    }
}