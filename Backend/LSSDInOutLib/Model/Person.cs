using System;
using System.Collections.Generic;
using System.Text;

namespace LSSDInOutLib.Model
{
    public class Person
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }                
        public bool IsEnabled { get; set; }
        public List<int> GroupsIDs { get; set; }
        public Status CurrentStatus { get; set; }
        public bool HasStatus { get
            {
                return this.CurrentStatus != null;
            }
        }
                

        public string DisplayName
        {
            get
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

        public Person()
        {
            this.GroupsIDs = new List<int>();
        }
        
    }
}
