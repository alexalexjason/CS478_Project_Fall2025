/*
 * Developer: Evan S.
 * Date Created: 10/19/2025
 * Last Updated: 10/23/2025
 * Description: For CS 478 Team Project
 *              This code extracts text from a pdf
*/

using System;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace FilePreProcessingAPI.Services
{
    public class PdfProcessor
    {
        public async Task<string> ExtractTextAsync(string pdfPath)
        {
            if (string.IsNullOrWhiteSpace(pdfPath))
                throw new ArgumentException("pdfPath is required.", nameof(pdfPath));

            var sb = new StringBuilder();

            // PdfPig is synchronous; run it on a background thread
            await Task.Run(() =>
            {
                using var document = PdfDocument.Open(pdfPath);
                foreach (var page in document.GetPages())
                {
                    if (!string.IsNullOrEmpty(page?.Text))
                        sb.AppendLine(page.Text);
                }
            });

            return sb.ToString();
        }
    }
}