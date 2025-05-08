using SHMS.Model;

namespace SHMS.Repositories
{
    public interface IPayment
    {
        IEnumerable<Payment> GetAllPayments();
        Payment? GetPaymentById(int id);
        IEnumerable<Payment> GetPaymentsByUser(int userId);
        Task AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(int id);
    }
}
