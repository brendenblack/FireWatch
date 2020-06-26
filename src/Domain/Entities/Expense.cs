using Firewatch.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public class Expense : AuditableEntity
    {
        public Expense() { }

        [Obsolete("Use ctor(Transaction) instead")]
        public Expense(Transaction transaction, ExpenseCategory category)
            : this(transaction)
        {
            Category = category;
            CategoryId = category.Id;
        }

        public Expense(Transaction transaction)
        {
            Transaction = transaction;
            TransactionId = transaction.Id;
        }

        public int Id { get; set; }

        public ICollection<string> Notes { get; private set; } = new List<string>();

        public ICollection<string> Tags { get; private set; } = new List<string>();

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
        }

        public void AddTag(string tag)
        {
            if (!Tags.Any(t => t.ToUpper() == tag.ToUpper()))
            {
                Tags.Add(tag);
            }
        }

        public Transaction Transaction { get; }
        public int TransactionId { get; }
        public ExpenseCategory Category { get; set; }
        public int CategoryId { get; set; }
    }
}
