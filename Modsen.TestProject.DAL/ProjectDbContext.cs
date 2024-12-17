using Modsen.TestProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Modsen.TestProject.DAL
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options)
        {
        }

        public DbSet<NewEventEntity> NewEvents { get; set; }
        public DbSet<ParticipantEntity> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NewEventEntity>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired(); 
            });

            modelBuilder.Entity<ParticipantEntity>()
                .HasOne(p => p.NewEvent)
                .WithMany(e => e.Participants)
                .HasForeignKey(p => p.NewEventId);
        }
    }
}
