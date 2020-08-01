using Firewatch.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public class BrokerageAccount : Account
    {
        public BrokerageAccount() { }

        public BrokerageAccount(Person owner, string accountNumber, string institution, string designation = "Investment")
            : this(owner, accountNumber)
        {
            Institution = institution;
            Designation = designation;
        }

        public BrokerageAccount(Person owner, string accountNumber)
            : base(owner)
        {
            AccountNumber = accountNumber;
            DisplayName = accountNumber;
        }

        public override string AccountType { get; protected set; } = AccountConstants.BROKERAGE;


        public string Designation { get; set; }

        public string Institution { get; set; }
    }
}
