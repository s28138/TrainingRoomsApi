using Microsoft.AspNetCore.Mvc;
using TrainingRoomsApi.Data;
using TrainingRoomsApi.Models;

namespace TrainingRoomsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetRooms(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var rooms = AppData.Rooms.AsEnumerable();
        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
        }
        if (hasProjector.HasValue)
        {
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
        }
        if (activeOnly == true)
        {
            rooms = rooms.Where(r => r.IsActive);
        }
        return Ok(rooms);
    }
    [HttpGet("{id:int}")]
    public ActionResult<Room> GetRoomById([FromRoute] int id)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
        {
            return NotFound($"Sala o id {id} nie istnieje.");
        }
        return Ok(room);
    }
    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetRoomsByBuildingCode([FromRoute] string buildingCode)
    {
        var rooms = AppData.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();
        return Ok(rooms);
    }
    [HttpPost]
    public ActionResult<Room> CreateRoom([FromBody] Room room)
    {
        room.Id = AppData.Rooms.Any()
            ? AppData.Rooms.Max(r => r.Id) + 1
            : 1;
        AppData.Rooms.Add(room);
        return CreatedAtAction(
            nameof(GetRoomById),
            new { id = room.Id },
            room
        );
    }
    [HttpPut("{id:int}")]
    public ActionResult<Room> UpdateRoom([FromRoute] int id, [FromBody] Room updatedRoom)
    {
        var existingRoom = AppData.Rooms.FirstOrDefault(r => r.Id == id);

        if (existingRoom is null)
        {
            return NotFound($"Sala o id {id} nie istnieje.");
        }
        existingRoom.Name = updatedRoom.Name;
        existingRoom.BuildingCode = updatedRoom.BuildingCode;
        existingRoom.Floor = updatedRoom.Floor;
        existingRoom.Capacity = updatedRoom.Capacity;
        existingRoom.HasProjector = updatedRoom.HasProjector;
        existingRoom.IsActive = updatedRoom.IsActive;
        return Ok(existingRoom);
    }
    [HttpDelete("{id:int}")]
    public IActionResult DeleteRoom([FromRoute] int id)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
        {
            return NotFound($"Sala o id {id} nie istnieje.");
        }
        var hasReservations = AppData.Reservations.Any(r => r.RoomId == id);
        if (hasReservations)
        {
            return Conflict("Nie można usunąć sali,bo są dla niej rezerwacje.");
        }
        AppData.Rooms.Remove(room);
        return NoContent();
    }
}