using Firewatch.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public class CashAccount : Account
    {
        public CashAccount() { }
        public CashAccount(Person owner) : base(owner) { }
        public override string AccountType { get; protected set; } = AccountConstants.CASH;
    }
}
