using SHMS.Model;

namespace SHMS.Repositories
{
    public interface IBooking
    {
        IEnumerable<Booking> GetAllBookings();
        IEnumerable<Booking> GetBookingsByHotel(int hotelId);
        IEnumerable<Booking> GetBookingsByUser(int userId);
        Task<Booking> GetBookingByIdAsync(int id);
        Task AddBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);
        Task<string> PatchBookingAsync(int bookingId, Booking patch);
        Task DeleteBookingAsync(int id);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate);
        Task<bool> CanCancelBookingAsync(int bookingId);
        Task<string> CancelBookingAsync(int bookingId);


    }
}
