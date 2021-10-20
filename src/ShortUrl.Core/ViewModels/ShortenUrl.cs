using System.ComponentModel.DataAnnotations;

namespace ShortUrl.Core.ViewModels
{
    public class ShortenUrl
    {
        [Required]
        public string Url { get; set; }
    }
}
