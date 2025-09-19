using Microsoft.ML.Data;

namespace OCRMLClassifierApp.Models
{
    public class DocumentPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; }

    }
}
