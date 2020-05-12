using System;
using System.Collections.Generic;
using System.Text;
using DeskBooker.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeskBooker.Persistance
{
    public class DeskBookerContext : DbContext
    {
        private DeskBookerContext()
        {
            
        }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<DeskBooking> DeskBookings { get; set; }

        public DeskBookerContext(DbContextOptions<DeskBookerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Desk>().HasData(
                new Desk {Id = 1},
                new Desk {Id = 2}
            );
        }
    }
}
