using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class RequestMap : EntityTypeConfiguration<Request>
    {
        public RequestMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestID);

            // Properties
            this.Property(t => t.EventName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.RejectedReason)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Request", "skpd");
            this.Property(t => t.RequestID).HasColumnName("RequestID");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.FromCountryID).HasColumnName("FromCountryID");
            this.Property(t => t.ToCountryID).HasColumnName("ToCountryID");
            this.Property(t => t.EventName).HasColumnName("EventName");
            this.Property(t => t.PositionID).HasColumnName("PositionID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.FlagID).HasColumnName("FlagID");
            this.Property(t => t.FlagCreatedDate).HasColumnName("FlagCreatedDate");
            this.Property(t => t.ApprovalPositionID).HasColumnName("ApprovalPositionID");
            this.Property(t => t.RejectedReason).HasColumnName("RejectedReason");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Requests)
                .HasForeignKey(d => d.UserID);
            this.HasRequired(t => t.Country)
                .WithMany(t => t.Requests)
                .HasForeignKey(d => d.FromCountryID);
            this.HasRequired(t => t.Country1)
                .WithMany(t => t.Requests1)
                .HasForeignKey(d => d.ToCountryID);
            this.HasRequired(t => t.Position)
                .WithMany(t => t.Requests)
                .HasForeignKey(d => d.PositionID);
            this.HasRequired(t => t.Position1)
                .WithMany(t => t.Requests1)
                .HasForeignKey(d => d.ApprovalPositionID);
            this.HasRequired(t => t.Flag)
                .WithMany(t => t.Requests)
                .HasForeignKey(d => d.FlagID);

        }
    }
}
