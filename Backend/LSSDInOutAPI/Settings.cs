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
                try
                {
                    return ConfigurationManager.ConnectionStrings["InOutDatabase"].ConnectionString;
                } catch(Exception ex)
                {
                    throw new Exception("Missing connection string: InOutDatabase", ex);
                }
            }
        }
    }
}