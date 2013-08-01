using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class CountryInRegionMap : EntityTypeConfiguration<CountryInRegion>
    {
        public CountryInRegionMap()
        {
            // Primary Key
            this.HasKey(t => t.CountryInRegionID);

            // Properties
            // Table & Column Mappings
            this.ToTable("CountryInRegion", "organization");
            this.Property(t => t.CountryInRegionID).HasColumnName("CountryInRegionID");
            this.Property(t => t.RegionID).HasColumnName("RegionID");
            this.Property(t => t.CountryID).HasColumnName("CountryID");

            // Relationships
            this.HasRequired(t => t.Country)
                .WithMany(t => t.CountryInRegions)
                .HasForeignKey(d => d.CountryID);
            this.HasRequired(t => t.Region)
                .WithMany(t => t.CountryInRegions)
                .HasForeignKey(d => d.RegionID);

        }
    }
}
