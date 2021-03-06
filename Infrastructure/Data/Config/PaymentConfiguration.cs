using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.Id).IsRequired().HasMaxLength(128);
            builder.Property(p => p.CreatedOn).IsRequired();
            builder.Property(p => p.CreditCardNumber).IsRequired().HasMaxLength(16);
            builder.Property(p => p.CardHolder).IsRequired().HasMaxLength(256);
            builder.Property(p => p.ExpirationDate).IsRequired();
            builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
            builder.Property(p => p.SecurityCode).IsRequired().HasMaxLength(3);
        }
    }
}
