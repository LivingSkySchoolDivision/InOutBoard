using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYInOut
{
    public class TrackedUser
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NavisionEmployeIDNumber { get; set; }
        public string LDAPUserName { get; set; }
        public bool IsHidden { get; set; }
        List<Group> Groups { get; set; }
    }
}