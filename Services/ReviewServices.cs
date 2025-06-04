using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SHMS.Data;
using SHMS.Model;
using SHMS.Repositories;

namespace SHMS.Services
{
    public class ReviewServices : IReview
    {
        private readonly SHMSContext _context;

        public ReviewServices(SHMSContext context)
        {
            _context = context;
        }

        public IEnumerable<Review> GetReviewsByHotel(Hotel hotel)
        {
            return _context.Reviews
                .Include(r => r.User)
                .Where(r => r.HotelID == hotel.HotelID)
                .ToList();
        }
        public IEnumerable<Review> GetAllReviews()
        {
            return _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Hotel)
                .ToList();
        }

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            await UpdateHotelRatingAsync(review.HotelID);
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await UpdateHotelRatingAsync(review.HotelID);
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                int hotelId = review.HotelID;
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                await UpdateHotelRatingAsync(hotelId);
            }
        }

        private async Task UpdateHotelRatingAsync(int hotelId)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Reviews)
                .FirstOrDefaultAsync(h => h.HotelID == hotelId);

            if (hotel != null)
            {
                // Calculate the average rating
                hotel.Rating = hotel.Reviews.Any()
                    ? hotel.Reviews.Average(r => r.Rating)
                    : (double?)null;

                _context.Entry(hotel).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
