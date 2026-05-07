using Microsoft.AspNetCore.Mvc;
using TrainingRoomsApi.Data;
using TrainingRoomsApi.Models;
namespace TrainingRoomsApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetReservations(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var reservations = AppData.Reservations.AsEnumerable();
        if (date.HasValue)
        {
            reservations = reservations.Where(r => r.Date == date.Value);
        }
        if (!string.IsNullOrWhiteSpace(status))
        {
            reservations = reservations.Where(r =>
                r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }
        if (roomId.HasValue)
        {
            reservations = reservations.Where(r => r.RoomId == roomId.Value);
        }
        return Ok(reservations);
    }
    [HttpGet("{id:int}")]
    public ActionResult<Reservation> GetReservationById([FromRoute] int id)
    {
        var reservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
        {
            return NotFound($"Rezerwacja o id {id} nie istnieje.");
        }
        return Ok(reservation);
    }
    [HttpPost]
    public ActionResult<Reservation> CreateReservation([FromBody] Reservation reservation)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room is null)
        {
            return BadRequest($"Sala o id {reservation.RoomId} nie istnieje.");
        }
        if (!room.IsActive)
        {
            return BadRequest("Nie można dodać rezerwacji dla sali, która jest nieaktywna.");
        }
        if (HasTimeConflict(reservation))
        {
            return Conflict("Rezerwacja koliduje czasowo z istniejącą rezerwacją");
        }
        reservation.Id = AppData.Reservations.Any()
            ? AppData.Reservations.Max(r => r.Id) + 1
            : 1;

        AppData.Reservations.Add(reservation);
        return CreatedAtAction(
            nameof(GetReservationById),
            new { id = reservation.Id },
            reservation
        );
    }
    [HttpPut("{id:int}")]
    public ActionResult<Reservation> UpdateReservation([FromRoute] int id, [FromBody] Reservation updatedReservation)
    {
        var existingReservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);
        if (existingReservation is null)
        {
            return NotFound($"Rezerwacja o id {id} nie istnieje.");
        }
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);
        if (room is null)
        {
            return BadRequest($"Sala o id {updatedReservation.RoomId} nie istnieje.");
        }
        if (!room.IsActive)
        {
            return BadRequest("Nie można dodać rezerwacji dla sali, która jest nieaktywna.");
        }
        if (HasTimeConflict(updatedReservation, id))
        {
            return Conflict("Rezerwacja koliduje czasowo z istniejącą rezerwacją");
        }
        existingReservation.RoomId = updatedReservation.RoomId;
        existingReservation.OrganizerName = updatedReservation.OrganizerName;
        existingReservation.Topic = updatedReservation.Topic;
        existingReservation.Date = updatedReservation.Date;
        existingReservation.StartTime = updatedReservation.StartTime;
        existingReservation.EndTime = updatedReservation.EndTime;
        existingReservation.Status = updatedReservation.Status;
        return Ok(existingReservation);
    }
    [HttpDelete("{id:int}")]
    public IActionResult DeleteReservation([FromRoute] int id)
    {
        var reservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
        {
            return NotFound($"Rezerwacja o id {id} nie istnieje.");
        }
        AppData.Reservations.Remove(reservation);
        return NoContent();
    }
    private static bool HasTimeConflict(Reservation newReservation, int? ignoredReservationId = null)
    {
        return AppData.Reservations.Any(existingReservation =>
            existingReservation.Id != ignoredReservationId &&
            existingReservation.RoomId == newReservation.RoomId &&
            existingReservation.Date == newReservation.Date &&
            existingReservation.Status.ToLower() != "cancelled" &&
            newReservation.Status.ToLower() != "cancelled" &&
            newReservation.StartTime < existingReservation.EndTime &&
            newReservation.EndTime > existingReservation.StartTime
        );
    }
}