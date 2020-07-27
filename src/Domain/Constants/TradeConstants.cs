using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.Constants
{
    public class TradeConstants
    {
        /// <summary>
        /// Implies the sale of an asset on the long side.
        /// </summary>
        public const string SELL_TO_CLOSE = "selltoclose";

        /// <summary>
        /// Implies the sale of an asset on the short side.
        /// </summary>
        public const string SELL_TO_OPEN = "selltoopen";

        /// <summary>
        /// Implies the purchase of an asset on the long side.
        /// </summary>
        public const string BUY_TO_OPEN = "buytoopen";

        /// <summary>
        /// Implies the purchase of an asset on the short side.
        /// </summary>
        public const string BUY_TO_CLOSE = "buytoclose";


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
