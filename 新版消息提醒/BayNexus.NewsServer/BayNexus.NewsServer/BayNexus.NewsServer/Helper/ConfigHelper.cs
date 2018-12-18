using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayNexus.NewsServer.Helper
{
    public static class ConfigHelper
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }
        public static string GetServiceAddress
        {
            get { return Get("ServiceAddress"); }
        }

        public static string GetMessageDelayMin
        {
            get { return Get("MessageDelayMin"); }
        }

    }
}
