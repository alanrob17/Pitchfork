using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ReviewModel
    {
        public int PitchforkId { get; set; }

        [Required]
        public int ReviewId { get; set; }

        public int ArtistId { get; set; }

        public int RecordId { get; set; }

        public string? Name { get; set; }

        public string? RecordName { get; set; }

        public string? Author { get; set; }

        public DateTime Published { get; set; }

        public string? Review { get; set; }
    }
}
