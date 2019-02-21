using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYInOut.Model
{
    public class LoginSession
    {
        public string Thumbprint { get; set; }
        public string Username { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public DateTime Starts { get; set; }
        public DateTime Ends { get; set; }
    }
}