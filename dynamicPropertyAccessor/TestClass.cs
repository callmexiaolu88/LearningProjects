using System;
namespace dynamicPropertyAccessor
{
    public class TestClass : BaseTestClass
    {
        public double Cost { get; set; }
        public Guid Gid { get; set; }
        public bool Flag { get; set; }
        public Item Item { get; set; } = new Item();
    }

    public class BaseTestClass
    {
        public int Id { get; private set; }
        public string Name { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}