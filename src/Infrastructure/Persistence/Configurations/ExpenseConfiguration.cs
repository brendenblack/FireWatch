using Firewatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Infrastructure.Persistence.Configurations
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public virtual void Configure(EntityTypeBuilder<Expense> builder)
        {
            ValueComparer<ICollection<string>> stringListComparer = new ValueComparer<ICollection<string>>(
                (a, b) => a.Equals(b),
                c => c.GetHashCode());

            builder.Property(c => c.Tags)
                .HasConversion(
                    model => JsonConvert.SerializeObject(model, Formatting.None),
                    db => JsonConvert.DeserializeObject<List<string>>(db))
                .Metadata.SetValueComparer(stringListComparer);

            builder.Property(c => c.Notes)
                .HasConversion(
                    model => JsonConvert.SerializeObject(model, Formatting.None),
                    db => JsonConvert.DeserializeObject<List<string>>(db))
                .Metadata.SetValueComparer(stringListComparer);
        }
    }
}
