using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SHMS.DTO;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowReactApp")]
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
        [HttpPost("{roomId}")]
        //[Authorize(Roles = "admin,user")]
        public async Task<IActionResult> PostBooking(int roomId, BookingDTO bookingdto)
        {
            try
            {
                // Validate that CheckInDate and CheckOutDate are not in the past
                if (bookingdto.CheckInDate < DateTime.UtcNow.Date)
                {
                    return BadRequest("Check-in date cannot be in the past.");
                }

                if (bookingdto.CheckOutDate < DateTime.UtcNow.Date)
                {
                    return BadRequest("Check-out date cannot be in the past.");
                }

                // Validate that CheckOutDate is after CheckInDate
                if (bookingdto.CheckOutDate <= bookingdto.CheckInDate)
                {
                    return BadRequest("Check-out date must be after the check-in date.");
                }

                // Retrieve the UserID from the authenticated user's claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized("User is not authenticated.");
                }

                int userId = int.Parse(userIdClaim.Value);

                // Create a new booking object
                var booking = new Booking
                {
                    UserID = userId, // Automatically set UserID
                    RoomID = roomId, // Automatically set RoomID from the URL
                    CheckInDate = bookingdto.CheckInDate,
                    CheckOutDate = bookingdto.CheckOutDate,
                    Status = "Unconfirmed" // Default status
                };

                // Add the booking
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

        // Cancel Booking
        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var message = await _bookingService.CancelBookingAsync(id);
            if (message != "Booking cancelled successfully.")
                return BadRequest(new { message });
            return Ok(new { message });
        }


        // Delete Booking
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            await _bookingService.DeleteBookingAsync(id);
            return Ok("Booking deleted.");
        }
    }
}
