using System;
using System.Collections.Generic;
using System.Text;

namespace LSSDInOutLib.Model
{
    public class PersonWithStatus : Person
    {        
        public Status CurrentStatus { get; set; }    

        public bool HasStatus { get
            {
                return CurrentStatus != null;
            }
        }
    }
}
