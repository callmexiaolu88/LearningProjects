using System.Threading.Tasks;
using System;
using System.Net.Sockets;
using System.Net;

namespace udpclient
{
    class Program
    {

        public Program(string name, int port)
        {
            this.name = name;
            this.port = port;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var p1 = new Program("p1", 1000);
            p1.Start(2000);
            var p2 = new Program("p2", 2000);
            p2.Start(1000);
            Console.Read();
        }

        public async void Start(int port)
        {
            for (int i = 0; i < 1000; i++)
            {
                SendBytes("127.0.0.1", port, new byte[] { byte.MaxValue });
                await Task.Delay(1000);
            }
        }

        private UdpClient _udpClient = null;
        int port = 1000;
        string name = null;
        private bool CorrectUdpClient()
        {
            bool result = false;
            try
            {
                if(_udpClient != null && _udpClient.Client.IsBound)
                {
                    result = true;
                }
                else
                {
                    _udpClient = new UdpClient(port);
                    Task.Run(async () =>
                    {
                        while (true)
                        {
                            try
                            {
                                if (_udpClient != null)
                                {
                                    IPEndPoint remoteEP = null;
                                    _udpClient.Receive(ref remoteEP);
                                    System.Console.WriteLine($"{name}-----------IP:{remoteEP.Address}, Port:{remoteEP.Port}");
                                }
                                else
                                {
                                    await Task.Delay(1000);
                                }
                            }
                            catch (System.Exception ex)
                            {
                                System.Console.WriteLine(ex);
                            }
                        }
                    });
                    result = true;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("retrieveClient() error, {0}.", ex.Message);
            }
            return result;
        }

        public void SendBytes(string remoteUrl, int remotePort, byte[] toSend)
        {
            try
            {
                if (CorrectUdpClient())
                {
                    _udpClient.Send(toSend, toSend.Length, remoteUrl, remotePort);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("SendBytes() error, {0}.", ex.Message);
            }
        }

    }
}
