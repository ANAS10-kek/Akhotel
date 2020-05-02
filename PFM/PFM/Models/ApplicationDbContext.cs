using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PFM.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DBCn", throwIfV1Schema: false)
        {
        }
        public virtual DbSet<MailViewModel> Mails { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
    }
}