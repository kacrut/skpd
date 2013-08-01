using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class ExchangeRateMap : EntityTypeConfiguration<ExchangeRate>
    {
        public ExchangeRateMap()
        {
            // Primary Key
            this.HasKey(t => t.ExchangeRateID);

            // Properties
            // Table & Column Mappings
            this.ToTable("ExchangeRate", "skpd");
            this.Property(t => t.ExchangeRateID).HasColumnName("ExchangeRateID");
            this.Property(t => t.CurrencyID).HasColumnName("CurrencyID");
            this.Property(t => t.ExchangeRate1).HasColumnName("ExchangeRate");
            this.Property(t => t.ExchangeDate).HasColumnName("ExchangeDate");

            // Relationships
            this.HasRequired(t => t.Currency)
                .WithMany(t => t.ExchangeRates)
                .HasForeignKey(d => d.CurrencyID);

        }
    }
}
