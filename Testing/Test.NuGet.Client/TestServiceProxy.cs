using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

[ServiceContract(ConfigurationName = "SampleContract", Name = "TestServiceContract", Namespace = "http://services.soalutions.net/testing")]
public interface TestServiceContract
{
    [OperationContract]
    string TestMethod(int requestNumber);
}

public class TestServiceContractClient : ClientBase<TestServiceContract>, TestServiceContract
{
    public TestServiceContractClient()
    { }

    public TestServiceContractClient(string endpointConfigurationName)
        : base(endpointConfigurationName)
    { }

    public string TestMethod(int requestNumber)
    {
        return base.Channel.TestMethod(requestNumber);
    }
}
