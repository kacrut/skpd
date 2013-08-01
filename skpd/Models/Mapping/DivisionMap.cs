using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class DivisionMap : EntityTypeConfiguration<Division>
    {
        public DivisionMap()
        {
            // Primary Key
            this.HasKey(t => t.DivisionID);

            // Properties
            this.Property(t => t.DivisionName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Division", "organization");
            this.Property(t => t.DivisionID).HasColumnName("DivisionID");
            this.Property(t => t.DivisionName).HasColumnName("DivisionName");
        }
    }
}
