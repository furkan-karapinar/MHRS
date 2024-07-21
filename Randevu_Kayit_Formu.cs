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
    public partial class Randevu_Kayit_Formu : Form
    {
        
        int id;
        string sehir;
        string hastane;
        // Veritabanı nesnesi oluşturuluyor
        Veritabani veritabani = new Veritabani();

        // Randevu_Kayit_Formu sınıfı tanımlanıyor ve constructor metodu tanımlanıyor
        public Randevu_Kayit_Formu(int _id)
        {
            // Form bileşenleri başlatılıyor
            InitializeComponent();

            // SaatEkle metodu çağrılıyor
            SaatEkle();

            // id değişkeni _id parametresine eşitleniyor
            id = _id;

            // Kullanıcının şehrini ve hastanesini veritabanından alınıyor
            sehir = veritabani.KullaniciSehirGetir(_id);
            hastane = veritabani.KullaniciHastaneGetir(_id);

            // Bölüm listesi alınıyor
            List<string> bolumler = veritabani.BolumListesiAl();

            // ComboBox'a bölümleri eklemek için döngü oluşturuluyor
            foreach (string bolum in bolumler)
            {
                comboBox1.Items.Add(bolum);
            }

            // ComboBox'ın seçili öğesi belirleniyor
            comboBox1.SelectedIndex = 0;
        }

        // Form yüklenirken çalışacak olan metot
        private void Randevu_Kayit_Formu_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        // ComboBox1'deki seçili öğe değiştiğinde çalışacak metot
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Seçili bölüme göre doktorların adları ve soyadları alınıyor
                List<string> doktorlar = veritabani.DoktorAdSoyadlariniGetir((comboBox1.SelectedIndex + 1),hastane);
                comboBox2.Items.Clear();
                foreach (string doktor in doktorlar)
                {
                    comboBox2.Items.Add(doktor);
                }
                comboBox2.SelectedIndex = 0;

                // Randevu kontrolü yapılıyor
                randevu_kontrol();
            }
            catch (Exception ex) { }
        }

        // Randevu kontrolünü gerçekleştiren metot
        private void randevu_kontrol()
        {
            SaatEkle();
            List<string> alinan_saatler = veritabani.MHRSSaatleriGetir(comboBox2.Text, dateTimePicker1.Value);
            foreach (var item in alinan_saatler)
            {
                comboBox3.Items.Remove(item);
            }
        }

        // Saatleri ComboBox'a ekleyen metot
        private void SaatEkle()
        {
            comboBox3.Items.Clear();
            TimeSpan baslangic = new TimeSpan(9, 0, 0);
            TimeSpan bitis = new TimeSpan(16, 0, 0);
            TimeSpan interval = TimeSpan.FromMinutes(10);
            TimeSpan suan = baslangic;

            while (suan < bitis)
            {
                if (!(suan >= new TimeSpan(12, 0, 0) && suan < new TimeSpan(14, 0, 0)))
                {
                    comboBox3.Items.Add(suan.ToString(@"hh\:mm"));
                }

                suan = suan.Add(interval);
            }
            comboBox3.SelectedIndex = 0;
        }

        // Kayıt butonuna tıklandığında çalışacak olan metot
        private void button1_Click(object sender, EventArgs e)
        {
            // Text kutularının boş olup olmadığı kontrol ediliyor
            if (textBox1.Text != string.Empty || textBox3.Text != string.Empty || textBox4.Text != string.Empty)
            {
                try
                {
                    // Hasta ID'si alınıyor
                    int hasta_id = veritabani.HastaIdGetir(textBox1.Text);
                    // Eğer hasta veritabanında yoksa hasta ekleniyor
                    if (hasta_id == -1)
                    {
                        hasta_id = veritabani.HastaEkleVeIdDondur(textBox4.Text + " " + textBox3.Text, textBox1.Text, textBox1.Text);
                    }

                    // Randevu ekleniyor
                    veritabani.MHRSEkle(hasta_id, sehir, dateTimePicker1.Value.ToString("yyyy-MM-dd"), comboBox3.Text, hastane, comboBox2.Text, comboBox1.Text);
                    MessageBox.Show("Kayıt Oluşturuldu!");
                }
                catch (Exception ex) { MessageBox.Show("Hata: " + ex); }
            }
            else { MessageBox.Show("Lütfen boş alanları doldurunuz!"); }
        }

        // ComboBox2'deki seçili öğe değiştiğinde çalışacak metot
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            randevu_kontrol();
        }

        // DateTimePicker'daki tarih değiştiğinde çalışacak metot
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            randevu_kontrol();
        }

    }
}
