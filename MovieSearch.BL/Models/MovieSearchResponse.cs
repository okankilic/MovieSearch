using MovieSearch.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.BL.Models
{
    public class MovieSearchResponse
    {
        public List<Movie> Search { get; set; }

        public int TotalResults { get; set; }

        public bool Response { get; set; }

        public string Error { get; set; }
    }
}
