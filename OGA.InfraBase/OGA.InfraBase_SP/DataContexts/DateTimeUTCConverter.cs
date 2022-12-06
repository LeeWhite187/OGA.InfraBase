using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace OGA.InfraBase.DataContexts
{
    /// <summary>
    /// This class provides bulk-configuration for all DateTime value conversions.
    /// If used by a datacontext, it will set the DateTimeKind property of all read DateTime values to UTC.
    /// To use this, add it as a HaveConversion in the configuration builder of your datacontext.
    /// NOTE: If used, ALL DateTime values will be set to UTC on read.
    /// So, us a more specific value conversion method, instead, if you store a mix of UTC and non-UTC in a datacontext.
    /// See this usage wiki: https://oga.atlassian.net/wiki/spaces/~311198967/pages/66322433/EF+Working+with+DateTime
    /// Reference this Microsoft note: https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#configuring-a-value-converter
    /// </summary>
    public class DateTimeUTCConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeUTCConverter()
            : base(v => v, v => new DateTime(v.Ticks, DateTimeKind.Utc)) { }
    }
}
