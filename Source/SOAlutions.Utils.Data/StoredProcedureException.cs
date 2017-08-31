// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data.Common;

namespace SOAlutions.Utils.Data
{
	public class StoredProcedureException : ApplicationException
	{
		private IDbCommand cmd;
		private Exception baseException;

		public StoredProcedureException(IDbCommand cmd, Exception baseException)
			: base(baseException.Message, baseException)
		{
			this.cmd = cmd;
			this.baseException = baseException;
		}

		public override string Message
		{
			get
			{
#if DEBUG
				return GetCommandText(this.cmd);
#else
				return this.baseException.Message;
#endif
			}
		}

		public override string StackTrace
		{
			get
			{
				return String.Format("{0}\r\n{1}", GetCommandText(this.cmd), base.StackTrace);
			}
		}

		public override string ToString()
		{
			return String.Format("{0} [{1}]", base.ToString(), GetCommandText(this.cmd));
		}

		private string GetCommandText(IDbCommand command)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("[Stored procedure executed: {0}", command.CommandText);
			int baseLength = sb.Length;

			foreach (DbParameter parameter in command.Parameters)
			{
				if (sb.Length > baseLength)
				{
					sb.Append(",");
				}
				object val = "NULL";
				string paramSymbol = parameter.ParameterName.StartsWith("@") ? String.Empty : "@";

				if (parameter.Value != DBNull.Value && parameter.Value != null)
				{
					val = parameter.Value;
				}

				if (parameter.DbType == DbType.Int16
					|| parameter.DbType == DbType.Int32
					|| parameter.DbType == DbType.Int64
					|| parameter.DbType == DbType.UInt16
					|| parameter.DbType == DbType.UInt32
					|| parameter.DbType == DbType.UInt64
					|| parameter.DbType == DbType.Decimal
					|| parameter.DbType == DbType.Double
					|| parameter.DbType == DbType.Single)
				{
					sb.AppendFormat(" {2}{0}={1}", parameter.ParameterName, val, paramSymbol);
				}
				else if (parameter.DbType == DbType.Boolean)
				{
					if (parameter.Value != DBNull.Value)
					{
						if (Convert.ToBoolean(parameter.Value))
						{
							val = "1";
						}
						else
						{
							val = "0";
						}
					}

					sb.AppendFormat(" {2}{0}={1}", parameter.ParameterName, val, paramSymbol);
				}
				else
				{
					sb.AppendFormat(" {2}{0}='{1}'", parameter.ParameterName, val, paramSymbol);
				}
			}
			sb.AppendFormat("]");
			return sb.ToString();
		}
	}
}