using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.DTOs;
using SHMS.Model;
using SHMS.Repositories;
using SHMS.Services;

namespace SHMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotel _hotelservice;

        public HotelsController(IHotel context)
        {
            _hotelservice = context;
        }

        // GET: api/Hotels
        [HttpGet]
        public ActionResult<IEnumerable<Hotel>> GetHotels()
        {
            return Ok(_hotelservice.GetHotels());
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotelByID(int id)
        {
            try
            {

                var hotel = _hotelservice.GetHotelById(id);
                if (hotel == null)
                {
                    return NotFound();
                }
                return Ok(hotel);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("ByName/{name}")]
        public async Task<ActionResult<Hotel>> GetHotelByName(string name)
        {
            try
            {

                var hotel = _hotelservice.GetHotelByName(name);
                if (hotel == null)
                {
                    return NotFound($"No hotel found with the name '{name}'.");
                }
                return Ok(hotel);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            }


        // PUT: api/Hotels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, Hotel hotel)
        {
            if (id != hotel.HotelID)
            {
                return BadRequest("Hotel ID mismatch.");
            }

            try
            {
                await _hotelservice.UpdateHotelAsync(hotel);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_hotelservice.HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }



        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = "manager")] 
        public async Task<ActionResult<Hotel>> PostHotel(HotelDTO hoteldto)
        {
            //// Extract UserID from JWT claims
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            //if (userIdClaim == null)
            //{
            //    return Unauthorized("User ID not found in token.");
            //}
            //int managerId = int.Parse(userIdClaim.Value);

            var hotel = new Hotel
            {
                Name = hoteldto.Name,
                Location = hoteldto.Location,
                ManagerID = hoteldto.ManagerID, 
 // ManagerID = managerId, // Set automatically from logged-in user
                Amenities = hoteldto.Amenities
            };

            try
            {
                await _hotelservice.AddHotelAsync(hotel);
                return CreatedAtAction(nameof(GetHotelByID), new { id = hotel.HotelID }, hotel);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            try
            {
                if (!_hotelservice.HotelExists(id))
                {
                    return NotFound();
                }

                await _hotelservice.DeleteHotelAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            }
        [HttpGet("Search")]
        public ActionResult<IEnumerable<Hotel>> SearchHotels(string? location, string? amenities)
        {
            var hotels = _hotelservice.SearchHotels(location, amenities).ToList();
            return Ok(hotels);
        }

        [HttpGet("AvailableHotels")]
        public async Task<ActionResult<IEnumerable<object>>> GetHotelsWithAvailableRooms()
        {
            var hotels = await _hotelservice.GetHotelsWithAvailableRoomsAsync();

            if (!hotels.Any())
            {
                return NotFound("No hotels with available rooms found.");
            }

            return Ok(hotels);
        }

    }
}