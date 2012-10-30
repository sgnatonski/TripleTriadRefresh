using System;
using System.Linq;

namespace TripleTriadRefresh.Server.Framework
{
    public static class IdGenerator
    {
        public static string GenerateId()
        {
            var i = Guid.NewGuid().ToByteArray().Aggregate<byte, long>(1, (current, b) => current * ((int)b + 1));
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
    }
}