namespace Modsen.TestProject.DAL.Entities
{
    public class ParticipantEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; }

        public Guid NewEventId { get; set; }

        public NewEventEntity NewEvent { get; set; }
    }
}
