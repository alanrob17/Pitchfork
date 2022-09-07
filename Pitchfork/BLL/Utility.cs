using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pitchfork.BLL
{
    public static class Utility
    {
        internal static void JoinReview(JoinModel review)
        {
            if (review.Review.Length > 0)
            {
                var author = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(review.Author.ToLower());
                var newReview = string.Empty;

                if (review.RecordReview.Length > 0)
                {
                    newReview = review.RecordReview + "<hr/><p>" + review.Review + "</p><p>&mdash; <strong>" + author + ", Pitchfork</strong></p>";
                }
                else
                {
                    newReview = "<p>" + review.Review + "</p><p>&mdash; <strong>" + author + ", Pitchfork</strong></p>";
                }

                review.RecordReview = newReview;
            }
        }

        internal static void WriteAzureScript(List<JoinModel> reviews)
        {
            var outFile = Environment.CurrentDirectory + "\\review-text.sql";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            foreach (var review in reviews)
            {
                var reviewText = FormatReviewText(review);

                sw.WriteLine(reviewText);
            }

            // flush and close
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// Format Review data for update command.
        /// </summary>
        /// <param name="review">The review.</param>
        /// <returns>The <see cref="string"/>review in SQL query format.</returns>
        internal static string FormatReviewText(JoinModel review)
        {
            var r = new StringBuilder();

            var newReview = RemoveLineBreaks(review.RecordReview);

            r.Append($"UPDATE [Record] SET Review = '{newReview}' WHERE RecordId = {review.RecordId};");

            return r.ToString();
        }

        internal static string RemoveLineBreaks(string newReview)
        {
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            return newReview.Replace("\r\n", " ")
                            .Replace("\n", " ")
                            .Replace("\r", " ")
                            .Replace(lineSeparator, " ")
                            .Replace(paragraphSeparator, " ");
        }
    }
}
