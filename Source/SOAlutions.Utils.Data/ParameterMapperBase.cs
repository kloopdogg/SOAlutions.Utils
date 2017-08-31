// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Globalization;
using System.Data.Common;

namespace SOAlutions.Utils.Data
{
	public class ParameterMapperBase : IParameterMapper
	{
		private Database database;

		public IDbCommand Command { get; set; }

		public ParameterMapperBase(Database database)
		{
			this.database = database;
		}

		public void AssignParameters(DbCommand command, object[] parameterValues)
		{
			if (this.Command == null)
			{
				this.Command = command;
			}

			if (parameterValues.Length > 0)
			{
				GuardParameterDiscoverySupported();
				AssignDatabaseParameters(command, parameterValues);
			}
		}

		protected virtual void AssignDatabaseParameters(DbCommand command, object[] parameterValues)
		{
			this.database.AssignParameters(command, parameterValues);
		}

		private void GuardParameterDiscoverySupported()
		{
			if (!this.database.SupportsParemeterDiscovery)
			{
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "The database type \"{0}\" does not support automatic parameter discovery. Use an IParameterMapper instead.", new object[] { this.database.GetType().FullName }));
			}
		}
	}
}
