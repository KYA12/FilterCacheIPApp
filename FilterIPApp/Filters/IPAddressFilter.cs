using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using FilterIPApp.Models;


namespace FilterIPApp.Filters
{
    /* IP Address action filter, that used to controllers to filter access for clients by IP Addresses from the Whitelist.*/
    public class IPAddressFilter : ActionFilterAttribute
    {
        private readonly List<IPTable> IPTables;
        private readonly ILogger _logger;
        public IPAddressFilter (ILoggerFactory loggerFactory, IPTableContext _context)
        {
            IPTables = _context.IPTables.ToList();//Get IP Address range from database
            _logger = loggerFactory.CreateLogger("FileLogger");

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string[] ipStart = new string[IPTables.Count()];
            string[] ipEnd = new string[IPTables.Count()]; 
            _logger.LogDebug($"Remote IpAddress: {context.HttpContext.Connection.RemoteIpAddress}");
            var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
            _logger.LogDebug($"Request from Remote IP address: {remoteIp}");

            for (int i = 0; i<IPTables.Count; i++)//Get array of values from start to end of the ip range
            {
                ipStart[i] = IPTables[i].IPStart;
                ipEnd[i] = IPTables[i].IPEnd;
            }

            var badIp = true;//On start, client is not allowed to see the web page 

            for (int i = 0; i<IPTables.Count; i++)//Check if client's ip is in the whitelist of the ip range. If yes - we allow client to see web page
            {
                var IPStart = IPAddress.Parse(ipStart[i]);
                var IPEnd = IPAddress.Parse(ipEnd[i]);
                if (remoteIp.IsInRange(IPStart, IPEnd))
                {
                    badIp = false;
                    break;
                }
            }

            if (badIp)//If client's ip address is not in the whitelist of the ip range, we dont allow client to see the web page
            {
                _logger.LogInformation( $"Forbidden Request from Remote IP address: {remoteIp}");
                context.Result = new StatusCodeResult(401);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}

