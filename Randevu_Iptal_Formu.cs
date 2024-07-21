using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHRS
{
    public partial class Randevu_Iptal_Formu : Form
    {
        // Veritabanı nesnesi oluşturuluyor.
        Veritabani veritabani = new Veritabani();

        // Randevu İptal Formu sınıfı, ID, şehir ve hastane adlarıyla başlatılıyor.
        int id;
        string sehir;
        string hastane;

        // Randevu_Iptal_Formu sınıfının yapıcı metodu, bir kullanıcı ID'si alarak başlatılıyor.
        public Randevu_Iptal_Formu(int _id)
        {
            // Form bileşenleri başlatılıyor.
            InitializeComponent();

            // Form üzerindeki bir label bileşenine kullanıcının adını ve soyadını ekliyoruz.
            label1.Text = "İyi günler, " + veritabani.KullaniciAdSoyadGetir(_id);

            // ID, şehir ve hastane bilgileri atanıyor.
            id = _id;
            sehir = veritabani.KullaniciSehirGetir(_id);
            hastane = veritabani.KullaniciHastaneGetir(_id);

            // Tabloyu dolduran fonksiyon çağrılıyor.
            tablo_yukle();
        }

        // İptal butonuna tıklandığında çalışacak olan fonksiyon.
        private void button3_Click(object sender, EventArgs e)
        {
            // Eğer kullanıcı bir satır seçmişse:
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Seçilen satırdaki MHRSSil ID'sini alıyoruz.
                string idValue = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                // Veritabanından bu ID'ye sahip randevuyu sil.
                veritabani.MHRSSil(idValue);

                // Kullanıcıya randevunun silindiğine dair bir mesaj göster.
                MessageBox.Show("Randevu Silindi!");

                // Tabloyu yeniden yükle.
                tablo_yukle();
            }
        }

        // Tabloyu dolduran fonksiyon.
        private void tablo_yukle()
        {
            // Veritabanından şehir ve hastane bilgileriyle bugünden sonraki randevu tarihlerini getir.
            DataTable dataTable = veritabani.BugundenSonrakiMHRSTarihleriniGetir(sehir, hastane);

            // DataGridView bileşeninin veri kaynağını ayarla.
            dataGridView1.DataSource = dataTable;

            // Bazı sütunları görünmez yap.
            dataGridView1.Columns["mhrs_id"].Visible = false;
            dataGridView1.Columns["mhrs_hasta_id"].Visible = false;

            // Sütun başlıklarını Türkçeleştir.
            dataGridView1.Columns["mhrs_tarih"].HeaderText = "Tarih";
            dataGridView1.Columns["mhrs_saat"].HeaderText = "Saat";
            dataGridView1.Columns["mhrs_doktor"].HeaderText = "Doktor";
            dataGridView1.Columns["mhrs_klinik"].HeaderText = "Bölüm";
            dataGridView1.Columns["hasta_ad_soyad"].HeaderText = "Hasta Tam Adı";
        }

    }
}
