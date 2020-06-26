using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public class CreditCardAccount : Account
    {
        public CreditCardAccount()
        {
        }

        public CreditCardAccount(Person owner, string number)
            : base(owner) // pass a blank number to make sure we aren't storing the full credit card number
        {
            var maskedNumber = MaskAccountNumber(number);
            AccountNumber = maskedNumber;
            DisplayName = maskedNumber;
        }

        public override string AccountType { get; protected set; } = "creditcard";

        /// <summary>
        /// Masks the middle 8 digits of a Visa number to ensure we are never saving the full credit card number but are still
        /// able to uniquely identify the account reasonably well.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string MaskAccountNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException();
            }

            number = number.Trim().Replace(" ", "");

            if (number.Length != 16)
            {
                throw new ArgumentException("The provided account number {} is not a recognized Visa number", number);
            }

            // TODO: mask
            var maskBuilder = new StringBuilder(number)
                .Remove(4, 8)
                .Insert(4, " **** **** ");

            return maskBuilder.ToString();
        }
    }
}
