// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.ServiceModel.Description;

//namespace Util.ServiceModel
//{
//    public class UnityBehaviorAttribute : Attribute, IContractBehavior, IContractBehaviorAttribute
//    {
//        #region IContractBehaviorAttribute Members

//        public Type TargetContract
//        {
//            get { return null; }
//        }

//        #endregion

//        #region IContractBehavior Members

//        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
//        { }

//        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
//        { }

//        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime)
//        {
//            Type contractType = contractDescription.ContractType;
//            dispatchRuntime.InstanceProvider = new UnityInstanceProvider(contractType, "");
//        }

//        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
//        { }

//        #endregion
//    }
//}