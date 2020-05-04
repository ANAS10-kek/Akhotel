using System.Collections.Generic;

namespace PFM.Models
{
    public class States
    {
        public double id { get; set; }
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cities> Cities { get; set; }
        public virtual Countries Countries { get; set; }
    }
}