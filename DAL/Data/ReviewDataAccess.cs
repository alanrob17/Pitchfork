using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using System.Configuration;

namespace DAL.Data
{
    public class ReviewDataAccess
    {
        public static List<ReviewModel> GetReviews()
        {
            using (IDbConnection cn = new SQLiteConnection(LoadConnectionString()))
            {
                var reviews = cn.Query<ReviewModel>("SELECT 0, a.reviewid As ReviewId, 0, 0, a.artist AS Name, r.title AS RecordName, r.author AS Author, r.pub_date AS Published, c.content AS Review FROM artists a inner join reviews r on a.reviewid = r.reviewid inner join content c on r.reviewid = c.reviewid", new DynamicParameters());

                return reviews.ToList();
            }

        }
        private static string LoadConnectionString(string id = "PitchforkDB")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
