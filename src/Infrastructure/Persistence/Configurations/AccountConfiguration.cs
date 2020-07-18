using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Firewatch.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : AuditableEntityConfiguration<Account> //IEntityTypeConfiguration<Account>
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);

            builder.HasDiscriminator(a => a.AccountType)
                .HasValue<CreditCardAccount>(AccountConstants.CREDIT_CARD)
                .HasValue<BankAccount>(AccountConstants.CHEQUING)
                .HasValue<CashAccount>(AccountConstants.CASH)
                .HasValue<BrokerageAccount>(AccountConstants.BROKERAGE);


            builder.HasOne(a => a.Owner)
                .WithMany(p => p.Accounts)
                .HasForeignKey(a => a.OwnerId)
                .IsRequired();

            builder.Property(a => a.BalanceOffset)
                .HasColumnType("decimal(38,2)");

            builder.HasMany(a => a.Transactions)
                .WithOne()
                .HasForeignKey(a => a.AccountId);

            //base.Configure(builder);
        }

    }
}
