using Domain.Entities;
using Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Infrastructure.Data.Config
{
    public class PaymentStateConfiguration : IEntityTypeConfiguration<PaymentState>
    {
        public void Configure(EntityTypeBuilder<PaymentState> builder)
        {
            builder.Property(p => p.Id).IsRequired().HasMaxLength(128);
            builder.Property(p => p.CreatedOn).IsRequired();
            builder.Property(p => p.Status)
                .HasConversion(
                    o => o.ToString(),
                    o => (PayState)Enum.Parse(typeof(PayState), o)
                ).HasMaxLength(20);

            builder.HasOne(b => b.Payment).WithMany(b => b.PaymentStates)
               .HasForeignKey(p => p.PaymentId).IsRequired();
        }
    }
}
