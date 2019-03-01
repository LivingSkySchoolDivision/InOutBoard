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

        public PersonWithStatus AddStatus(Status status)
        {
            return new PersonWithStatus()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                IsEnabled = this.IsEnabled,
                ID = this.ID,
                CurrentStatus = status,
                GroupsIDs = this.GroupsIDs
            };
        }
    }
}
