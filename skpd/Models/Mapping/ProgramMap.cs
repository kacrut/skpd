using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class ProgramMap : EntityTypeConfiguration<Program>
    {
        public ProgramMap()
        {
            // Primary Key
            this.HasKey(t => t.ProgramID);

            // Properties
            this.Property(t => t.ProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProgramName)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Program", "skpd");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");
            this.Property(t => t.PositionID).HasColumnName("PositionID");
            this.Property(t => t.ProgramName).HasColumnName("ProgramName");
            this.Property(t => t.FLag).HasColumnName("FLag");
        }
    }
}
