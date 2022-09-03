using DAL.Data;
using DAL.Models;

namespace Pitchfork
{
    public class ReviewData
    {
        public static void Main(string[] args)
        {
            InsertReviews();
        }

        private static void InsertReviews()
        {
            List<ReviewModel> reviews = new List<ReviewModel>();

            reviews = ReviewDataAccess.GetReviews();

            foreach (var review in reviews)
            {
                RecordDataAccess record = new RecordDataAccess();

                review.Review = review.Review.Replace('“', '"');
                review.Review = review.Review.Replace('”', '"');
                review.Review = review.Review.Replace('’', '\'');

                var reviewId = record.Insert(review);
                Console.WriteLine(reviewId);
            }
        }
    }
}