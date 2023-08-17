namespace valuetypeLearning
{
    public class ReferenceValue
    {
        public StructData VT { get; set; }

        public void SetVT(StructData data)
        {
            VT = data;
        }

        public override string ToString()
        {
            return $"{VT}";
        }

        public static void Print(StructData data)
        {
            System.Console.WriteLine(data);
        }

    }
}
