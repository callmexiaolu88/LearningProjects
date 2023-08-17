using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace valuetypeLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var valueTypeList = new List<StructData>();
            for (int i = 0; i < 10; i++)
            {
                StructData data = new StructData(i, i + 1);
                valueTypeList.Add(data);
            }
            //foreach (var item in valueTypeList)
            //{
            //    Console.WriteLine(item);
            //}

            ReferenceValue value = new ReferenceValue();
            value.SetVT(default(StructData));
            Console.WriteLine(value);

            value.SetVT(new StructData(45, 100));
            Console.WriteLine(value);

            ReferenceValue.Print(new StructData(12, 34));

            Span<ReferenceValue> sp = new Span<ReferenceValue>(new ReferenceValue[10]);
            Span<StructData> vsp = new Span<StructData>(valueTypeList.ToArray());
            vsp[1].X=100;

            var ptr= Marshal.AllocHGlobal(100);
            var str = "this is a test";
            Span<char> arr = str.ToCharArray();
            var strspan = str.AsSpan();
            new Span()
            Console.Read();
        }
    }
}
