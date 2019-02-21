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

        public Person()
        {
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
            };
        }
    }
}
