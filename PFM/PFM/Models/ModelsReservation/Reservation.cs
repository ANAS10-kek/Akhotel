﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFM.Models.ModelsReservation
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public int RoomId { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public bool Confirmation { get; set; }

        public int NbPers { get; set; }

        public int NbChambres { get; set; }
       
        public virtual Room Room { set; get; }
    }
}