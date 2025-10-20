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
