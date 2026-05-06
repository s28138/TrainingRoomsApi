using System.ComponentModel.DataAnnotations;

namespace TrainingRoomsApi.Models;

public class Room
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa sali jest wymagana.")]
    [MinLength(1, ErrorMessage = "Nazwa sali nie może być pusta.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kod budynku jest wymagany.")]
    [MinLength(1, ErrorMessage = "Kod budynku nie może być pusty.")]
    public string BuildingCode { get; set; } = string.Empty;

    public int Floor { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Pojemność sali musi być większa od zera.")]
    public int Capacity { get; set; }

    public bool HasProjector { get; set; }

    public bool IsActive { get; set; }
}