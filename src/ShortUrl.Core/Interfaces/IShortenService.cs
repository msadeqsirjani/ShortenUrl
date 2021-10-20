using OperationResult;

namespace ShortUrl.Core.Interfaces
{
    public interface IShortenService
    {
        Result<string> Shorten(string originalUrl);
        Result<string> Extend(string shortCode);
    }
}
