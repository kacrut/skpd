using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class PositionTypeMap : EntityTypeConfiguration<PositionType>
    {
        public PositionTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.PositionTypeID);

            // Properties
            this.Property(t => t.PositionTypeName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PositionType", "organization");
            this.Property(t => t.PositionTypeID).HasColumnName("PositionTypeID");
            this.Property(t => t.PositionTypeName).HasColumnName("PositionTypeName");
            this.Property(t => t.LevelPositionID).HasColumnName("LevelPositionID");

            // Relationships
            this.HasOptional(t => t.LevelPosition)
                .WithMany(t => t.PositionTypes)
                .HasForeignKey(d => d.LevelPositionID);

        }
    }
}
