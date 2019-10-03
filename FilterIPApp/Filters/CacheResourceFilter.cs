using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FilterIPApp.Filters
{
    /* The Resource Filter to cache client's allowed and denied IP Addresses */
    public class CacheResourceFilter : IResourceFilter
    {
        private static readonly Dictionary<string, object> _cache 
                = new Dictionary<string, object>();
        private string _cacheKey;
        private ILogger _logger;
        public CacheResourceFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("FileLogger");
            _logger.LogInformation($"---");
            _logger.LogInformation($"Data/Time: {DateTime.Now}");
            _logger.LogInformation($"---");
        }
        public void OnResourceExecuting(ResourceExecutingContext context)// If client visits the web page again, he would get the cached web page
        {
            _cacheKey = context.HttpContext.Connection.RemoteIpAddress.ToString();
            if (_cache.ContainsKey(_cacheKey))
            {
                _logger.LogDebug($"Request from Cached Remote IP address: {context.HttpContext.Connection.RemoteIpAddress}");
                var cachedValue = _cache[_cacheKey] as string;
                if (cachedValue != null)
                {
                    context.Result = new ContentResult()
                    { Content = cachedValue };
                }
            }
        }
        public void OnResourceExecuted(ResourceExecutedContext context)// If client visits the web page first time, his IP Address is added to the cache storage
        {
            if (!String.IsNullOrEmpty(_cacheKey) && !_cache.ContainsKey(_cacheKey))
            {
                _logger.LogDebug($"Request from Non Cached Remote IP address: {context.HttpContext.Connection.RemoteIpAddress}");
                var result = context.Result as ContentResult;
                if (result != null)
                {
                    _cache.Add(_cacheKey, result.Content);
                }
            }
        }
    }
}
