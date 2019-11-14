using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.InOut.Lib.Model
{
    public class Status
    {
        public int StatusID { get; set; }
        public int PersonID { get; set; }
        public DateTime Expires { get; set; }
        public string Content { get; set; }
        public StatusType StatusType { get; set; }
    }
}
