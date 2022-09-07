using DAL.Data;
using DAL.Models;
using Pitchfork.BLL;
using System.Text;

namespace Pitchfork
{
    public class ReviewData
    {
        public static void Main(string[] args)
        {
            // InsertReviews();
            JoinReviews();
        }

        private static void JoinReviews()
        {
            List<JoinModel> reviews = new List<JoinModel>();

            RecordDataAccess records = new RecordDataAccess();

            reviews = records.Select();

            foreach (var review in reviews)
            {
                // Add Record review and Pitchfork review together
                Utility.JoinReview(review);

                // Insert into Db
                int recordId = records.UpdateReview(review);
                Console.WriteLine(recordId);


                review.RecordReview = Utility.RemoveLineBreaks(review.RecordReview);
                Console.WriteLine($"{review.RecordId}\n{review.RecordReview}");

            }

            // Write script to upload records to Azure
            Utility.WriteAzureScript(reviews);
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