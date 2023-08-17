using System;
using System.IO;
using System.Text.RegularExpressions;

namespace regexTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var fileFullName = @"C:\Repositories\R\trunk\prj\NewTVUTransport\OutputDebug\NginxConf\WebR.conf";
                var newValue = "1575";
                var replacementFormat = "listen {0};";
                var pattern = @"listen\s+(?<port>\d+?);";
                var replacement = string.Format(replacementFormat, newValue);
                string content = File.ReadAllText(fileFullName);
                Console.WriteLine(content);
                Match match = Regex.Match(content, pattern);
                Console.WriteLine($"match: {match}");
                if (match != null)
                {
                    Console.WriteLine($"match count: {match.Groups.Count}");
                    if (match.Groups.Count >= 2)
                    {
                        string oldValue = match.Groups[1].Value;
                        Console.WriteLine(oldValue);
                        if (oldValue != newValue)
                        {
                            //content = string.Concat(content, Environment.NewLine, string.Format(replacementFormat, newValue));                               
                            content = Regex.Replace(content, pattern, replacement);
                            Console.WriteLine(content);
                            File.WriteAllText(fileFullName, content);
                        }
                    }
                    else
                    {
                        content = string.Concat(content, Environment.NewLine, replacement, Environment.NewLine);
                        File.WriteAllText(fileFullName, content);
                    }
                }
                else
                {
                    Console.WriteLine("match is null");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
