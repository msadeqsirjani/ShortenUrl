using Microsoft.Extensions.Options;
using OperationResult;
using ShortUrl.Core.Exceptions;
using ShortUrl.Core.Helpers;
using ShortUrl.Core.Interfaces;
using ShortUrl.Domain;
using ShortUrl.Domain.Entities;
using ShortUrl.Domain.Interfaces;
using System;
using System.Net;

namespace ShortUrl.Core.Services
{
    public class ShortenService : IShortenService
    {
        private readonly ServiceHelper _serviceHelper;
        private readonly IUrlInfoRepository _urlInfoRepository;
        private readonly IValidationService _validationService;

        private readonly object _lock = new();

        private readonly int _expirationOfShortUrlInMonths;
        private readonly int _numberOfTryIfDuplicateShortCode;

        public ShortenService(ServiceHelper serviceHelper, IUrlInfoRepository urlInfoRepository,
            IValidationService validationService, IOptions<AppSettings> options)
        {
            _serviceHelper = serviceHelper;
            _urlInfoRepository = urlInfoRepository;
            _validationService = validationService;
            _expirationOfShortUrlInMonths = options.Value.ExpirationOfShortUrlInMonths;
            _numberOfTryIfDuplicateShortCode = options.Value.NumberOfTryIfDuplicateShortCode;
        }

        public Result<string> Shorten(string url)
        {
            lock (_lock)
            {
                var (isSuccess, _) = _validationService.IsValidUrl(url);
                if (!isSuccess)
                    return Result.Error<string>(new MessageException("url is invalid"));

                var encodedUrl = WebUtility.UrlEncode(url);

                url = GetShortUrl(encodedUrl);

                return url;
            }
        }

        public Result<string> Extend(string shortCode)
        {
            var url = GetOriginalUrl(shortCode);

            return string.IsNullOrEmpty(url)
                ? Result.Error<string>(new MessageException("لینک مورد نظر موجود نمی باشد"))
                : url;
        }

        private string GetOriginalUrl(string shortCode)
        {
            var (isSuccess, _) = _validationService.IsValidShortCode(shortCode);
            if (!isSuccess)
                return string.Empty;

            var urlInfo = _urlInfoRepository.GetUrlInfoWithShortCode(shortCode).Value;
            return urlInfo == null
                ? string.Empty
                : WebUtility.UrlDecode(urlInfo.OriginalUrl);
        }

        private string GetShortUrl(string originalUrl)
        {
            var (isSuccess, value) = _urlInfoRepository.GetUrlInfoWithOriginalUrl(originalUrl);

            if (isSuccess)
                return GetSuccessShortUrl(originalUrl, value.ShortCode);

            var shortCode = PrepareShortCode(originalUrl);
            if (string.IsNullOrEmpty(shortCode))
                return string.Empty;

            _urlInfoRepository.Insert(CreateUrlInfoObject(originalUrl, shortCode));
            _urlInfoRepository.SaveChange();

            return GetSuccessShortUrl(originalUrl, shortCode);
        }

        private string PrepareShortCode(string originalUrl)
        {
            for (var @try = 0; @try < _numberOfTryIfDuplicateShortCode; @try++)
            {
                var shortCodeLength = ServiceHelper.GetShortCodeLength(originalUrl).Value;
                var shortCode = ServiceHelper.GenerateShortCode(shortCodeLength).Value;
                var urlInfo = _urlInfoRepository.GetUrlInfoWithShortCode(shortCode).Value;

                if (urlInfo == null)
                    return shortCode;
            }

            return string.Empty;
        }

        private string GetSuccessShortUrl(string originalUrl, string shortCode)
        {
            var shortUrlBaseAddress = ServiceHelper.GetShortUrlBaseAddress(originalUrl).Value;
            var shortUrl = $"{shortUrlBaseAddress}{shortCode}";

            return shortUrl;
        }

        private UrlInfo CreateUrlInfoObject(string url, string shortCode)
        {
            return new()
            {
                OriginalUrl = url,
                ShortCode = shortCode,
                CreateDate = DateTime.UtcNow,
                ExpiredDate = DateTime.UtcNow.AddMonths(_expirationOfShortUrlInMonths),
                UrlHits = 1
            };
        }
    }
}
