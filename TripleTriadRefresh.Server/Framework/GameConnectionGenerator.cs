using System;
using SignalR;

namespace TripleTriadRefresh.Server.Framework
{
    public class GameConnectionGenerator : IConnectionIdGenerator
    {
        public string GenerateConnectionId(IRequest request)
        {
            if (request.Headers["srconnectionid"] != null && request.Headers["srconnectionid"] != "null")
            {
                return request.Headers["srconnectionid"];
            }

            return Guid.NewGuid().ToString();
        }
    }
}