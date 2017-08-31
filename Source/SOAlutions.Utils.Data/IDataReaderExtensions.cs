// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

using System;
using System.Data;

namespace SOAlutions.Utils.Data
{
	public static class IDataReaderExtensions
	{
		public static bool GetBoolean(this IDataReader reader, string fieldName)
		{
			return Convert.ToBoolean(reader[fieldName]);
		}

		public static char GetChar(this IDataReader reader, string fieldName)
		{
			return (char)reader[fieldName];
		}

		public static DateTime GetDateTime(this IDataReader reader, string fieldName)
		{
			DateTime dt;
			if (!DateTime.TryParse(reader[fieldName].ToString(), out dt))
			{
				dt = DateTime.MinValue;
			}
			return dt;
		}

		public static decimal GetDecimal(this IDataReader reader, string fieldName)
		{
			return Convert.ToDecimal(reader[fieldName]);
		}

		public static double GetDouble(this IDataReader reader, string fieldName)
		{
			return Convert.ToDouble(reader[fieldName]);
		}

		public static byte[] GetByteArray(this IDataReader reader, string fieldName)
		{
			return (byte[])reader[fieldName];
		}

		public static Guid GetGuid(this IDataReader reader, string fieldName)
		{
			return new Guid(reader[fieldName].ToString());
		}

		public static Int16 GetInt16(this IDataReader reader, string fieldName)
		{
			Int16 i;
			if (!Int16.TryParse(reader[fieldName].ToString(), out i))
			{
				i = default(Int16);
			}
			return i;
		}

		public static Int32 GetInt32(this IDataReader reader, string fieldName)
		{
			Int32 i;
			if (!Int32.TryParse(reader[fieldName].ToString(), out i))
			{
				i = default(Int32);
			}
			return i;
		}

		public static Int64 GetInt64(this IDataReader reader, string fieldName)
		{
			Int64 i;
			if (!Int64.TryParse(reader[fieldName].ToString(), out i))
			{
				i = default(Int64);
			}
			return i;
		}

		public static short GetShort(this IDataReader reader, string fieldName)
		{
			return (short)reader[fieldName];
		}

		public static DateTime? GetNullableDateTime(this IDataReader reader, string fieldName)
		{
			DateTime? temp = null;
			try
			{
				TypeParser<DateTime>.TryParse(reader[fieldName].ToString(), out temp);
			}
			catch { }
			return temp;
		}

		public static int? GetNullableInt(this IDataReader reader, string fieldName)
		{
			int? temp = null;
			try
			{
				temp = Convert.ToInt32(reader[fieldName]);
			}
			catch { }
			return temp;
		}

		public static decimal? GetNullableDecimal(this IDataReader reader, string fieldName)
		{
			decimal? temp = null;
			try
			{
				temp = Convert.ToDecimal(reader[fieldName]);
			}
			catch { }
			return temp;
		}

		//WARNING: Untested code
		public static T? GetNullableValue<T>(this IDataReader reader, string fieldName) where T : struct
		{
			T? val;
			TypeParser<T>.TryParse(reader[fieldName].ToString(), out val);
			return val;
		}

		public static double? GetNullableDouble(this IDataReader reader, string fieldName)
		{
			double? temp = null;
			try
			{
				temp = Convert.ToDouble(reader[fieldName]);
			}
			catch { }
			return temp;
		}

		public static string GetString(this IDataReader reader, string fieldName)
		{
			return reader[fieldName].ToString();
		}

		public static object GetValue(this IDataReader reader, string fieldName)
		{
			return reader[fieldName];
		}
	}
}
