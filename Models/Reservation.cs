using System.ComponentModel.DataAnnotations;

namespace TrainingRoomsApi.Models;

public class Reservation : IValidatableObject
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Id sali musi być większe od zera.")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Imię i nazwisko organizatora jest wymagane.")]
    [MinLength(1, ErrorMessage = "Organizator nie może być pusty.")]
    public string OrganizerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Temat rezerwacji jest wymagany.")]
    [MinLength(1, ErrorMessage = "Temat nie może być pusty.")]
    public string Topic { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    [Required(ErrorMessage = "Status rezerwacji jest wymagany.")]
    public string Status { get; set; } = "planned";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "Godzina zakończenia musi być późniejsza niż godzina rozpoczęcia.",
                new[] { nameof(EndTime), nameof(StartTime) }
            );
        }
    }
}