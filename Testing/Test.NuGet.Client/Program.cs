using Microsoft.Practices.Unity;
using SOAlutions.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Service.DataAccess;

namespace Test.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.WriteHeaderAndTitle("Test Client");
            ConsoleHelper.WriteInfo("WriteHeaderAndTitle: {0}", "Success");

            ConsoleHelper.WriteTestSection("Testing ConsoleHelper");
            ConsoleHelper.WriteInfo("WriteTestSection: {0}", "Success");

            ConsoleHelper.WriteError("Testing WriteError");
            ConsoleHelper.WriteInfo("WriteError: {0}", "Success");

            ConsoleHelper.WriteTestStep("Testing WriteTestStep");
            ConsoleHelper.WriteInfo("WriteTestStep: {0}", "Success");

            ConsoleHelper.WriteTestStep("Testing GetArgumentValue");
            var result = ConsoleHelper.GetArgumentValue(args, "-test"); // arguments set in project Debug properties
            ConsoleHelper.WriteInfo("GetArgumentValue: {0}", result);

            ConsoleHelper.WriteTestSection("Testing UnityHelper");

            ConsoleHelper.WriteTestStep("Testing ConfigureUnityContainer");
            IUnityContainer container = UnityHelper.ConfigureUnityContainer();
            ConsoleHelper.WriteInfo("ConfigureUnityContainer: {0}", "Success");

            ConsoleHelper.WriteTestStep("Testing Dependency Resolution");
            var repository = container.Resolve<ISampleRepository>();
            var text = repository.GetSampleText(42);
            ConsoleHelper.WriteInfo("GetSampleText: {0}", text);

            var dbText = repository.GetSampleTextFromDatabase(2);
            ConsoleHelper.WriteInfo("GetSampleTextFromDatabase: {0}", dbText);

            var set = repository.GetSampleCollectionFromDatabase();
            ConsoleHelper.WriteInfo("GetSampleCollectionFromDatabase:", null);
            foreach (var item in set)
            {
                ConsoleHelper.WriteInfo(" - {0} {1}", item.ID, item.Value);
            }


            ConsoleHelper.WriteTestStep("Testing DeepCopy");
            RunDeepCopyTests();

            ConsoleHelper.WriteTestSection("Testing ServiceModelExtensions");

            TestManager testManager = new TestManager();
            testManager.RunTests();
            
            ConsoleHelper.WaitForAnyKey();
        }

        private static void RunDeepCopyTests()
        {
            var test = new TestObject
            {
                TestEnum = TestEnum.First,
                TestInt = 5150,
                TestString = "Hello, World!",
            };
            var copiedTest = test.Copy();

            if (copiedTest.Equals(test))
            {
                ConsoleHelper.WriteError("DeepCopy failed");
            }
            else
            {
                copiedTest.TestEnum = TestEnum.Second;
                if (copiedTest.Equals(test))
                {
                    ConsoleHelper.WriteError("Copied object isolation failed");
                }
                else
                {
                    ConsoleHelper.WriteInfo("DeepCopy: {0}", "Success");
                }
            }
        }
    }
}
