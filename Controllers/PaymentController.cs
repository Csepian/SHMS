using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPayment _paymentService;

        public PaymentsController(IPayment paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/Payments
        [HttpGet]
        public ActionResult<IEnumerable<Payment>> GetAllPayments()
        {
            var payments = _paymentService.GetAllPayments();
            return Ok(payments);
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public ActionResult<Payment> GetPaymentById(int id)
        {
            var payment = _paymentService.GetPaymentById(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        // GET: api/Payments/User/5
        [HttpGet("User/{userId}")]
        public ActionResult<IEnumerable<Payment>> GetPaymentsByUser(int userId)
        {
            var payments = _paymentService.GetPaymentsByUser(userId);
            return Ok(payments);
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<IActionResult> PostPayment(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _paymentService.AddPaymentAsync(payment);
                return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentID }, payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.PaymentID)
            {
                return BadRequest("Payment ID mismatch.");
            }

            await _paymentService.UpdatePaymentAsync(payment);
            return NoContent();
        }

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            await _paymentService.DeletePaymentAsync(id);
            return NoContent();
        }
    }
}
