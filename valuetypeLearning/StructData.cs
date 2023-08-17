namespace valuetypeLearning
{
    public struct StructData
    {
        public int X { get; set; }
        public int Y { get; set; }

        public StructData(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"[{X}:{Y}]";
        }
    }
}
