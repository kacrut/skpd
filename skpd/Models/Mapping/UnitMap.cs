using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class UnitMap : EntityTypeConfiguration<Unit>
    {
        public UnitMap()
        {
            // Primary Key
            this.HasKey(t => t.UnitID);

            // Properties
            this.Property(t => t.UnitName)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Unit", "organization");
            this.Property(t => t.UnitID).HasColumnName("UnitID");
            this.Property(t => t.UnitName).HasColumnName("UnitName");
            this.Property(t => t.DivisionID).HasColumnName("DivisionID");

            // Relationships
            this.HasRequired(t => t.Division)
                .WithMany(t => t.Units)
                .HasForeignKey(d => d.DivisionID);

        }
    }
}
