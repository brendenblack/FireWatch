using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public class Person
    {
        public string Id { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
