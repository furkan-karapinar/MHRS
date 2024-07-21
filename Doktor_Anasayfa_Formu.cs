using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHRS
{
    public partial class Doktor_Anasayfa_Formu : Form
    {
        // Veritabanı nesnesi oluşturuluyor.
        Veritabani veritabani = new Veritabani();
        // Kullanıcı ID'si ve şehir, hastane bilgilerini tutacak değişkenler tanımlanıyor.
        int id;
        string sehir;
        string hastane;

        // Doktor_Anasayfa_Formu sınıfının yapıcı metodu, bir kullanıcı ID'si alır.
        public Doktor_Anasayfa_Formu(int _id)
        {
            // Form bileşenleri başlatılıyor.
            InitializeComponent();
            // Formda bir etiket içerisinde kullanıcının adı soyadı gösteriliyor.
            label1.Text = "İyi günler, " + veritabani.KullaniciAdSoyadGetir(_id);
            // Kullanıcı ID'si atanıyor.
            id = _id;

            // Tabloyu yükleme fonksiyonu çağrılıyor.
            tablo_yukle();
        }

        // Tabloyu yükleme fonksiyonu
        private void tablo_yukle()
        {
            // Kullanıcının şehir ve hastane bilgileri alınıyor.
            sehir = veritabani.KullaniciSehirGetir(id);
            hastane = veritabani.KullaniciHastaneGetir(id);
            // Veritabanından doktorun muayene bilgileri alınıyor.
            DataTable dataTable = veritabani.DoktorMHRSGetir(sehir, hastane, veritabani.KullaniciAdSoyadGetir(id));

            // DataGridView'e veri kaynağı olarak alınan veri tablosu atanıyor.
            dataGridView1.DataSource = dataTable;
            // Belirli sütunlar gizleniyor veya başlık metinleri değiştiriliyor.
            dataGridView1.Columns["mhrs_id"].Visible = false;
            dataGridView1.Columns["mhrs_hasta_id"].Visible = false;
            dataGridView1.Columns["mhrs_tarih"].HeaderText = "Tarih";
            dataGridView1.Columns["mhrs_saat"].HeaderText = "Saat";
            dataGridView1.Columns["mhrs_doktor"].Visible = false;
            dataGridView1.Columns["mhrs_klinik"].HeaderText = "Bölüm";
            dataGridView1.Columns["hasta_ad_soyad"].HeaderText = "Hasta Tam Adı";
        }

        // İşlem butonuna tıklandığında çağrılan fonksiyon
        private void button3_Click(object sender, EventArgs e)
        {
            // Seçilen satırdaki hasta adı soyadı bilgisini alıyor.
            string value = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            // Bir mesaj kutusu ile hasta adını ve "çağırıldı" mesajını gösteriyor.
            MessageBox.Show(String.Format("{0} adlı hasta çağırıldı!", value));
        }

    }
}
