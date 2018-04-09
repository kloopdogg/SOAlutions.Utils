using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Client
{

    public class TestObject
    {
        public string TestString { get; set; }
        public int TestInt { get; set; }
        public TestEnum TestEnum { get; set; }
    }

    public enum TestEnum
    {
        First,
        Second,
    }
}
