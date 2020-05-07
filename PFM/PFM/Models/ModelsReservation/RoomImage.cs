using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFM.Models.ModelsReservation
{
    public class RoomImage
    {
        public int ImageId { get; set; }

        public string Name { get; set; }

        public string FullPath { get; set; }

        public virtual Room Room { set; get; }

        public int RoomId { get; set; }

    }
}