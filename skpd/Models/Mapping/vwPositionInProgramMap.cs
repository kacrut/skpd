using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class vwPositionInProgramMap : EntityTypeConfiguration<vwPositionInProgram>
    {
        public vwPositionInProgramMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProgramID, t.ProgramName, t.UnitID, t.UnitName, t.DivisionID, t.DivisionName, t.PositionID, t.fullname, t.FLag });

            // Properties
            this.Property(t => t.ProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProgramName)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.UnitID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UnitName)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.DivisionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.DivisionName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PositionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.fullname)
                .IsRequired()
                .HasMaxLength(56);

            // Table & Column Mappings
            this.ToTable("vwPositionInProgram", "skpd");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");
            this.Property(t => t.ProgramName).HasColumnName("ProgramName");
            this.Property(t => t.UnitID).HasColumnName("UnitID");
            this.Property(t => t.UnitName).HasColumnName("UnitName");
            this.Property(t => t.DivisionID).HasColumnName("DivisionID");
            this.Property(t => t.DivisionName).HasColumnName("DivisionName");
            this.Property(t => t.PositionID).HasColumnName("PositionID");
            this.Property(t => t.fullname).HasColumnName("fullname");
            this.Property(t => t.FLag).HasColumnName("FLag");
        }
    }
}
