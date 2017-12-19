using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LSKYInOut
{
    public class Settings
    {
        public static string DBConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["InOutDatabase"].ConnectionString; }
        }
    }
}