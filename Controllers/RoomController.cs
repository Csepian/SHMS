using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoom _roomService;

        public RoomsController(IRoom roomService)
        {
            _roomService = roomService;
        }

        // GET: api/Rooms
        [HttpGet]
        public ActionResult<IEnumerable<Room>> GetRooms()
        {
            var rooms = _roomService.GetRooms();
            return Ok(rooms);
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public ActionResult<Room> GetRoomById(int id)
        {
            var room = _roomService.GetRoomById(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        // POST: api/Rooms
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            await _roomService.AddRoomAsync(room);
            return CreatedAtAction(nameof(GetRoomById), new { id = room.RoomID }, room);
        }

        // PUT: api/Rooms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.RoomID)
            {
                return BadRequest("Room ID mismatch.");
            }

            try
            {
                await _roomService.UpdateRoomAsync(room);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_roomService.RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            if (!_roomService.RoomExists(id))
            {
                return NotFound();
            }

            await _roomService.DeleteRoomAsync(id);
            return NoContent();
        }

        // GET: api/Rooms/Search
        [HttpGet("Search")]
        public ActionResult<IEnumerable<Room>> SearchRooms(string? type, decimal? minPrice, decimal? maxPrice, bool? availability)
        {
            var rooms = _roomService.SearchRooms(type, minPrice, maxPrice, availability).ToList();
            return Ok(rooms);
        }
    }
}
