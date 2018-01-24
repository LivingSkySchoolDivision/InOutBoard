﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYInOut
{
    public class UserStatus
    {
        public Status Status { get; set; }
        public DateTime Expires { get; set; }
        public UserStatus()
        {
            this.Status = new Status();
            this.Expires = DateTime.MinValue;
        }

        public override string ToString()
        {
            return this.Status.ToString();
        }
    }    
}