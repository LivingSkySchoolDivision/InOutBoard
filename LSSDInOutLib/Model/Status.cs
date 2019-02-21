using System;
using System.Collections.Generic;
using System.Text;

namespace LSSDInOutLib.Model
{
    public class Status
    {
        public int PersonID { get; set; }
        public DateTime Expires { get; set; }
        public string Content { get; set; }
        public StatusType StatusType { get; set; }
    }
}
