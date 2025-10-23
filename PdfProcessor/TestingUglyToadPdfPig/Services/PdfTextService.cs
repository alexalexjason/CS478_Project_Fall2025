using System.Collections.Generic;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;
using UglyToad.PdfPig.Content;

namespace TestingUglyToadPdfPig.Services
{
    public interface IPdfTextService
    {
        IReadOnlyList<string> ExtractTextFromPages(string path);
    }

    public class PdfTextService : IPdfTextService
    {
        public IReadOnlyList<string> ExtractTextFromPages(string path)
        {
            var result = new List<string>();

            using (PdfDocument document = PdfDocument.Open(path))
            {
                foreach (Page page in document.GetPages())
                {
                    string text = ContentOrderTextExtractor.GetText(page);
                    result.Add(text);

                    // If you need words:
                    // var words = page.GetWords(NearestNeighbourWordExtractor.Instance);
                    // process words as needed
                }
            }

            return result;
        }
    }
}