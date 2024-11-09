using MarriageAgency.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace MarriageAgency.DataLayer.Data
{
    public partial class MarriageAgencyContext : DbContext
    {
        public MarriageAgencyContext(DbContextOptions<MarriageAgencyContext> options) : base(options)
        {
        }

        public MarriageAgencyContext()
        {

        }

        public virtual DbSet<AdditionalService> AdditionalServices { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<PhysicalAttribute> PhysicalAttributes { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ZodiacSign> ZodiacSigns { get; set; }
    }
}
