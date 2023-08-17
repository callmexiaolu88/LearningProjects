using System;
using System.Drawing;
using System.IO;

namespace imageBase64
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args?.Length > 0)
            {
                foreach (var item in args)
                {
                    try
                    {
                        var image = Image.FromFile(item);
                        string base64Str;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            image.Save(ms, image.RawFormat);

                            base64Str = Convert.ToBase64String(ms.ToArray());
                        }
                        Console.WriteLine($"================= {item} =================");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(base64Str);
                        Console.ResetColor();
                        Console.WriteLine($"================= End {item} =================");
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"================= {item} =================");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex);
                        Console.ResetColor();
                        Console.WriteLine($"================= End {item} =================");
                        Console.WriteLine();
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please type the full path of image.");
                Console.ResetColor();
                Console.WriteLine("Please press any key to exist.... ");
                Console.Read();
            }
        }
    }
}

