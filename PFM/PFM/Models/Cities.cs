using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFM.Models
{
    public class Cities
    {
        public double id { get; set; }
        public string name { get; set; }

        public virtual States States { get; set; }
    }
}