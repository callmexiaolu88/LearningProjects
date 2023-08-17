using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;

namespace networkTools
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkInterface[] itfs = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface itf in itfs)
            {
                if (itf.OperationalStatus == OperationalStatus.Up && (itf.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || itf.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    IPInterfaceProperties p = itf.GetIPProperties();
                    if (itf.Name != "docker0")
                    {
                        foreach (UnicastIPAddressInformation ip in p.UnicastAddresses)
                        {
 
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                string ipv4Address = ip.Address.ToString().Trim();
                                System.Console.WriteLine($"[{ipv4Address.Length}]");
                                if (ipv4Address.Length > 0)
                                {
                                    System.Console.WriteLine($"[{ipv4Address}]");
                                }
                                else
                                    System.Console.WriteLine($"No");
                                if (!ipv4Address.StartsWith("169.254"))
                                    System.Console.WriteLine(ipv4Address);
                            }
                        }
                    }
                }
            }
        }
    }
}
