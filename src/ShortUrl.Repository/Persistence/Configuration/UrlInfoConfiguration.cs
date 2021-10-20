using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShortUrl.Domain.Entities;
using ShortUrl.Repository.Persistence.Configuration.Common;

namespace ShortUrl.Repository.Persistence.Configuration
{
    public class UrlInfoConfiguration : EntityConfiguration<UrlInfo>
    {
        public override void Configure(EntityTypeBuilder<UrlInfo> builder)
        {
            base.Configure(builder);

            builder?.Property(x => x.OriginalUrl)
                .HasMaxLength(300)
                .IsRequired();

            builder?.Property(x => x.ShortCode)
                .HasMaxLength(8)
                .IsRequired();

            builder?.Property(x => x.UrlHits)
                .HasDefaultValue(0)
                .IsRequired();
        }
    }
}
