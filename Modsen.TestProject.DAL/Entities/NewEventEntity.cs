using System.ComponentModel.DataAnnotations;

namespace Modsen.TestProject.DAL.Entities
{
    public class NewEventEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Place { get; set; }
        public string Category { get; set; }
        public int MaxParticipant { get; set; }
        public string ImagePath { get; set; }
        public ICollection<ParticipantEntity> Participants { get; set; }

    }
}
