using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Services  // handles bussiness logic
{
    // handles payment processing and management for Hotel bookings
    // implements the business logic for handling payments.
    public class PaymentService : IPayment
    {
        private readonly SHMSContext _context;

        public PaymentService(SHMSContext context)
        {
            _context = context;
        }

        // get all payments with user and booking detail
        public IEnumerable<Payment> GetAllPayments()
        {
            return _context.Payments
                .Include(p => p.User)
                .Include(p => p.Booking)
                .ToList();

            //uses Include to load navigation properties (User and Booking)
        }

        // get payment by id
        public Payment? GetPaymentById(int id)
        {
            return _context.Payments
                .Include(p => p.User)
                .Include(p => p.Booking)
                .FirstOrDefault(p => p.PaymentID == id);
        }

       
        // get all payment as particular user
        public IEnumerable<Payment> GetPaymentsByUser(int userId)
        {
            return _context.Payments
                .Include(p => p.Booking)
                .Where(p => p.UserID == userId)
                .ToList();
        }

        // process new payment check condition
        public async Task<string> AddPaymentAsync(Payment payment)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.BookingID == payment.BookingID); //fetch booking detail

            //check payment amount and room price same or not then status shuffle
            if (payment.Amount != booking.Room.Price)  

            {
                throw new InvalidOperationException($"Payment amount must match the room price. Expected: {booking.Room.Price}, Received: {payment.Amount}");
            }
            else
            {

            // Check if payment status is "Done" and update booking status

                payment.Status = true; //payment sucess then status set true 
                booking.Room.Availability = false;
                _context.Entry(booking.Room).State = EntityState.Modified;
            }

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // update booking status on successful payment

            if (payment.Status)
            {
                await UpdateBookingStatusAsync(payment.BookingID, "Confirmed");
            }
            return "Payment Successful";

        }

        // update payment sucess then confirmed status
        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Entry(payment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            if (payment.Status)
            {
                await UpdateBookingStatusAsync(payment.BookingID, "Confirmed");  
            }
        }

        //delete payment record by id
        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        // update booking status "Confirmed" after payment sucess
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



