using OperationResult;
using ShortUrl.Domain.Entities;
using ShortUrl.Domain.Interfaces;
using ShortUrl.Repository.Persistence;
using System;
using System.Linq;

namespace ShortUrl.Repository.Repositories
{
    public class UrlInfoRepository : BaseRepository<UrlInfo>, IUrlInfoRepository
    {
        private readonly object _lock = new();

        public UrlInfoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Result<UrlInfo> GetUrlInfoWithShortCode(string shortCode)
        {
            var urlInfo = Db.FirstOrDefault(x => x.ShortCode == shortCode && x.ExpiredDate > DateTime.UtcNow);

            if (urlInfo == null)
                return Result.Error<UrlInfo>(new ArgumentNullException());

            UpdateUrlHits(urlInfo);
            SaveChange();

            return urlInfo;

        }

        public Result<UrlInfo> GetUrlInfoWithOriginalUrl(string originalUrl)
        {
            var urlInfo = Db.FirstOrDefault(x => x.OriginalUrl == originalUrl);

            return urlInfo ?? Result.Error<UrlInfo>(new ArgumentNullException());
        }

        private void UpdateUrlHits(UrlInfo urlInfo)
        {
            lock (_lock)
            {
                urlInfo.UrlHits += 1;
                Update(urlInfo);
            }
        }
    }
}
