using System;
using System.Data.Entity;
using Cinema.Models.Domain;
using EntityFramework.DynamicFilters;

namespace Cinema.DataBaseLayer
{
    public class CinemaContext : DbContext
    {
        public CinemaContext() : base("Cinema")
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Hall> Halls { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Filter("IsDeleted", (BaseEntity x) => x.IsDeleted, false);

            modelBuilder.Entity<TimeSlot>()
                    .HasRequired(x => x.Movie)
                        .WithMany(x => x.Timeslots);

            modelBuilder.Entity<Movie>()
                    .HasMany(x => x.Timeslots)
                         .WithRequired(x => x.Movie);
        }
    }
}