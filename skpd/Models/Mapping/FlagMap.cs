using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class FlagMap : EntityTypeConfiguration<Flag>
    {
        public FlagMap()
        {
            // Primary Key
            this.HasKey(t => t.FlagID);

            // Properties
            this.Property(t => t.FlagName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Flag", "skpd");
            this.Property(t => t.FlagID).HasColumnName("FlagID");
            this.Property(t => t.FlagName).HasColumnName("FlagName");
        }
    }
}
