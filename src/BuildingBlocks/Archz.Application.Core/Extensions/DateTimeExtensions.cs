using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archz.Application.Core.Extensions;
public static class DateTimeExtensions
{
    private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long ToTimestamp(this DateTime value)
    {
        TimeSpan elapsedTime = value.ToUniversalTime() - Epoch;
        return (long)elapsedTime.TotalSeconds;
    }
}
