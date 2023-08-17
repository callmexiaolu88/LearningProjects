using System;

namespace enumFlagsLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            EnumMode mode = EnumMode.Read | EnumMode.Access;
            EnumMode none = EnumMode.None;

            System.Console.WriteLine(mode);
            System.Console.WriteLine(none);
            if ((none & EnumMode.Access) == EnumMode.None)
                System.Console.WriteLine(none);
            if ((mode & EnumMode.None) == EnumMode.None)
                System.Console.WriteLine(mode);
            if ((mode & EnumMode.Read) == EnumMode.Read)
                System.Console.WriteLine(mode);
            if ((mode & EnumMode.Write) == EnumMode.Write)
                System.Console.WriteLine(mode);
            Enum.TryParse<EnumMode>("42", true, out EnumMode parsedMode);
            System.Console.WriteLine(parsedMode);
            Enum.TryParse<EnumMode>("read", true, out parsedMode);
            System.Console.WriteLine(parsedMode);
            Enum.TryParse<EnumMode>("Read,access", true, out parsedMode);
            System.Console.WriteLine(parsedMode);
            Enum.TryParse<EnumMode>("write, access", true, out parsedMode);
            System.Console.WriteLine(parsedMode);
            Enum.TryParse<EnumMode>("deny|Full", true, out parsedMode);
            System.Console.WriteLine(parsedMode);
            Enum.TryParse<EnumMode>("none", true, out parsedMode);
            System.Console.WriteLine(parsedMode);

            var rwmode = EnumMode.ReadWrite;
            System.Console.WriteLine(rwmode);
            rwmode = EnumMode.Read | EnumMode.Write;
            System.Console.WriteLine(rwmode);
            Enum.TryParse<EnumMode>("readwrite", out parsedMode);
            System.Console.WriteLine(parsedMode);
            Enum.TryParse<EnumMode>("readwrite", true, out parsedMode);
            System.Console.WriteLine(parsedMode);

            var valueMode = (EnumMode)15;
            System.Console.WriteLine(valueMode);
            valueMode = (EnumMode)32;
            System.Console.WriteLine(valueMode);
        }
    }

    [Flags]
    public enum EnumMode
    {
        None = 0,
        Read = 1,
        Write = 2,
        Access = 4,
        Deny = 8,
        Full = 16,
        ReadWrite = Read | Write
    }
}
