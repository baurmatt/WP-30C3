using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Net.NetworkInformation;
using System.Threading;
using System.ComponentModel;

namespace _30C3
{
    public class NetworkUtilty
    {
        /// <summary>
        /// Property which returns if the wifi connection is also the currently used internet connection
        /// </summary>
        public static bool IsWiFiInternetConnection
        {
            get
            {
                if (NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    return true;
                else
                    return false;
            }
        }
    }
}