using SHMS.Model;

namespace SHMS.Repositories  //manages DB operations(CRUD)  defines only
{

    // handles Payment operations for SHMS

  

    public interface IPayment
    {
        // gets all payment records from DB
        IEnumerable<Payment> GetAllPayments();

        // finds Payment using id
        Payment? GetPaymentById(int id);

        // fetch User specific Payments from DB
        IEnumerable<Payment> GetPaymentsByUser(int userId);

        Task<string> AddPaymentAsync(Payment payment);

        // modifies existing payment info in DB

        Task UpdatePaymentAsync(Payment payment);

        // removes payment record from DB 
        Task DeletePaymentAsync(int id);

    }
}