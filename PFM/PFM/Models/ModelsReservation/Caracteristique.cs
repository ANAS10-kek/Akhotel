using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFM.Models.ModelsReservation
{
    public class Caracteristique
    {
        public int CaracId { get; set; }
        public string Description { get; set; }
        public int RoomId { get; set; }

        public virtual Room Room { set; get; }
    }
}