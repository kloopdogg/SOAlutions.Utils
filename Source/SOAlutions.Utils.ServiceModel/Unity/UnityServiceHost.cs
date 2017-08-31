// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

using System;
using System.ServiceModel;
using Microsoft.Practices.Unity;

namespace SOAlutions.Utils.ServiceModel
{
	public class UnityServiceHost : ServiceHost
	{
		private static object initializationLockObject = new object();
		public IUnityContainer Container { set; get; }

		public UnityServiceHost()
			: base()
		{
			InitializeContainer();
		}

		public UnityServiceHost(Type serviceType)
			: base(serviceType)
		{
			InitializeContainer();
		}

		public UnityServiceHost(Type serviceType, params Uri[] baseAddresses)
			: base(serviceType, baseAddresses)
		{
			InitializeContainer();
		}

		private void InitializeContainer()
		{
			lock (initializationLockObject)
			{
				Container = UnityHelper.ConfigureUnityContainer();
			}
		}

		protected override void OnOpening()
		{
			if (this.Description.Behaviors.Find<UnityServiceBehavior>() == null)
				this.Description.Behaviors.Add(new UnityServiceBehavior(Container));

			base.OnOpening();
		}
	}
}