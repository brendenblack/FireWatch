using Firewatch.Domain.Enums;
using Firewatch.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public class OptionsTrade : Trade
    {
        public OptionsTrade(string symbol) : base(symbol)
        {
        }

        public string UnderlyingSymbol { get; }

        public static string Format(string underlying, OptionTypes optionType, DateTime contractDate, decimal strikePrice)
        {
            return "";
        }
    }
}
