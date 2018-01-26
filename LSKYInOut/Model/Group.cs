using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYInOut
{
    public class Group
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsHidden { get; set; }
        public bool IsSystemAdministrator { get; set; }
    }
}