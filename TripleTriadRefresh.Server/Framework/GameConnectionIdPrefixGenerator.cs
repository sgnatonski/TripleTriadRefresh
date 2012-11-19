using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;

namespace TripleTriadRefresh.Server.Framework
{
    public class GameConnectionIdPrefixGenerator : IConnectionIdPrefixGenerator
    {
        public string GenerateConnectionIdPrefix(IRequest request)
        {
            if (request.Headers["srconnectionid"] != null && request.Headers["srconnectionid"] != "null")
            {
                return request.Headers["srconnectionid"];
            }

            return Guid.NewGuid().ToString();
        }
    }
}