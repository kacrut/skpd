using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class RequestInTransportMap : EntityTypeConfiguration<RequestInTransport>
    {
        public RequestInTransportMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestInTransportID);

            // Properties
            // Table & Column Mappings
            this.ToTable("RequestInTransport", "skpd");
            this.Property(t => t.RequestInTransportID).HasColumnName("RequestInTransportID");
            this.Property(t => t.RequestID).HasColumnName("RequestID");
            this.Property(t => t.TransportID).HasColumnName("TransportID");

            // Relationships
            this.HasRequired(t => t.Request)
                .WithMany(t => t.RequestInTransports)
                .HasForeignKey(d => d.RequestID);
            this.HasRequired(t => t.Transport)
                .WithMany(t => t.RequestInTransports)
                .HasForeignKey(d => d.TransportID);

        }
    }
}
