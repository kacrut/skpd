using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class CountryAreaMap : EntityTypeConfiguration<CountryArea>
    {
        public CountryAreaMap()
        {
            // Primary Key
            this.HasKey(t => t.CountryAreaID);

            // Properties
            this.Property(t => t.CountryAreaName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("CountryArea", "organization");
            this.Property(t => t.CountryAreaID).HasColumnName("CountryAreaID");
            this.Property(t => t.CountryAreaName).HasColumnName("CountryAreaName");
        }
    }
}
