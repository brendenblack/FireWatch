using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.Constants
{
    public class AccountConstants
    {
        public const string CREDIT_CARD = "cc";
        public const string CHEQUING = "chk";
        public const string SAVINGS = "sav";
        public const string CASH = "cash";
        public const string BROKERAGE = "broker";

        public static readonly string[] SUPPORTED_ACCOUNT_TYPES = {
            CREDIT_CARD,
            CHEQUING,
            SAVINGS, 
            CASH,
            BROKERAGE
        };
    }
}
