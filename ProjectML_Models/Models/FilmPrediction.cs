using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectML_Models.Models
{
    public class FilmPrediction : FilmData
    {
        [ColumnName("PredictedLabel")]
        public string Prediction { get; set; } //<----hoogste %
        public float[] Score { get; set; } //<---- Scores alle labels
    }
}
