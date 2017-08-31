using SOAlutions.Utils;
using SOAlutions.Utils.ServiceModel;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Test.Service;
using Test.Service.DataAccess;

namespace Test.NuGet.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.WriteHeaderAndTitle("Sample Unity-Enabled WCF Service (Console-Hosted)");

            ConsoleHelper.WriteTestSection("Testing SOAlutions.Utils.ServiceModel");

            ServiceHost host = OpenHost(typeof(TestService));

            ConsoleHelper.WaitForKey(true, ConsoleKey.Enter);

            CloseHosts(host);
        }

        private static void CloseHosts(params ServiceHost[] hosts)
        {
            foreach (ServiceHost host in hosts)
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }

        private static ServiceHost OpenHost(Type serviceType)
        {
            ServiceHost host = new UnityServiceHost(serviceType);
            host.Open();

            ConsoleHelper.WriteTestStep("UnityServiceHost");
            ConsoleHelper.WriteInfo("UnityServiceHost opened for {0} on {1}", serviceType, host.BaseAddresses[0]);

            return host;
        }
    }
}
