/*
 * Developer: Evan S.
 * Date Created: 10/19/2025
 * Last Updated: 10/19/2025
 * Description: For CS 478 Team Project
 *              This code extracts text from a pdf
*/

namespace FilePreProcessingAPI.Services
{
    public class PdfProcessor
    {
        public Task<string> ExtractTextAsync(string pdfPath)
        {
            //Later: Use PdfPig https://github.com/UglyToad/PdfPig
            //or iTextSharp here
            return Task.FromResult("[Extracted text from PDF]");
        }
    }
}
