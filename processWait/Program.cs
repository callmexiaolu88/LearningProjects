using System;
using System.Diagnostics;

namespace processWait
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Staring");
                Process p = new Process();
                p.StartInfo.FileName = "notepad";
                //p.StartInfo.Arguments = $"udp://10.12.22.33:40001";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                var b = p.Start();
                Console.WriteLine($"Start:{b}, PID:{p.Id}");
                b = p.WaitForExit(5000);
                Console.WriteLine($"WaitForExit:{b}");
                p.Dispose();
                Console.WriteLine("Read.");
                p.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SniffFormat() error, {ex.Message}.");
            }
        }
    }
}
