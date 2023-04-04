using Graduate_Work.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Graduate_Work.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageType> PackageTypes { get; set; }
        public DbSet<SenderInfo> SenderInfos { get; set; }
        public DbSet<ReciverInfo> ReciverInfos { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Models.Route> Routes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Package>()
                .HasOne(x => x.SenderInfo)
                .WithOne().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Package>()
                .HasOne(x => x.ReciverInfo)
                .WithOne().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
