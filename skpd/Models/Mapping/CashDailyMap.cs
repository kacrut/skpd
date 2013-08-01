using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class CashDailyMap : EntityTypeConfiguration<CashDaily>
    {
        public CashDailyMap()
        {
            // Primary Key
            this.HasKey(t => t.CashDailyID);

            // Properties
            // Table & Column Mappings
            this.ToTable("CashDaily", "skpd");
            this.Property(t => t.CashDailyID).HasColumnName("CashDailyID");
            this.Property(t => t.CountryID).HasColumnName("CountryID");
            this.Property(t => t.LevelPositionID).HasColumnName("LevelPositionID");
            this.Property(t => t.CashStay).HasColumnName("CashStay");
            this.Property(t => t.CashNotStay).HasColumnName("CashNotStay");
            this.Property(t => t.CurrencyID).HasColumnName("CurrencyID");

            // Relationships
            this.HasRequired(t => t.Country)
                .WithMany(t => t.CashDailies)
                .HasForeignKey(d => d.CountryID);
            this.HasRequired(t => t.LevelPosition)
                .WithMany(t => t.CashDailies)
                .HasForeignKey(d => d.LevelPositionID);
            this.HasRequired(t => t.Currency)
                .WithMany(t => t.CashDailies)
                .HasForeignKey(d => d.CurrencyID);

        }
    }
}
