using MovieSearch.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.BL.Models
{
    public class MovieGetByIdResponse: Movie
    {
        public bool Response { get; set; }
    }
}
