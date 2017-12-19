using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYInOut.Model
{
    public class UserStatus
    {
        public int UserID { get; set; }
        public int StatusID { get; set; }
        public DateTime Expires { get; set; }
    }
}