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
    }
}
