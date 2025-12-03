using System.Collections.Generic;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace CtrlAltEliteProject.Services
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
                foreach (Page page in document.GetPages()) // or fully qualify: UglyToad.PdfPig.Content.Page
                {
                    string text = ContentOrderTextExtractor.GetText(page);
                    result.Add(text);
                }
            }

            return result;
        }
    }
}
