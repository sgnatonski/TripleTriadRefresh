using System;
using System.Collections.Generic;
using System.Linq;

namespace TripleTriadRefresh.Data.Linq
{
    public static class LinqExtensions
    {
        public static double StdDev(this IEnumerable<int> values)
        {
            double ret = 0;
            int count = values.Count();
            if (count > 1)
            {
                //Compute the Average
                double avg = values.Average();

                //Perform the Sum of (value-avg)^2
                double sum = values.Sum(d => (d - avg) * (d - avg));

                //Put it all together
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }
    }
}