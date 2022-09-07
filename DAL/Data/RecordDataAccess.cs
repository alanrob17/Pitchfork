using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Dapper;
using DAL.Models;
using DAL.Components;

namespace DAL.Data
{
    public class RecordDataAccess
    {
        /// <summary>
        /// Insert a new record.
        /// </summary>
        /// <param name="review">The review object.</param>
        /// <returns>The <see cref="int"/> Pitchfork Id.</returns>
        public int Insert(ReviewModel review)
        {
            var pitchforkId = -1; // 0 is used for record is already in the db.

            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var cmd = new SqlCommand("adm_InsertSqliteReview", cn) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.AddWithValue("ReviewId", review.ReviewId);
                cmd.Parameters.AddWithValue("ArtistId", 0);
                cmd.Parameters.AddWithValue("RecordId", 0);
                cmd.Parameters.AddWithValue("Name", review.Name);
                cmd.Parameters.AddWithValue("RecordName", review.RecordName);
                cmd.Parameters.AddWithValue("Author", review.Author);
                cmd.Parameters.AddWithValue("Published", review.Published);
                cmd.Parameters.AddWithValue("Review", review.Review);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                using (cn)
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    pitchforkId = (int)cmd.Parameters["@ReturnValue"].Value;
                }
            }

            return pitchforkId;
        }

        /// <summary>
        /// Get all Record Reviews and Pitchfork Reviews.
        /// </summary>
        /// <returns>A List of Review objects</returns>
        public List<JoinModel> Select()
        {
            var dt = new DataTable();

            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var sql = "dbo.adm_GetSelectedReviews";
                var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.StoredProcedure };

                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            var query = from dr in dt.AsEnumerable()
                        select new JoinModel
                        {
                            RecordId = Convert.ToInt32(dr["RecordId"]),
                            RecordReview = dr["RecordReview"].ToString(),
                            Author = dr["Author"].ToString(),
                            Review = dr["Review"].ToString()
                        };

            return query.ToList();
        }

        /// <summary>
        /// Update a Record review.
        /// </summary>
        /// <param name="artist">The review.</param>
        /// <returns>The <see cref="int"/>Record Id.</returns>
        public int UpdateReview(JoinModel record)
        {
            var recordId = -1;

            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var cmd = new SqlCommand("adm_UpdateRecordReview", cn) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.AddWithValue("@RecordID", record.RecordId);
                cmd.Parameters.AddWithValue("@Review", record.RecordReview);
                var recordIdParameter = new SqlParameter("@RecordId", SqlDbType.Int, 4)
                {
                    Direction =
                        ParameterDirection
                        .ReturnValue
                };
                cmd.Parameters.Add(recordIdParameter);

                using (cn)
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    recordId = int.Parse(recordIdParameter.Value.ToString());
                }
            }

            return recordId;
        }
    }
}
