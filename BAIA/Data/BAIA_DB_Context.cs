using BAIA.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Data
{
    public class BAIA_DB_Context : DbContext
    {
        public BAIA_DB_Context(DbContextOptions<BAIA_DB_Context> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceDetail> ServiceDetails { get; set; }
        public DbSet<UserStory> UserStories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Project>()
            .HasOne(p => p.User)
            .WithMany(u => u.Projects);
            

            /*modelBuilder.Entity<User>()
                .HasMany(u => u.Projects)
                .WithOne();

            modelBuilder.Entity<User>()
                .Navigation(u => u.Projects)
                .UsePropertyAccessMode(PropertyAccessMode.Property);


            //------------------------------------------------------------

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Meetings)
                .WithOne();

            modelBuilder.Entity<Project>()
                .Navigation(p => p.Meetings)
                .UsePropertyAccessMode(PropertyAccessMode.Property);


            //------------------------------------------------------------

            modelBuilder.Entity<Meeting>()
                .HasMany(m => m.Services)
                .WithOne();

            modelBuilder.Entity<Meeting>()
                .Navigation(m => m.Services)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Meeting>()
                .HasMany(m => m.UserStories)
                .WithOne();

            modelBuilder.Entity<Meeting>()
                .Navigation(m => m.UserStories)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            //------------------------------------------------------------

            modelBuilder.Entity<Service>()
                .HasMany(s => s.ServiceDetails)
                .WithOne();
            
            modelBuilder.Entity<Service>()
                .Navigation(s => s.ServiceDetails)
                .UsePropertyAccessMode(PropertyAccessMode.Property);*/



        }



    }
}
