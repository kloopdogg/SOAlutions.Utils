using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Test.Service.DataAccess;

namespace Test.Service
{
    [ServiceContract(Name = "TestServiceContract", Namespace = "http://services.soalutions.net/testing")]
    public interface ITestService
    {
        [OperationContract]
        string TestMethod(int requestNumber);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class TestService : ITestService
    {
        [Dependency]
        public ISampleRepository SampleRepository { get; set; }

        [OperationBehavior]
        public string TestMethod(int requestNumber)
        {
            string responseText = SampleRepository.GetSampleText(requestNumber);
            return responseText;
        }
    }
}