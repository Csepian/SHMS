using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var hotel = _hotelservice.GetHotelById(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
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

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(HotelDTO hoteldto)
        {
            var hotel = new Hotel
            {

                Name = hoteldto.Name,
                Location = hoteldto.Location,
                ManagerID = hoteldto.ManagerID,
                Amenities = hoteldto.Amenities

            };
            await _hotelservice.AddHotelAsync(hotel);
            return CreatedAtAction(nameof(GetHotelByID), new { id = hotel.HotelID }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (!_hotelservice.HotelExists(id))
            {
                return NotFound();
            }

            await _hotelservice.DeleteHotelAsync(id);
            return NoContent();
        }
        [HttpGet("Search")]
        public ActionResult<IEnumerable<Hotel>> SearchHotels(string? location, string? amenities)
        {
            var hotels = _hotelservice.SearchHotels(location, amenities).ToList();
            return Ok(hotels);
        }

    }
}
