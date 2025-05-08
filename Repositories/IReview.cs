using SHMS.Model;

namespace SHMS.Repositories
{
    public interface IReview
    {
        IEnumerable<Review> GetReviewsByHotel(Hotel hotel);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int id);
    }
}
