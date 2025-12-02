namespace WebApplication1.Models
{
    public class HuggingFaceOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "gpt2"; // change to the HF model you want
    }
}
