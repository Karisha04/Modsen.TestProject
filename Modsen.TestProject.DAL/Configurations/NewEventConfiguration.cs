using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modsen.TestProject.DAL.Entities;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.DAL.Configurations
{
    public class NewEventConfiguration : IEntityTypeConfiguration<NewEventEntity>
    {
        public void Configure(EntityTypeBuilder<NewEventEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(b => b.Name)
                .HasMaxLength(NewEvent.MAX_NAME_LENGTH)
                .IsRequired();
            builder.Property(b => b.Description)
                .IsRequired();
            builder.Property(b => b.DateAndTime)
                .IsRequired();
            builder.Property(b => b.Place)
                .IsRequired();
            builder.Property(b => b.Category)
                .IsRequired();
            builder.Property(b => b.MaxParticipant)
                .IsRequired();
            builder.Property(b => b.Participants)
                .IsRequired();
            builder.Property(b => b.ImagePath)
                .IsRequired();

        }
    }
}
