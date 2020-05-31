using System.Collections.Generic;

namespace PFM.Models.ModelsReservation
{
    public class Room
    {
        public int ChambreId { get; set; }
        public string Titre { get; set; }

        public int? ImageId { get; set; }

        public float Prix { get; set; }

        public string TypeDeLit { get; set; }

        public int  Disponibilité { get; set; }
        public int NbChambres { get; set; }

        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        public ICollection<RoomImage> RoomImages { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

        public ICollection<Caracteristique> Caracteristiques { get; set; }

    }
}