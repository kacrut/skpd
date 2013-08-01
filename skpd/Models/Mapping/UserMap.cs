using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.NIK)
                .HasMaxLength(50);

            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(56);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LastName)
                .HasMaxLength(50);

            this.Property(t => t.Rekening)
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Npwp)
                .HasMaxLength(50);

            this.Property(t => t.Alamat)
                .HasMaxLength(150);

            this.Property(t => t.FaceUrl)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("User");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.NIK).HasColumnName("NIK");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.Rekening).HasColumnName("Rekening");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.PositionID).HasColumnName("PositionID");
            this.Property(t => t.Npwp).HasColumnName("Npwp");
            this.Property(t => t.Alamat).HasColumnName("Alamat");
            this.Property(t => t.FaceUrl).HasColumnName("FaceUrl");
            this.Property(t => t.isActive).HasColumnName("isActive");

            // Relationships
            this.HasRequired(t => t.Position)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.PositionID);

        }
    }
}
