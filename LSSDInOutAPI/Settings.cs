using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LSSDInOutAPI
{
    public static class Settings
    {
        public static string dbConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["InOutDatabase"].ConnectionString;
            }
        }
    }
}