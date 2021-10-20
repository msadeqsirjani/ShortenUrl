using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShortUrl.Domain.Common;

namespace ShortUrl.Repository.Persistence.Configuration.Common
{
    public class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreateDate)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(x => x.ExpiredDate)
                .IsRequired();
        }
    }
}
