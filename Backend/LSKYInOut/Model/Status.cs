using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYInOut
{
    public class Status
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool IsInOffice { get; set; }
        public bool IsAtWork { get; set; }
        public bool IsBusy { get; set; }
        public int SortOrder { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}