// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using SOAlutions.Utils;

namespace SOAlutions.Utils.ServiceModel
{
	public class UnityServiceHostFactory : ServiceHostFactory
	{
		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			UnityServiceHost serviceHost = new UnityServiceHost(serviceType, baseAddresses);
			
			return serviceHost;
		}
	}
}