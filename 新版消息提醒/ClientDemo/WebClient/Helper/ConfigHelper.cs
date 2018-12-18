using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebClient.Helper
{
    public static class ConfigHelper
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }

        public static string GetMessageDelayMin
        {
            get { return Get("MessageDelayMin"); }
        }
    }
}