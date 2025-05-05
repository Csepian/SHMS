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
                throw new InvalidOperationException("The room is already booked for the selected dates.");
            }

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            if (!await IsRoomAvailableAsync(booking.RoomID, booking.CheckInDate, booking.CheckOutDate))
            {
                throw new InvalidOperationException("The room is already booked for the selected dates.");
            }

            _context.Entry(booking).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
    }
}
