using System;

namespace ShortUrl.Domain.Common
{
    public class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiredDate { get; set; } = DateTime.UtcNow.AddMonths(1);
    }
}
