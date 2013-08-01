using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class CountryMap : EntityTypeConfiguration<Country>
    {
        public CountryMap()
        {
            // Primary Key
            this.HasKey(t => t.CountryID);

            // Properties
            this.Property(t => t.CountryName)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Country", "organization");
            this.Property(t => t.CountryID).HasColumnName("CountryID");
            this.Property(t => t.CountryName).HasColumnName("CountryName");
            this.Property(t => t.CountryAreaID).HasColumnName("CountryAreaID");

            // Relationships
            this.HasRequired(t => t.CountryArea)
                .WithMany(t => t.Countries)
                .HasForeignKey(d => d.CountryAreaID);

        }
    }
}
