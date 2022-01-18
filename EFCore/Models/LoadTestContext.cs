using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Models
{
    public class LoadTestContext: DbContext
    {
        public LoadTestContext() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=localhost;Database=LoadTest_EFCore;User Id=loadtest;Password=loadtest;");
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasMany(x => x.KnownForTitles)
                .WithOne()
                .HasForeignKey(x => x.NConst);

            modelBuilder.Entity<Person>()
                .HasMany(x => x.PrimaryProfession)
                .WithOne()
                .HasForeignKey(x => x.NConst);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Profession> Professions { get; set;}
    }
}
