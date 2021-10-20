using ShortUrl.Domain.Common;

namespace ShortUrl.Domain.Entities
{
    public class UrlInfo : BaseEntity
    {
        public string OriginalUrl { get; set; }
        public string ShortCode { get; set; }
        public long UrlHits { get; set; }
    }
}
