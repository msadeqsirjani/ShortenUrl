using OperationResult;
using ShortUrl.Domain.Entities;

namespace ShortUrl.Domain.Interfaces
{
    public interface IUrlInfoRepository : IBaseRepository<UrlInfo>
    {
        Result<UrlInfo> GetUrlInfoWithShortCode(string shortCode);
        Result<UrlInfo> GetUrlInfoWithOriginalUrl(string originalUrl);
    }
}
