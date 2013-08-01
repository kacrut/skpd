using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class vwUserProfileMap : EntityTypeConfiguration<vwUserProfile>
    {
        public vwUserProfileMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.Username, t.FirstName, t.Email, t.PositionID, t.PositionName, t.PositionTypeID, t.PositionTypeName, t.LevelPositionID, t.LevelPositionName, t.UnitID, t.UnitName, t.DivisionID, t.DivisionName, t.CountryID, t.CountryName, t.AssignID, t.Assign, t.SignID });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(56);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LastName)
                .HasMaxLength(50);

            this.Property(t => t.fullname)
                .HasMaxLength(101);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PositionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PositionName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PositionTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PositionTypeName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LevelPositionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LevelPositionName)
                .IsRequired()
                .HasMaxLength(50);

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

            this.Property(t => t.CountryID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CountryName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.AssignID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Assign)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.SignID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SignName)
                .HasMaxLength(159);

            this.Property(t => t.FaceUrl)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("vwUserProfile", "skpd");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.fullname).HasColumnName("fullname");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.PositionID).HasColumnName("PositionID");
            this.Property(t => t.PositionName).HasColumnName("PositionName");
            this.Property(t => t.PositionTypeID).HasColumnName("PositionTypeID");
            this.Property(t => t.PositionTypeName).HasColumnName("PositionTypeName");
            this.Property(t => t.LevelPositionID).HasColumnName("LevelPositionID");
            this.Property(t => t.LevelPositionName).HasColumnName("LevelPositionName");
            this.Property(t => t.UnitID).HasColumnName("UnitID");
            this.Property(t => t.UnitName).HasColumnName("UnitName");
            this.Property(t => t.DivisionID).HasColumnName("DivisionID");
            this.Property(t => t.DivisionName).HasColumnName("DivisionName");
            this.Property(t => t.CountryID).HasColumnName("CountryID");
            this.Property(t => t.CountryName).HasColumnName("CountryName");
            this.Property(t => t.AssignID).HasColumnName("AssignID");
            this.Property(t => t.Assign).HasColumnName("Assign");
            this.Property(t => t.SignID).HasColumnName("SignID");
            this.Property(t => t.SignName).HasColumnName("SignName");
            this.Property(t => t.FaceUrl).HasColumnName("FaceUrl");
        }
    }
}
