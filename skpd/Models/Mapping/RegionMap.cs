using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class RegionMap : EntityTypeConfiguration<Region>
    {
        public RegionMap()
        {
            // Primary Key
            this.HasKey(t => t.RegionID);

            // Properties
            this.Property(t => t.RegionName)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Region", "organization");
            this.Property(t => t.RegionID).HasColumnName("RegionID");
            this.Property(t => t.RegionName).HasColumnName("RegionName");
        }
    }
}
