/*
 * Developer: Evan S.
 * Date Created: 10/19/2025
 * Last Updated: 10/19/2025
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
        /// <summary>
        /// Extracts text asynchronously from a PDF file located at pdfPath.
        /// </summary>
        /// <param name="pdfPath">The file path of the PDF.</param>
        /// <returns>The extracted text as a string.</returns>


        //Use PdfPig https://github.com/UglyToad/PdfPig
        //or iTextSharp here

        public async Task<string> ExtractTextAsync(string pdfPath)
        {
            try
            {
                var textBuilder = new StringBuilder();

                // PdfPig is synchronous, so wrap it in a Task to keep async flow
                await Task.Run(() =>
                {
                    using (var document = PdfDocument.Open(pdfPath))
                    {
                        foreach (var page in document.GetPages())
                        {
                            textBuilder.AppendLine(page.Text);
                        }
                    }
                });

                return textBuilder.ToString();
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return $"Error extracting text: {ex.Message}";
            }

            /* 
             * This was before the PdfPig functionality was included
            //return Task.FromResult("[Extracted text from PDF]");
            */

        } //CLOSING public async Task<string> ...
    
    } //CLOSING public class PdfProcessor

} //CLOSING namespace FilePreProcessingAPI.Services
