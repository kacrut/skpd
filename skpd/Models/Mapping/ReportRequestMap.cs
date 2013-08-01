using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class ReportRequestMap : EntityTypeConfiguration<ReportRequest>
    {
        public ReportRequestMap()
        {
            // Primary Key
            this.HasKey(t => new { t.NO__SKPD, t.JABATAN, t.BAGIAN, t.TGL__PERGI, t.TGL__PULANG, t.HARI, t.MATA_UANG, t.KURS, t.TGL__KURS, t.BERMALAM_, t.UANG_HARIAN, t.KONVERSI_UANG_HARIAN, t.TOTAL_UANG_HARIAN, t.KODE_MATA_ANGGARAN, t.NAMA_MATA_ANGGARAN, t.TGL_PENGAJUAN });

            // Properties
            this.Property(t => t.NO__SKPD)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NAMA_PEGAWAI)
                .HasMaxLength(8000);

            this.Property(t => t.JABATAN)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.BAGIAN)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.REKENING)
                .HasMaxLength(50);

            this.Property(t => t.EMAIL)
                .HasMaxLength(50);

            this.Property(t => t.NIK)
                .HasMaxLength(50);

            this.Property(t => t.NPWP)
                .HasMaxLength(50);

            this.Property(t => t.ALAMAT)
                .HasMaxLength(150);

            this.Property(t => t.AREA_ASAL)
                .HasMaxLength(100);

            this.Property(t => t.NEGARA_ASAL)
                .HasMaxLength(100);

            this.Property(t => t.LOKASI_ASAL)
                .HasMaxLength(100);

            this.Property(t => t.AREA_TUJUAN)
                .HasMaxLength(100);

            this.Property(t => t.NEGARA_TUJUAN)
                .HasMaxLength(100);

            this.Property(t => t.LOKASI_TUJUAN)
                .HasMaxLength(100);

            this.Property(t => t.KEGIATAN)
                .HasMaxLength(50);

            this.Property(t => t.HARI)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.MATA_UANG)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.KURS)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.BERMALAM_)
                .IsRequired()
                .HasMaxLength(11);

            this.Property(t => t.UANG_HARIAN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.KONVERSI_UANG_HARIAN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TOTAL_UANG_HARIAN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.KODE_MATA_ANGGARAN)
                .IsRequired()
                .HasMaxLength(7);

            this.Property(t => t.NAMA_MATA_ANGGARAN)
                .IsRequired()
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("ReportRequest", "skpd");
            this.Property(t => t.NO__SKPD).HasColumnName("NO. SKPD");
            this.Property(t => t.NAMA_PEGAWAI).HasColumnName("NAMA PEGAWAI");
            this.Property(t => t.JABATAN).HasColumnName("JABATAN");
            this.Property(t => t.BAGIAN).HasColumnName("BAGIAN");
            this.Property(t => t.REKENING).HasColumnName("REKENING");
            this.Property(t => t.EMAIL).HasColumnName("EMAIL");
            this.Property(t => t.NIK).HasColumnName("NIK");
            this.Property(t => t.NPWP).HasColumnName("NPWP");
            this.Property(t => t.ALAMAT).HasColumnName("ALAMAT");
            this.Property(t => t.AREA_ASAL).HasColumnName("AREA ASAL");
            this.Property(t => t.NEGARA_ASAL).HasColumnName("NEGARA ASAL");
            this.Property(t => t.LOKASI_ASAL).HasColumnName("LOKASI ASAL");
            this.Property(t => t.AREA_TUJUAN).HasColumnName("AREA TUJUAN");
            this.Property(t => t.NEGARA_TUJUAN).HasColumnName("NEGARA TUJUAN");
            this.Property(t => t.LOKASI_TUJUAN).HasColumnName("LOKASI TUJUAN");
            this.Property(t => t.TGL__PERGI).HasColumnName("TGL. PERGI");
            this.Property(t => t.TGL__PULANG).HasColumnName("TGL. PULANG");
            this.Property(t => t.KEGIATAN).HasColumnName("KEGIATAN");
            this.Property(t => t.HARI).HasColumnName("HARI");
            this.Property(t => t.MATA_UANG).HasColumnName("MATA UANG");
            this.Property(t => t.KURS).HasColumnName("KURS");
            this.Property(t => t.TGL__KURS).HasColumnName("TGL. KURS");
            this.Property(t => t.BERMALAM_).HasColumnName("BERMALAM?");
            this.Property(t => t.UANG_HARIAN).HasColumnName("UANG HARIAN");
            this.Property(t => t.KONVERSI_UANG_HARIAN).HasColumnName("KONVERSI UANG HARIAN");
            this.Property(t => t.TOTAL_UANG_HARIAN).HasColumnName("TOTAL UANG HARIAN");
            this.Property(t => t.KODE_MATA_ANGGARAN).HasColumnName("KODE MATA ANGGARAN");
            this.Property(t => t.NAMA_MATA_ANGGARAN).HasColumnName("NAMA MATA ANGGARAN");
            this.Property(t => t.TGL_PENGAJUAN).HasColumnName("TGL PENGAJUAN");
            this.Property(t => t.TGL_SKPD).HasColumnName("TGL SKPD");
        }
    }
}
