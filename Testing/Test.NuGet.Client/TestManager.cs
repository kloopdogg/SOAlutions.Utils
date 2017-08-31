using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOAlutions.Utils;

namespace Test.Client
{
    class TestManager
    {
        public void RunTests()
        {
            RunConsoleHostedTest();
            RunWebHostedTest();
        }

        private void RunConsoleHostedTest()
        {
            TestServiceContractClient client = new TestServiceContractClient("console");
            string response = client.TestMethod(22);
            client.SafeClose();

            ConsoleHelper.WriteInfo("Response from console-hosted service: {0}", response);
        }

        private void RunWebHostedTest()
        {
            TestServiceContractClient client = new TestServiceContractClient("web");
            string response = client.TestMethod(38);
            client.SafeClose();

            ConsoleHelper.WriteInfo("Response from web-hosted service: {0}", response);
        }
    }
}
