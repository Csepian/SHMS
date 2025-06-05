using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHMS.DTO;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReview _reviewService;

        public ReviewsController(IReview reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/Reviews/Hotel/5
        [HttpGet("Hotel/{hotelId}")]
        public ActionResult<IEnumerable<Review>> GetReviewsByHotel(int hotelId)
        {
            var hotel = new Hotel { HotelID = hotelId }; // Simulating a hotel object
            var reviews = _reviewService.GetReviewsByHotel(hotel);
            return Ok(reviews);
        }
        // GET: api/Review
        [HttpGet]
        public ActionResult<IEnumerable<Review>> GetAllReviews()
        {
            var reviews = _reviewService.GetAllReviews();
            return Ok(reviews);
        }

        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(ReviewDTO reviewdto)
        {
            var review = new Review
            {
                HotelID = reviewdto.HotelID,
                UserID = reviewdto.UserID,
                Rating = reviewdto.Rating,
                Comment = reviewdto.Comment,
                TimeStamp = DateTime.UtcNow
            };
            await _reviewService.AddReviewAsync(review);
            return CreatedAtAction(nameof(GetReviewsByHotel), new { hotelId = review.HotelID }, review);
        }

        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.ReviewID)
            {
                return BadRequest("Review ID mismatch.");
            }

            try
            {
                await _reviewService.UpdateReviewAsync(review);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchReview(int id, [FromBody] Review patch)
        {
            var result = await _reviewService.PatchReviewAsync(id, patch);
            if (result != "Review updated successfully.")
                return BadRequest(new { message = result });
            return Ok(new { message = result });
        }

    }
}
