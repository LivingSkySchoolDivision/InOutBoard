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
        public List<Group> Groups { get; set; }
        public List<int> GroupIDs { get; set; }
        public Status Status { get; set; }


        public string DisplayName { get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        public string DisplayNameLastNameFirst
        {
            get
            {
                return this.LastName + ", " + this.FirstName;
            }
        }

        public override string ToString()
        {
            return this.DisplayName + ": " + this.Status;
        }
    }
}