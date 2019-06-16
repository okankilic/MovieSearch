using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieSearch.Domain.Data.Models
{
    public class Movie
    {
        [Key]
        public string ImdbId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Year { get; set; }

        public string Type { get; set; }

        public string Poster { get; set; }
    }
}
