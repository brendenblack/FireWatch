using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public class BankAccount : Account
    {
        public BankAccount() { }

        public BankAccount(Person owner, int institutionNumber, int transitNumber, int accountNumber)
            : base(owner)
        {
            // TODO: some validation around these numbers
            InstitutionNumber = institutionNumber;
            TransitNumber = transitNumber;
            InstitutionAccountNumber = accountNumber;

            AccountNumber = $"{transitNumber}-${institutionNumber}-${accountNumber}";
            DisplayName = "" + accountNumber;
        }

        public BankAccount(Person owner, string number)
            : base(owner)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number));
            }

            number = number.Trim().Replace(" ", "").Replace("-", "");
            if (number.Length != 15)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            InstitutionNumber = int.TryParse(number.Substring(0, 3), out int institutionNumber)
                ? institutionNumber
                : 999;

            TransitNumber = int.TryParse(number.Substring(3, 5), out int transitNumber)
                ? transitNumber
                : 99999;

            InstitutionAccountNumber = int.TryParse(number.Substring(8, 7), out int accountNumber)
                ? accountNumber
                : 9999999;

            AccountNumber = $"{InstitutionNumber:D3}-{TransitNumber:D5}-{InstitutionAccountNumber:D7}";

            DisplayName = InstitutionAccountNumber.ToString("D7");
        }

        public override string AccountType { get; protected set; } = "bankaccount";

        /// <summary>
        /// A code that identifies the financial institution that holds the account.
        /// </summary>
        public int InstitutionNumber { get; private set; }

        public int TransitNumber { get; private set; }

        /// <summary>
        /// The bank account number assigned by the financial institution.
        /// </summary>
        public int InstitutionAccountNumber { get; private set; }

    }
}
