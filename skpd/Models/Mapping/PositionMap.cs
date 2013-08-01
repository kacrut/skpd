using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class PositionMap : EntityTypeConfiguration<Position>
    {
        public PositionMap()
        {
            // Primary Key
            this.HasKey(t => t.PositionID);

            // Properties
            this.Property(t => t.PositionName)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Position", "organization");
            this.Property(t => t.PositionID).HasColumnName("PositionID");
            this.Property(t => t.PositionName).HasColumnName("PositionName");
            this.Property(t => t.UnitID).HasColumnName("UnitID");
            this.Property(t => t.CountryID).HasColumnName("CountryID");
            this.Property(t => t.AssignID).HasColumnName("AssignID");
            this.Property(t => t.PositionTypeID).HasColumnName("PositionTypeID");
            this.Property(t => t.SignID).HasColumnName("SignID");

            // Relationships
            this.HasRequired(t => t.Country)
                .WithMany(t => t.Positions)
                .HasForeignKey(d => d.CountryID);
            this.HasRequired(t => t.Position2)
                .WithMany(t => t.Position1)
                .HasForeignKey(d => d.AssignID);
            this.HasRequired(t => t.PositionType)
                .WithMany(t => t.Positions)
                .HasForeignKey(d => d.PositionTypeID);
            this.HasOptional(t => t.Position3)
                .WithMany(t => t.Position11)
                .HasForeignKey(d => d.SignID);
            this.HasRequired(t => t.Unit)
                .WithMany(t => t.Positions)
                .HasForeignKey(d => d.UnitID);

        }
    }
}
