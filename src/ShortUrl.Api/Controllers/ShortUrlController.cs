using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShortUrl.Core.Exceptions;
using ShortUrl.Core.Interfaces;
using ShortUrl.Core.ViewModels;
using System;

namespace ShortUrl.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class ShortUrlController : ControllerBase
    {
        private readonly IShortenService _shortenService;
        private readonly ILogger<ShortUrlController> _logger;

        public ShortUrlController(IShortenService shortenService, ILogger<ShortUrlController> logger)
        {
            _shortenService = shortenService;
            _logger = logger;
        }

        /// <summary>
        /// انتقال دهنده به کوتاه کننده به صفحه مورد نظر
        /// </summary>
        /// <param name="shortCode">کد اختصاری سایت</param>
        /// <returns></returns>
        [HttpGet("{shortCode}")]
        public IActionResult Get(string shortCode)
        {
            var (isSuccess, value, exception) = _shortenService.Extend(shortCode);

            _logger.LogInformation(
                $"User {Guid.NewGuid()} Wants to route to {value} using {ControllerContext.HttpContext.Request.Host + "/" + value}");

            return isSuccess
                ? Redirect(value)
                : GetFailedActionResult(exception);
        }

        /// <summary>
        /// کوتاه کننده لینک
        /// </summary>
        /// <param name="url">لینک سایت</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(ShortenUrl url)
        {
            var userId = Guid.NewGuid();

            _logger.LogInformation($"User {userId} Wants to shorten '{url.Url}'");

            var (isSuccess, value, exception) = _shortenService.Shorten(url.Url);

            _logger.LogInformation(
                $"User {userId} shorten '{url.Url}' to {ControllerContext.HttpContext.Request.Host + "/" + value}");

            return isSuccess
                ? Ok(new ShortenUrl { Url = value })
                : GetFailedActionResult(exception);
        }

        private IActionResult GetFailedActionResult(Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            if (exception is MessageException ex)
                return BadRequest(ex.Message);
            else
                return new JsonResult("internal service error");
        }
    }
}
