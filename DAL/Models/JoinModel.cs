using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class JoinModel
    {
        [Required]
        public int RecordId { get; set; }

        /// <summary>
        /// Current Record Review.
        /// </summary>
        public string? RecordReview { get; set; }

        public string? Author { get; set; }

        /// <summary>
        /// Pitchforh Review.
        /// </summary>
        public string? Review { get; set; }
    }
}
