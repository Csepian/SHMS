using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Services
{
    public class BookingService : IBooking
    {
        private readonly SHMSContext _context;

        public BookingService(SHMSContext context)
        {
            _context = context;
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            return _context.Bookings
                .Include(b => b.User)
                .ThenInclude(h=>h.Hotel)
                .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                .ToList();
        }

        public IEnumerable<Booking> GetBookingsByHotel(int hotelId)
        {
            return _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Where(b => b.Room.HotelID == hotelId)
                .ToList();
        }

        public IEnumerable<Booking> GetBookingsByUser(int userId)
        {
            return _context.Bookings
                .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                .Where(b => b.UserID == userId)
                .ToList();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                .FirstOrDefaultAsync(b => b.BookingID == id);
        }

        public async Task AddBookingAsync(Booking booking)
        {
            if (!await IsRoomAvailableAsync(booking.RoomID, booking.CheckInDate, booking.CheckOutDate))
            {
                throw new InvalidOperationException($"The room is already booked for the selected dates{ booking.CheckInDate }");
            }
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomID == booking.RoomID); //fetch room detail
            room.Availability = false;
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            

            _context.Entry(booking).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<string> PatchBookingAsync(int bookingId, Booking patch)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
                return "Booking not found.";

            // Update only provided fields (example: CheckInDate, CheckOutDate, Status)
            if (patch.CheckInDate != default && patch.CheckInDate != booking.CheckInDate)
                booking.CheckInDate = patch.CheckInDate;
            if (patch.CheckOutDate != default && patch.CheckOutDate != booking.CheckOutDate)
                booking.CheckOutDate = patch.CheckOutDate;
            if (!string.IsNullOrEmpty(patch.Status) && patch.Status != booking.Status)
                booking.Status = patch.Status;
            // Add more fields as needed

            await _context.SaveChangesAsync();
            return "Booking updated successfully.";
        }


        public async Task DeleteBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
     
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            return !await _context.Bookings
                .AnyAsync(b => b.RoomID == roomId &&
                               b.Status == "Confirmed" &&
                               ((checkInDate >= b.CheckInDate && checkInDate < b.CheckOutDate) ||
                                (checkOutDate > b.CheckInDate && checkOutDate <= b.CheckOutDate) ||
                                (checkInDate <= b.CheckInDate && checkOutDate >= b.CheckOutDate)));
        }
        public async Task<bool> CanCancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return false;
            }

            // Check if the current time is at least 23 hours before the check-in date
            var now = DateTime.Today;
            return now < booking.CheckInDate.AddHours(-23);
        }
        public async Task<string> CancelBookingAsync(int bookingId)
        {
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.Room)
                    .FirstOrDefaultAsync(b => b.BookingID == bookingId);

                if (booking == null)
                    return "Booking not found.";

                if (!await CanCancelBookingAsync(bookingId))
                    return "You can't cancel booking within 24 hours of check-in date. Please contact hotel manager";

                booking.Status = "Cancelled";
                booking.Room.Availability = true;

                await _context.SaveChangesAsync();
                return "Booking cancelled successfully.";
            }
            catch (Exception)
            {
                return "An error occurred while cancelling the booking.";
            }
        }


    }
}
