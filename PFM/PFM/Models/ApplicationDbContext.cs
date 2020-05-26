using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PFM.Models.ModelsReservation;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PFM.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DBCn", throwIfV1Schema: false)
        {
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }

        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomImage> RoomImages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Room Model sets
            modelBuilder.Entity<Room>().HasKey(r => r.ChambreId);
            modelBuilder.Entity<Room>().Property(r => r.Titre).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<Room>().Property(r => r.ImageId).IsOptional();




            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //Images Model sets
            modelBuilder.Entity<RoomImage>()
                .Property(ri => ri.Name)
                .HasMaxLength(255);

            modelBuilder.Entity<RoomImage>()
                .HasKey(ri => ri.ImageId);

            modelBuilder.Entity<RoomImage>().HasRequired(r => r.Room)
                .WithMany(ri => ri.RoomImages)
                .HasForeignKey(ri => ri.RoomId);




            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(u => new { u.RoleId, u.UserId }).ToTable("AspNetUserRoles");

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
            .ToTable("AspNetUserLogins");

            //Reservation

            modelBuilder.Entity<Reservation>().HasRequired(r => r.Room)
           .WithMany(r => r.Reservations)
           .HasForeignKey(ri => ri.RoomId);

            //Caracteristiques

            modelBuilder.Entity<Caracteristique>().HasKey(c => c.CaracId);

            modelBuilder.Entity<Caracteristique>().HasRequired(r => r.Room)
            .WithMany(r => r.Caracteristiques)
            .HasForeignKey(ri => ri.RoomId);


        }

        public System.Data.Entity.DbSet<PFM.Models.ModelsReservation.Reservation> Reservations { get; set; }

        public System.Data.Entity.DbSet<PFM.Models.ModelsReservation.Caracteristique> Caracteristiques { get; set; }

        public System.Data.Entity.DbSet<PFM.Models.ApplicationRole> IdentityRoles { get; set; }
    }
}