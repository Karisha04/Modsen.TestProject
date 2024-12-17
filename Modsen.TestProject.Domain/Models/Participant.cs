using System;

namespace Modsen.TestProject.Domain.Models
{
    public class Participant
    {
        public const int MAX_NAME_LENGTH = 100; 

        public Participant(Guid id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string email, Guid newEventId)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            RegistrationDate = registrationDate;
            Email = email;
            NewEventId = newEventId;
        }

        public Guid Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; }
        public Guid NewEventId { get; set; } 

        public static (Participant Participant, string Error) Create(Guid id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string email, Guid newEventId)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(firstName) || firstName.Length > MAX_NAME_LENGTH)
            {
                error = "First name can't be empty or longer than 100 characters.";
                return (null, error);
            }

            if (string.IsNullOrEmpty(lastName) || lastName.Length > MAX_NAME_LENGTH)
            {
                error = "Last name can't be empty or longer than 100 characters.";
                return (null, error);
            }

            if (birthDate > DateTime.Now)
            {
                error = "Birth date cannot be in the future.";
                return (null, error);
            }

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                error = "Invalid email address.";
                return (null, error);
            }

            var participant = new Participant(id, firstName, lastName, birthDate, registrationDate, email, newEventId);
            return (participant, error);
        }
    }
}
