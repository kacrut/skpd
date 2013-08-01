using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class ReportRequest
    {
        public int NO__SKPD { get; set; }
        public string NAMA_PEGAWAI { get; set; }
        public string JABATAN { get; set; }
        public string BAGIAN { get; set; }
        public string REKENING { get; set; }
        public string EMAIL { get; set; }
        public string NIK { get; set; }
        public string NPWP { get; set; }
        public string ALAMAT { get; set; }
        public string AREA_ASAL { get; set; }
        public string NEGARA_ASAL { get; set; }
        public string LOKASI_ASAL { get; set; }
        public string AREA_TUJUAN { get; set; }
        public string NEGARA_TUJUAN { get; set; }
        public string LOKASI_TUJUAN { get; set; }
        public System.DateTime TGL__PERGI { get; set; }
        public System.DateTime TGL__PULANG { get; set; }
        public string KEGIATAN { get; set; }
        public int HARI { get; set; }
        public string MATA_UANG { get; set; }
        public decimal KURS { get; set; }
        public System.DateTime TGL__KURS { get; set; }
        public string BERMALAM_ { get; set; }
        public decimal UANG_HARIAN { get; set; }
        public decimal KONVERSI_UANG_HARIAN { get; set; }
        public decimal TOTAL_UANG_HARIAN { get; set; }
        public string KODE_MATA_ANGGARAN { get; set; }
        public string NAMA_MATA_ANGGARAN { get; set; }
        public System.DateTime TGL_PENGAJUAN { get; set; }
        public Nullable<System.DateTime> TGL_SKPD { get; set; }
    }
}
