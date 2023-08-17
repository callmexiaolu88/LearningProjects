namespace AnalysisTest
{
    public partial class SourceGeneratorTest
    {
        public void Hello(string name)
            => innerHello(name);

        [AnalysisLibrary.Hello]
        partial void innerHello(string name);
    }
}