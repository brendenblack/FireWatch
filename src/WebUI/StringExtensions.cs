using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Firewatch.WebUI
{
    public static class StringExtensions
    {
        /// <summary>
        /// Uses <see cref="DateTime.TryParseExact(string?, string?, IFormatProvider?, DateTimeStyles, out DateTime)"/> to convert a given string in to a <see cref="DateTime"/> object.
        /// If the parsing fails, <see cref="DateTime.MinValue"/> is returned.
        /// </summary>
        /// <param name="input">The string containing a date representation.</param>
        /// <param name="format">What format the string is in. Default is yyyyMMdd.</param>
        /// <param name="behaviour>Whether the returned <see cref="DateTIme"/> should be at the start or end of the target day.</param>
        /// <returns></returns>
        public static DateTime ToDate(this string input, string format = "yyyyMMdd", DateTimeBehaviours behaviour = DateTimeBehaviours.Default)
        {
            DateTime date;

            if (!DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                date = DateTime.MinValue;
            }

            switch (behaviour)
            {
                case DateTimeBehaviours.StartOfDay:
                    date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    break;
                case DateTimeBehaviours.EndOfDay:
                    date = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    break;
                default:
                    break;
            }

            return date;
        }


    }
}
