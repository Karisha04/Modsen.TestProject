namespace Modsen.TestProject.Domain.Models
{
    public class NewEvent
    {
        public const int MAX_NAME_LENGTH = 300;
        public NewEvent() { }
        public NewEvent( Guid id, string name, string description, DateTime dateAndTime, string place, string category, int maxParticipant, ICollection<Participant> participants, string imagePath) 
        { 
            Id= id;
            Name = name;
            Description = description;
            DateAndTime = dateAndTime;
            Place = place;
            Category = category;
            MaxParticipant = maxParticipant;
            Participants = participants;
            ImagePath = imagePath;
        }

        public Guid Id { get; set; }
        public string Name { get;  set; }
        public string Description { get;  set;    }
        public DateTime DateAndTime { get; set; }
        public string Place { get; set; }
        public string Category { get; set; }
        public int MaxParticipant { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public string ImagePath { get; set; }
        public void UpdateImagePath(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                throw new ArgumentException("Image path cannot be null or empty.", nameof(imagePath));

            ImagePath = imagePath;
        }
        public static (NewEvent NewEvent, string Error) Create(Guid id, string name, string description, DateTime dateAndTime, string place, string category, int maxParticipant, ICollection<Participant> participants, string imagePath)
        { 
            var error = string.Empty;
            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGTH)
            {
                error = "Name can't be empty or longer than 300 symbols";
                return (null, error);
            }
            var newEvent = new NewEvent(id, name, description, dateAndTime, place, category, maxParticipant, participants, imagePath);
            return (newEvent, error);
        
        }
    }
}
