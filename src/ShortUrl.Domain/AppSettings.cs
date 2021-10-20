namespace ShortUrl.Domain
{
    public class AppSettings
    {
        public int ShortCodeMaxLength { get; set; }
        public int ShortCodeMinLength { get; set; }
        public int ExpirationOfShortUrlInMonths { get; set; }
        public int NumberOfTryIfDuplicateShortCode { get; set; }
        public string ShortCodeCharacterSet { get; set; }
        public string UrlValidationRegex { get; set; }
    }
}
