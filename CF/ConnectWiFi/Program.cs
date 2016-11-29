using NativeWifi;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using NativeWifi;

namespace ConnectWiFi
{
    class Program
    {
        private static ILog log;
        string[] nameWifi = new string[] { "dlink", "dlink 2" };
        String key = "password"; //TODO не забыть

        static void Main(string[] args)
        {

            FileInfo f = new FileInfo("log4net.config");
            log4net.Config.XmlConfigurator.Configure(f);
            log = LogManager.GetLogger(typeof(Program));
            Program ex = new Program();
            ex.StartTimer(); // запустить через 2 секунды

            Console.WriteLine("Press Enter to end the program.");
            Console.ReadLine();
        }

        public void StartTimer()
        {
            log.Debug("START");
            Timer t = new Timer(new TimerCallback(Connected));
            t.Change(0, 120000); //  повторять каждые 2 минуты
        }

        private void Connected(object state)
        {
            log.Debug("Call Connected");
            try
            {

                WlanClient client = new WlanClient();
                foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
                {
                    Wlan.WlanAvailableNetwork[] wlanBssEntries = wlanIface.GetAvailableNetworkList(0);
                    foreach (Wlan.WlanAvailableNetwork network in wlanBssEntries)
                    {

                        if (IsConn())
                        {
                            log.Debug("Connect is exist. Exit");
                            Environment.Exit(0);
                        }

                        String profileName = System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).Trim((char)0);
                       

                        if (nameWifi.Contains(profileName)) // если нашли сеть по имени
                        {
                            String strTemplate = "RSNAPSK.xml";
                            String authentication = "RSNAPSK";                            

                            switch ((int)network.dot11DefaultAuthAlgorithm)
                            {
                                case 7:
                                    try
                                    {
                                        string profileXml = string.Format("<?xml version=\"1.0\" encoding=\"US-ASCII\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><connectionMode>auto</connectionMode><autoSwitch>false</autoSwitch><MSM><security><authEncryption><authentication>WPAPSK</authentication><encryption>TKIP</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>passPhrase</keyType><protected>false</protected><keyMaterial>{1}</keyMaterial></sharedKey></security></MSM></WLANProfile>", profileName, key); //WPA-PSK
                                        wlanIface.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
                                        wlanIface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profileName);
                                        log.Debug("CONNECTED ENABLE");
                                        Thread.Sleep(10000);
                                    }
                                    catch (Exception)
                                    {

                                    }

                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex);
            }
        }


        private bool IsConn()
        {
            WlanClient client = new WlanClient();
            bool isConn = false;
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                Wlan.WlanAvailableNetwork[] wlanBssEntries = wlanIface.GetAvailableNetworkList(0);
                try
                {
                    var conn = wlanIface.CurrentConnection;
                    if ((int)conn.isState == 1)
                    {
                        isConn = true;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return isConn;

        }

    }
}
