using Microsoft.Extensions.Options;
using OperationResult;
using ShortUrl.Domain;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ShortUrl.Core.Helpers
{
    public sealed class ServiceHelper
    {
        private static int _shortCodeMaxLength;
        private static char[] _shortCodeCharacterSetArray;
        private static RNGCryptoServiceProvider _cryptoServiceProvider;
        private static int _characterBase;

        public ServiceHelper(IOptions<AppSettings> options)
        {
            _shortCodeMaxLength = options.Value.ShortCodeMaxLength;
            _shortCodeCharacterSetArray = options.Value.ShortCodeCharacterSet.ToCharArray();
            _characterBase = _shortCodeCharacterSetArray.Length;
            _cryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        public static Result<string> GenerateShortCode(int length = 6)
        {
            var byteArray = new byte[length];
            _cryptoServiceProvider.GetNonZeroBytes(byteArray);

            var shortCode = new StringBuilder(length);
            foreach (var currentByte in byteArray)
            {
                var position = currentByte % (_characterBase - 1);
                shortCode.Append(_shortCodeCharacterSetArray[position]);
            }

            return shortCode.ToString();
        }

        public Result<int> GetShortCodeLength(string url)
        {
            return url.Length > _shortCodeMaxLength
                ? _shortCodeMaxLength
                : url.Length - 1;
        }

        public Result<string> GetShortUrlBaseAddress(string url)
        {
            var baseAddress = url.StartsWith(Uri.UriSchemeHttps)
                ? ShortUrlInfo.SecureDomain
                : ShortUrlInfo.HttpDomain;

            return baseAddress;
        }
    }
}
