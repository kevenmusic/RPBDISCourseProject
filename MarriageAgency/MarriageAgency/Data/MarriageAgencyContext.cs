using MarriageAgency.Models;
using Microsoft.EntityFrameworkCore;

namespace MarriageAgency.Data;

public class MarriageAgencyContext : DbContext
{
    public DbSet<AdditionalService> AdditionalServices { get; set; }

    public DbSet<Client> Clients { get; set; }

    public DbSet<Contact> Contacts { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Nationality> Nationalities { get; set; }

    public DbSet<Photo> Photos { get; set; }

    public DbSet<PhysicalAttribute> PhysicalAttributes { get; set; }

    public DbSet<Service> Services { get; set; }

    public DbSet<ZodiacSign> ZodiacSigns { get; set; }
    public MarriageAgencyContext(DbContextOptions<MarriageAgencyContext> options) : base(options)
    {
    }
}
