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
        public string imdb_title_id { get; set; } // Bijhorende ID van deze film
        [LoadColumn(1)]
        [ColumnName("title")]
        public string title { get; set; } // Titel van de film
        [LoadColumn(2)]
        [ColumnName("original_title")]
        public string original_title { get; set; } // Originele titel van de film
        [LoadColumn(3)]
        [ColumnName("year")]
        public float year { get; set; } // Jaar dat de film is uitgebracht
        [LoadColumn(4)]
        [ColumnName("date_published")]
        public DateTime date_published { get; set; } // Exacte datum dat de film is uitgebracht
        [LoadColumn(5)]
        [ColumnName("genre")]
        public string genre { get; set; } // Genre(s) van de film
        [LoadColumn(6)]
        [ColumnName("duration")]
        public float duration { get; set; } // Lengte van de film minuten
        [LoadColumn(7)]
        [ColumnName("country")]
        public string country { get; set; } // Land van herkomst
        [LoadColumn(8)]
        [ColumnName("language")]
        public string language { get; set; } // Taal
        [LoadColumn(9)]
        [ColumnName("director")]
        public string director { get; set; } // Regisseur
        [LoadColumn(10)]
        [ColumnName("writer")]
        public string writer { get; set; } //Schrijver
        [LoadColumn(11)]
        [ColumnName("production_company")]
        public string production_company { get; set; } // Productiehuis
        [LoadColumn(12)]
        [ColumnName("actors")]
        public string actors { get; set; } // Acteur(s)
        [LoadColumn(13)]
        [ColumnName("description")]
        public string description { get; set; } // Beschrijving van de film
        [LoadColumn(14)]
        [ColumnName("avg_vote")]
        public float avg_vote { get; set; } // Gemiddelde score op 10
        [LoadColumn(15)]
        [ColumnName("votes")]
        public float votes { get; set; } // Aantal gegeven scores
        [LoadColumn(16)]
        [ColumnName("reviews_from_users")]
        public float reviews_from_users { get; set; } // Aantal geschreven reviews door gebruikers van IMDB
        [LoadColumn(17)]
        [ColumnName("reviews_from_critics")]
        public float reviews_from_critics { get; set; } // Aantal geschreven reviews door erkende critici
    }
}
