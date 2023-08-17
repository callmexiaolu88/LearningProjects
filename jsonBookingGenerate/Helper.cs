using System;
using System.Collections.Generic;
using System.Text;

namespace jsonBookingGenerate
{
    public class Helper
    {
        public static string GetInput()
        {
            StringBuilder stringBuilder = new StringBuilder();
            do
            {
                char key;
                if (!Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey();
                    if (keyInfo.Key.Equals(ConsoleKey.Enter))
                    {
                        break;
                    }
                    else
                    {
                        key = keyInfo.KeyChar;
                    }
                }
                else
                {
                    key = (char)Console.Read();
                }
                stringBuilder.Append(key);
            } while (true);
            return stringBuilder.ToString();
        }
    }
}
