using Firewatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;

namespace Firewatch.Infrastructure.Persistence.Configurations
{
    public class TradeExecutionConfiguration : AuditableEntityConfiguration<TradeExecution>
    {
        public override void Configure(EntityTypeBuilder<TradeExecution> builder)
        {
            base.Configure(builder);

            ValueComparer<IReadOnlyCollection<string>> stringListComparer = new ValueComparer<IReadOnlyCollection<string>>(
                (a, b) => a.Equals(b),
                c => c.GetHashCode());

            builder.Property(c => c.Tags)
               .HasConversion(
                   model => JsonConvert.SerializeObject(model, Formatting.None),
                   db => JsonConvert.DeserializeObject<List<string>>(db))
               .Metadata.SetValueComparer(stringListComparer);

            builder.Property(c => c.Exchanges)
               .HasConversion(
                   model => JsonConvert.SerializeObject(model, Formatting.None),
                   db => JsonConvert.DeserializeObject<List<string>>(db))
               .Metadata.SetValueComparer(stringListComparer);

            builder.Property(t => t.Quantity)
                .HasColumnType("decimal(30,6)");

            builder.HasOne(t => t.Account)
                .WithMany()
                .HasForeignKey(t => t.AccountId);

            builder.OwnsOne(t => t.UnitPrice, up =>
            {
                up.Property(p => p.Amount)
                    .HasColumnName("UnitPriceAmount")
                    .HasColumnType("decimal(30,2)");

                up.Property(p => p.Currency)
                    .HasColumnName("UnitPriceCurrency")
                    .HasConversion(
                        model => model.AlphabeticCode,
                        db => Currency.ByAlphabeticCode(db));
            });

            builder.OwnsOne(t => t.Fees, fees =>
            {
                fees.Property(p => p.Amount)
                   .HasColumnName("FeesAmount")
                   .HasColumnType("decimal(30,2)");

                fees.Property(p => p.Currency)
                    .HasColumnName("FeesCurrency")
                    .HasConversion(
                        model => model.AlphabeticCode,
                        db => Currency.ByAlphabeticCode(db));
            });

            builder.OwnsOne(t => t.Commissions, c =>
            {
                c.Property(p => p.Amount)
                   .HasColumnName("CommissionsAmount")
                   .HasColumnType("decimal(30,2)");

                c.Property(p => p.Currency)
                    .HasColumnName("CommissionsCurrency")
                    .HasConversion(
                        model => model.AlphabeticCode,
                        db => Currency.ByAlphabeticCode(db));
            });
        }
    }
}
