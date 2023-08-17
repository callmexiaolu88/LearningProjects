namespace Test
{
    public class Subclass : IA, IB
    {
        public void A()
        {
            System.Console.WriteLine("AAAAAAAAAAA");
        }

        public void B()
        {
            System.Console.WriteLine("BBBBBBBBBBBB");

        }
    }

    public interface IA
    {
        void A();
    }

    public interface IB
    {
        void B();
    }
}