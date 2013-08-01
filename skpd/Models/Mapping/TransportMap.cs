using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class TransportMap : EntityTypeConfiguration<Transport>
    {
        public TransportMap()
        {
            // Primary Key
            this.HasKey(t => t.TransportID);

            // Properties
            this.Property(t => t.TransportName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Description)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Transport", "skpd");
            this.Property(t => t.TransportID).HasColumnName("TransportID");
            this.Property(t => t.TransportName).HasColumnName("TransportName");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
