using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class CurrencyMap : EntityTypeConfiguration<Currency>
    {
        public CurrencyMap()
        {
            // Primary Key
            this.HasKey(t => t.CurrencyID);

            // Properties
            this.Property(t => t.CurrencyName)
                .IsRequired()
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("Currency", "skpd");
            this.Property(t => t.CurrencyID).HasColumnName("CurrencyID");
            this.Property(t => t.CurrencyName).HasColumnName("CurrencyName");
        }
    }
}
