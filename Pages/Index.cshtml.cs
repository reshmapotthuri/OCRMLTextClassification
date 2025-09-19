using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.ML;
using OCRMLClassifierApp.Helper;
using OCRMLClassifierApp.Models;

namespace OCRMLClassifierApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly PredictionEngine<DocumentData, DocumentPrediction> _predictor;
        public string PredictedLabel { get; private set; }
        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
            var mlContext = new MLContext();
            var modelPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "models", "Model.zip");

            var model = mlContext.Model.Load(modelPath, out _);
            _predictor = mlContext.Model.CreatePredictionEngine<DocumentData, DocumentPrediction>(model);

        }
        public async Task<IActionResult> OnPostAsync(IFormFile UploadedImage)
        {
            var filePath = Path.Combine(_env.WebRootPath, "uploads", UploadedImage.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await UploadedImage.CopyToAsync(stream);

            string extractedText = OcrHelper.ExtractText(filePath);
            var prediction = _predictor.Predict(new DocumentData { Text = extractedText });
            PredictedLabel = prediction.PredictedLabel;

            return Page();
        }

        public void OnGet()
        {

        }
    }
}
