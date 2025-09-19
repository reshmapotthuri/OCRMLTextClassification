using Emgu.CV;
using Emgu.CV.Structure;
using Tesseract;

namespace OCRMLClassifierApp.Helper
{
    public class OcrHelper
    {
        public static string ExtractText(string imagePath)
        {
            var img = new Image<Bgr, byte>(imagePath).Convert<Gray, byte>();
            img = img.ThresholdBinary(new Gray(120), new Gray(255)).SmoothGaussian(3);
            var tessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            using var ocr = new TesseractEngine(tessPath, "eng", EngineMode.Default);
            using var pix = Pix.LoadFromFile(imagePath);
            using var page = ocr.Process(pix);
            return page.GetText();
        }

    }
}
