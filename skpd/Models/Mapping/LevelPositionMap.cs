using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class LevelPositionMap : EntityTypeConfiguration<LevelPosition>
    {
        public LevelPositionMap()
        {
            // Primary Key
            this.HasKey(t => t.LevelPositionID);

            // Properties
            this.Property(t => t.LevelPositionName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("LevelPosition", "organization");
            this.Property(t => t.LevelPositionID).HasColumnName("LevelPositionID");
            this.Property(t => t.LevelPositionName).HasColumnName("LevelPositionName");
        }
    }
}
