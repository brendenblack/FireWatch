using Firewatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Firewatch.Infrastructure.Persistence.Configurations
{
    public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.ChildrenCategories)
                .HasForeignKey(c => c.ParentCategoryId);

            builder.HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(c => c.OwnerId);

            builder.Property(t => t.Color)
                .HasConversion(
                    model => ColorTranslator.ToHtml(model),
                    db => ColorTranslator.FromHtml(db));

            builder.Property(c => c.MonthlyBudget)
                .HasColumnType("decimal(38,2)");

            builder.Ignore(c => c.IsParent);
            builder.Ignore(c => c.HasChildren);
        }
    }
}
