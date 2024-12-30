using Modsen.TestProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.DAL.Configurations;

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

            modelBuilder.ApplyConfiguration(new NewEventConfiguration());
            modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
        }
    }
}
