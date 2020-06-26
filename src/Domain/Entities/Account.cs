using Firewatch.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public abstract class Account : AuditableEntity
    {
        protected Account() { }

        public Account(Person owner)
        {
            Owner = owner;
            OwnerId = owner.Id;
        }

        public int Id { get; set; }

        /// <summary>
        /// A unique identifier for this account, by default the account number.
        /// </summary>
        public string AccountNumber { get; protected set; }

        /// <summary>
        /// A user-friendly name for this account.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// An offset to apply before summing up <see cref="Transaction"/>s to calculate the balance of this account.
        /// <para>
        /// This is useful for obtaining an accurate balance without having to import the entire history of transactions.
        /// </para>
        /// </summary>
        public decimal BalanceOffset { get; set; }

        public string OwnerId { get; set; }
        public Person Owner { get; set; }

        public abstract string AccountType { get; protected set; }

        public virtual ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public void AddTransaction(DateTime date, string[] descriptions, decimal amount, string currencyCode)
        {
            var transaction = new Transaction(this, date, amount, currencyCode);
            Transactions.Add(transaction);
        }


        public override string ToString()
        {
            return DisplayName;
        }
    }
}
