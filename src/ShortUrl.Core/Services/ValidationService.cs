using Microsoft.Extensions.Options;
using OperationResult;
using ShortUrl.Core.Exceptions;
using ShortUrl.Core.Helpers;
using ShortUrl.Core.Interfaces;
using ShortUrl.Domain;
using ShortUrl.Domain.Enums;
using System;
using System.Text.RegularExpressions;

namespace ShortUrl.Core.Services
{
    public class ValidationService : IValidationService
    {
        private readonly string _urlPattern;
        private readonly int _shortCodeMaxLength;
        private readonly int _shortCodeMinLength;
        private readonly ServiceHelper _serviceHelper;

        public ValidationService(IOptions<AppSettings> options, ServiceHelper serviceHelper)
        {
            _urlPattern = options.Value.UrlValidationRegex;
            _shortCodeMaxLength = options.Value.ShortCodeMaxLength;
            _shortCodeMinLength = options.Value.ShortCodeMinLength;
            _serviceHelper = serviceHelper;
        }

        public Result<string> IsValidUrl(string url)
        {
            return !IsMatch(url)
                ? Result.Error<string>(new MessageException("invalid url pattern"))
                : UrlType.ShortUri.ToString();
        }

        public Result<string> IsValidShortCode(string shortCode)
        {
            var pattern = $"^[a-zA-Z0-9]{{{_shortCodeMinLength},{_shortCodeMaxLength}}}$";
            var regex = new Regex(pattern, RegexOptions.Compiled);
            var isMatch = regex.IsMatch(shortCode);

            return isMatch
                ? Result.Success(UrlType.ShortUri.ToString())
                : Result.Error<string>(new MessageException("invalid short code pattern"));
        }

        private Result<bool> IsMatch(string url)
        {
            if (string.IsNullOrEmpty(url))
                return Result.Error<bool>(new ArgumentNullException(nameof(url)));

            var regex = new Regex(_urlPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return regex.IsMatch(url);
        }
    }
}
