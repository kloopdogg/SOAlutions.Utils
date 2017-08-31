// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

using System;
using System.Configuration;

namespace SOAlutions.Utils
{
    public static class ConfigurationHelper
    {
        public static bool GetBooleanSetting(string key)
        {
            return GetBooleanSetting(key, false);
        }

        public static bool GetBooleanSetting(string key, bool defaultValue)
        {
            bool val;
            if (!Boolean.TryParse(GetAppSetting(key), out val))
            {
                val = defaultValue;
            }
            return val;
        }

        public static int GetIntSetting(string key)
        {
            return GetIntSetting(key, 0);
        }

        public static int GetIntSetting(string key, int defaultValue)
        {
            int val;
            if (!Int32.TryParse(GetAppSetting(key), out val))
            {
                val = defaultValue;
            }
            return val;
        }

        public static T GetAppSetting<T>(string key) where T : struct
        {
            return GetAppSetting<T>(key, default(T));
        }

        public static T GetAppSetting<T>(string key, T defaultValue) where T : struct
        {
            string temp = ConfigurationManager.AppSettings[key];
            T result;
            TypeParser<T>.TryParse(temp, out result, defaultValue);
            return result;
        }

        public static string GetAppSetting(string key)
        {
            return GetAppSetting(key, string.Empty);
        }

        public static string GetAppSetting(string key, string defaultValue)
        {
            string val = ConfigurationManager.AppSettings[key];

            if (val == null)
                val = defaultValue;

            return val;
        }
    }
}