using Firewatch.Domain.Common;
using Firewatch.Domain.Entities;
using Firewatch.Domain.Enums;
using Firewatch.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Firewatch.Domain.ValueObjects
{
    public class OptionContract : ValueObject
    {
        private OptionContract() { }

        /// <summary>
        /// Creates a value object representing a given option contract.
        /// The provided pattern must use the following named capture groups: 
        /// <list type="bullet">
        /// <item><description>year</description></item>
        /// <item><description>month</description></item>
        /// <item><description>day</description></item>
        /// <item><description>symbol</description></item>
        /// <item><description>type</description></item>
        /// <item><description>strike</description></item>
        /// </list>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static OptionContract For(string input, string pattern = DOMAIN_PATTERN)
        {
            // TODO: better unhappy path handling

            try
            {
                foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
                {
                    string rawYear = match.Groups["year"].Value;
                    if (rawYear.Length == 2)
                    {
                        // if we were given a 2 digit year, assume 21st century
                        rawYear = $"20{rawYear}";
                    }

                    DateTime expiration = new DateTime();
                    if (int.TryParse(match.Groups["month"].Value, out int monthNumber))
                    {
                        expiration = DateTime.ParseExact($"{rawYear} {monthNumber:D2} {match.Groups["day"].Value}", "yyyy MM dd", null);
                    }
                    else
                    {
                        expiration = DateTime.Parse($"{match.Groups["day"].Value} {match.Groups["month"].Value} {rawYear}");
                    }
                        
                    decimal strike = decimal.Parse(match.Groups["strike"].Value);

                    var contract = new OptionContract();
                    contract.Symbol = match.Groups["symbol"].Value;
                    contract.ExpirationDate = expiration;
                    contract.OptionType = (match.Groups["type"].Value.Equals("C")) ? OptionTypes.CALL : OptionTypes.PUT;
                    contract.StrikePrice = new Price(strike, "USD"); // TODO: should this always be USD?

                    return contract;
                }
            }
            catch (Exception ex)
            {
                // This was probably caused by a faulty regex
                throw new OptionContractInvalidException(input, ex);
            }

            // No match was found with the pattern, 
            throw new OptionContractInvalidException(input);

        }

        /// <summary>
        /// Creates a value object representing a given option contract.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="expiration"></param>
        /// <param name="strike"></param>
        /// <param name="optionType"></param>
        /// <returns></returns>
        public static OptionContract For(string symbol, DateTime expiration, decimal strike, OptionTypes optionType)
        {
            return new OptionContract
            {
                ExpirationDate = expiration,
                Symbol = symbol,
                StrikePrice = new Price(strike, "USD"),
                OptionType = optionType
            };
        }

        /// <summary>
        /// The underlying asset that the contract is for.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// What day this contract expires on.
        /// </summary>
        public DateTime ExpirationDate { get; private set; }

        /// <summary>
        /// The price at which this option can be exercised.
        /// </summary>
        public Price StrikePrice { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public OptionTypes OptionType { get; private set; }

        public override string ToString()
        {
            // WARNING:
            // Any changes made here must be reflected in the DOMAIN_FORMAT regex pattern.
            string callOrPut = OptionType == OptionTypes.CALL ? "C" : "P";
            string date = ExpirationDate.ToString("yyyyMMdd");
            return $"{Symbol} {date} {StrikePrice.Amount:0.00} {callOrPut}";
        }


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Symbol;
            yield return ExpirationDate;
            yield return StrikePrice;
            yield return OptionType;
        }

        /// <summary>
        /// A pattern that matches the value produced by <see cref="ToString"/>.
        /// </summary>
        public const string DOMAIN_PATTERN = @"(?<symbol>\w+)\s+(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})\s(?<strike>[\d\.]+)\s(?<type>[CP])";

    }
}
