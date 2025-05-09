using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Services
{
    public class PaymentService : IPayment
    {
        private readonly SHMSContext _context;

        public PaymentService(SHMSContext context)
        {
            _context = context;
        }

        public IEnumerable<Payment> GetAllPayments()
        {
            return _context.Payments
                .Include(p => p.User)
                .Include(p => p.Booking)
                .ToList();
        }

        public Payment? GetPaymentById(int id)
        {
            return _context.Payments
                .Include(p => p.User)
                .Include(p => p.Booking)
                .FirstOrDefault(p => p.PaymentID == id);
        }

        public IEnumerable<Payment> GetPaymentsByUser(int userId)
        {
            return _context.Payments
                .Include(p => p.Booking)
                .Where(p => p.UserID == userId)
                .ToList();
        }

        public async Task<string> AddPaymentAsync(Payment payment)
        {
            var booking = await _context.Bookings
        .Include(b => b.Room) // Include the associated room
        .FirstOrDefaultAsync(b => b.BookingID == payment.BookingID);

            if (payment.Amount != booking.Room.Price)
            {
                throw new InvalidOperationException($"Payment amount must match the room price. Expected: {booking.Room.Price}, Received: {payment.Amount}");
            }
            else
            {
                payment.Status = true; // Set status to true for successful payment
            }
                
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            // Check if payment status is "Done" and update booking status
            if (payment.Status)
            {
                await UpdateBookingStatusAsync(payment.BookingID, "Confirmed");
            }
            return "Payment Successful";

            
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Entry(payment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Check if payment status is "Done" and update booking status
            if (payment.Status)
            {
                await UpdateBookingStatusAsync(payment.BookingID, "Confirmed");
            }
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
        private async Task UpdateBookingStatusAsync(int bookingId, string status)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                booking.Status = status;
                _context.Entry(booking).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
