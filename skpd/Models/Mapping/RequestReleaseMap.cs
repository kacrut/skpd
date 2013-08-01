using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class RequestReleaseMap : EntityTypeConfiguration<RequestRelease>
    {
        public RequestReleaseMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestReleaseID);

            // Properties
            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(56);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(101);

            this.Property(t => t.PositionCountryName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PositionTypeName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LevelPositionName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PositionName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.UnitName)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.DivisionName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Rekening)
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .HasMaxLength(50);

            this.Property(t => t.NIK)
                .HasMaxLength(50);

            this.Property(t => t.Npwp)
                .HasMaxLength(50);

            this.Property(t => t.Alamat)
                .HasMaxLength(150);

            this.Property(t => t.ToCountryAreaName)
                .HasMaxLength(100);

            this.Property(t => t.ToRegionName)
                .HasMaxLength(100);

            this.Property(t => t.ToCountryName)
                .HasMaxLength(100);

            this.Property(t => t.FromCountryAreaName)
                .HasMaxLength(100);

            this.Property(t => t.FromRegionName)
                .HasMaxLength(100);

            this.Property(t => t.FromCountryName)
                .HasMaxLength(100);

            this.Property(t => t.EventName)
                .HasMaxLength(50);

            this.Property(t => t.CurrencyName)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.CashStayOrCashNotStay)
                .IsRequired()
                .HasMaxLength(11);

            this.Property(t => t.ProgramID)
                .IsRequired()
                .HasMaxLength(7);

            this.Property(t => t.ProgramName)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.SignName)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("RequestRelease", "skpd");
            this.Property(t => t.RequestReleaseID).HasColumnName("RequestReleaseID");
            this.Property(t => t.RequestID).HasColumnName("RequestID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.PositionCountryID).HasColumnName("PositionCountryID");
            this.Property(t => t.PositionCountryName).HasColumnName("PositionCountryName");
            this.Property(t => t.PositionTypeID).HasColumnName("PositionTypeID");
            this.Property(t => t.PositionTypeName).HasColumnName("PositionTypeName");
            this.Property(t => t.LevelPositionID).HasColumnName("LevelPositionID");
            this.Property(t => t.LevelPositionName).HasColumnName("LevelPositionName");
            this.Property(t => t.PositionID).HasColumnName("PositionID");
            this.Property(t => t.PositionName).HasColumnName("PositionName");
            this.Property(t => t.UnitID).HasColumnName("UnitID");
            this.Property(t => t.UnitName).HasColumnName("UnitName");
            this.Property(t => t.DivisionID).HasColumnName("DivisionID");
            this.Property(t => t.DivisionName).HasColumnName("DivisionName");
            this.Property(t => t.Rekening).HasColumnName("Rekening");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.NIK).HasColumnName("NIK");
            this.Property(t => t.Npwp).HasColumnName("Npwp");
            this.Property(t => t.Alamat).HasColumnName("Alamat");
            this.Property(t => t.ToCountryAreaID).HasColumnName("ToCountryAreaID");
            this.Property(t => t.ToCountryAreaName).HasColumnName("ToCountryAreaName");
            this.Property(t => t.ToRegionID).HasColumnName("ToRegionID");
            this.Property(t => t.ToRegionName).HasColumnName("ToRegionName");
            this.Property(t => t.CountryID).HasColumnName("CountryID");
            this.Property(t => t.ToCountryName).HasColumnName("ToCountryName");
            this.Property(t => t.FromCountryAreaID).HasColumnName("FromCountryAreaID");
            this.Property(t => t.FromCountryAreaName).HasColumnName("FromCountryAreaName");
            this.Property(t => t.FromRegionID).HasColumnName("FromRegionID");
            this.Property(t => t.FromRegionName).HasColumnName("FromRegionName");
            this.Property(t => t.FromCountryID).HasColumnName("FromCountryID");
            this.Property(t => t.FromCountryName).HasColumnName("FromCountryName");
            this.Property(t => t.EventName).HasColumnName("EventName");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Days).HasColumnName("Days");
            this.Property(t => t.CurrencyName).HasColumnName("CurrencyName");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.ExchangeDate).HasColumnName("ExchangeDate");
            this.Property(t => t.CashStayOrCashNotStay).HasColumnName("CashStayOrCashNotStay");
            this.Property(t => t.CashPerDay).HasColumnName("CashPerDay");
            this.Property(t => t.ConvertCashPerDay).HasColumnName("ConvertCashPerDay");
            this.Property(t => t.TotalCash).HasColumnName("TotalCash");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");
            this.Property(t => t.ProgramName).HasColumnName("ProgramName");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.FlagID).HasColumnName("FlagID");
            this.Property(t => t.FlagCreatedDate).HasColumnName("FlagCreatedDate");
            this.Property(t => t.SignID).HasColumnName("SignID");
            this.Property(t => t.SignName).HasColumnName("SignName");

            // Relationships
            this.HasRequired(t => t.Request)
                .WithMany(t => t.RequestReleases)
                .HasForeignKey(d => d.RequestID);

        }
    }
}
