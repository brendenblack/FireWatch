using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.Constants
{
    public class TradeConstants
    {
        public const string SELL_TO_CLOSE = "stc";
        public const string SELL_TO_OPEN = "sto";
        public const string BUY_TO_OPEN = "bto";
        public const string BUY_TO_CLOSE = "btc";


        public static readonly string[] SUPPORTED_TRADE_ACTIONS = { 
            SELL_TO_CLOSE,
            SELL_TO_OPEN,
            BUY_TO_CLOSE,
            BUY_TO_OPEN
        };

        public const string CREATION_METHOD_MANUAL = "manual";
        public const string CREATION_METHOD_IMPORT = "import";
    }
}
