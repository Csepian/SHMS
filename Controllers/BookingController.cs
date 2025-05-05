using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBooking _bookingService;

        public BookingsController(IBooking bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/Bookings
        [HttpGet]
        public ActionResult<IEnumerable<Booking>> GetAllBookings()
        {
            var bookings = _bookingService.GetAllBookings();
            return Ok(bookings);
        }

        // GET: api/Bookings/Hotel/5
        [HttpGet("Hotel/{hotelId}")]
        public ActionResult<IEnumerable<Booking>> GetBookingsByHotel(int hotelId)
        {
            var bookings = _bookingService.GetBookingsByHotel(hotelId);
            return Ok(bookings);
        }

        // GET: api/Bookings/User/5
        [HttpGet("User/{userId}")]
        public ActionResult<IEnumerable<Booking>> GetBookingsByUser(int userId)
        {
            var bookings = _bookingService.GetBookingsByUser(userId);
            return Ok(bookings);
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        // POST: api/Bookings
        [HttpPost]
        public async Task<IActionResult> PostBooking(Booking booking)
        {
            try
            {
                await _bookingService.AddBookingAsync(booking);
                return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingID }, booking);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingID)
            {
                return BadRequest("Booking ID mismatch.");
            }

            try
            {
                await _bookingService.UpdateBookingAsync(booking);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return NoContent();
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            await _bookingService.DeleteBookingAsync(id);
            return NoContent();
        }
    }
}
