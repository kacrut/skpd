using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class RequestInProgramMap : EntityTypeConfiguration<RequestInProgram>
    {
        public RequestInProgramMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestInProgramID);

            // Properties
            // Table & Column Mappings
            this.ToTable("RequestInProgram", "skpd");
            this.Property(t => t.RequestInProgramID).HasColumnName("RequestInProgramID");
            this.Property(t => t.RequestID).HasColumnName("RequestID");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");

            // Relationships
            this.HasRequired(t => t.Program)
                .WithMany(t => t.RequestInPrograms)
                .HasForeignKey(d => d.ProgramID);
            this.HasRequired(t => t.Request)
                .WithMany(t => t.RequestInPrograms)
                .HasForeignKey(d => d.RequestID);

        }
    }
}
