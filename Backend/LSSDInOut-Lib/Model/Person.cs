using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.InOut.Lib.Model
{
    public class Person
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsEnabled { get; set; }
        public List<int> GroupsIDs { get; set; }
        public Status CurrentStatus { get; set; }
        public bool HasStatus
        {
            get
            {
                return CurrentStatus != null;
            }
        }


        public string DisplayName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string DisplayNameLastNameFirst
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public Person()
        {
            GroupsIDs = new List<int>();
        }

    }
}
