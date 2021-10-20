using OperationResult;

namespace ShortUrl.Core.Interfaces
{
    public interface IValidationService
    {
        Result<string> IsValidUrl(string url);
        Result<string> IsValidShortCode(string shortCode);
    }
}