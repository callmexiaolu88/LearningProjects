using System;

static class UtilitiesHelper
{
    public static void Debug(object message)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}