using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Linq;

namespace Core.Personal
{
    public class ConfigUtil
    {
        public static T Get<T>(string key, T defaultValue)
        {
            var result = defaultValue;

            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                try
                {
                    var configVal = ConfigurationManager.AppSettings[key];
                    result = (T)Convert.ChangeType(configVal, typeof(T));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }

            return result;
        }

        public static T Get<T>(string key)
        {
            return Get(key, default(T));
        }
    }
}
