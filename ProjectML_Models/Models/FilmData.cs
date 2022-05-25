using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectML_Models.Models
{
    public partial class FilmData
    {
        [LoadColumn(0)]
        [ColumnName("imdb_title_id")]
        public string imdb_title_id { get; set; } // ID of the movie
        [LoadColumn(1)]
        [ColumnName("title")]
        public string title { get; set; } // Title of the movie
        [LoadColumn(2)]
        [ColumnName("original_title")]
        public string original_title { get; set; } // Original Title of the movie
        [LoadColumn(3)]
        [ColumnName("year")]
        public float year { get; set; } // Year of release
        [LoadColumn(4)]
        [ColumnName("date_published")]
        public DateTime date_published { get; set; } // Exact release date
        [LoadColumn(5)]
        [ColumnName("genre")]
        public string genre { get; set; } // Genre(s) of the movie
        [LoadColumn(6)]
        [ColumnName("duration")]
        public float duration { get; set; } // Duration in minutes
        [LoadColumn(7)]
        [ColumnName("country")]
        public string country { get; set; } // Country of origin
        [LoadColumn(8)]
        [ColumnName("language")]
        public string language { get; set; } // Language
        [LoadColumn(9)]
        [ColumnName("director")]
        public string director { get; set; } // Director
        [LoadColumn(10)]
        [ColumnName("writer")]
        public string writer { get; set; } //Writer
        [LoadColumn(11)]
        [ColumnName("production_company")]
        public string production_company { get; set; } // Production Company
        [LoadColumn(12)]
        [ColumnName("actors")]
        public string actors { get; set; } // Actors
        [LoadColumn(13)]
        [ColumnName("description")]
        public string description { get; set; } // Description of the movie
        [LoadColumn(14)]
        [ColumnName("avg_vote")]
        public float avg_vote { get; set; } // Average vote number
        [LoadColumn(15)]
        [ColumnName("votes")]
        public float votes { get; set; } // Total amount of votes
        [LoadColumn(16)]
        [ColumnName("reviews_from_users")]
        public float reviews_from_users { get; set; } // Written reviews by IMDB users
        [LoadColumn(17)]
        [ColumnName("reviews_from_critics")]
        public float reviews_from_critics { get; set; } // Written reviews by critics
    }
}
