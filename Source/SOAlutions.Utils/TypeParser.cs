// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

using System;
using System.Globalization;

namespace SOAlutions.Utils
{
	public static class TypeParser<T> where T : struct
	{
		public static bool TryParse(string s, out T result)
		{
			bool success = false;
			result = default(T);
			if (!String.IsNullOrWhiteSpace(s))
			{
				IConvertible convertableString = s as IConvertible;
				if (convertableString != null)
				{
					try
					{
						result = (T)convertableString.ToType(typeof(T), CultureInfo.CurrentCulture);
						success = true;
					}
					catch
					{ }
				}
			}
			return success;
		}

		public static bool TryParse(string s, out T result, T defaultValue)
		{
			bool success = false;
			result = default(T);
			if (String.IsNullOrWhiteSpace(s))
			{
				result = defaultValue;
				success = true;
			}
			else
			{
				IConvertible convertableString = s as IConvertible;
				if (convertableString != null)
				{
					try
					{
						result = (T)convertableString.ToType(typeof(T), CultureInfo.CurrentCulture);
						success = true;
					}
					catch
					{ }
				}
			}
			return success;
		}

		public static bool TryParse(string s, out Nullable<T> result)
		{
			bool success = false;
			result = new Nullable<T>();
			if (String.IsNullOrWhiteSpace(s))
			{
				success = true;
			}
			else
			{
				IConvertible convertableString = s as IConvertible;
				if (convertableString != null)
				{
					try
					{
						result = new Nullable<T>((T)convertableString.ToType(typeof(T), CultureInfo.CurrentCulture));
						success = true;
					}
					catch
					{ }
				}
			}
			return success;
		}
	}
}
