using Firewatch.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Infrastructure.Persistence.Configurations
{
    public class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : AuditableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            //builder.HasOne(a => a.CreatedBy)
            //    .WithMany()
            //    .HasForeignKey(a => a.CreatedBy);

            //builder.HasOne(a => a.LastModifiedBy)
            //    .WithMany()
            //    .HasForeignKey(a => a.LastModifiedBy);
        }
    }
}
