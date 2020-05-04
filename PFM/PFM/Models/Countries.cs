using System.Collections.Generic;

namespace PFM.Models
{
    public class Countries
    {

        public double id { get; set; }
        public string sortname { get; set; }
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<States> States { get; set; }
    }
}