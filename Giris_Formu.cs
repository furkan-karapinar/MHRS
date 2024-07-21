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
    public partial class Giris_Formu : Form
    {
        // Bir Veritabani nesnesi oluşturuluyor.
        Veritabani veritabani = new Veritabani();

        // Giriş Formu sınıfının kurucu metodudur.
        public Giris_Formu()
        {
            // Formun bileşenleri başlatılıyor.
            InitializeComponent();
        }

        // Kayıt linkine tıklandığında gerçekleşecek olayı belirleyen metot.
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Giriş formunu gizle.
            this.Hide();

            // Kayıt formunu oluştur.
            Kayit_Formu kayit = new Kayit_Formu();

            // Kayıt formunu göster.
            kayit.ShowDialog();

            // Giriş formunu tekrar göster.
            this.Show();
        }

        // Giriş butonuna tıklandığında gerçekleşecek olayı belirleyen metot.
        private void button1_Click(object sender, EventArgs e)
        {
            // Kullanıcı adı veya şifre boş değilse:
            if (textBox1.Text != string.Empty || textBox2.Text != string.Empty)
            {
                // Kullanıcıyı doğrula ve kullanıcı kimliğini al.
                int id = veritabani.KullaniciDogrula(textBox1.Text, textBox2.Text);

                // Kullanıcı bulunduysa:
                if (id != -1)
                {
                    // Kullanıcının türünü kontrol et: Doktor mu?
                    if (veritabani.KullaniciTurunuGetir(id) == "Doktor")
                    {
                        // Eğer kullanıcı bir doktorsa, doktor ana sayfa formunu aç.
                        this.Hide();
                        Doktor_Anasayfa_Formu doktor = new Doktor_Anasayfa_Formu(id);
                        doktor.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        // Doktor değilse, çalışan ana sayfa formunu aç.
                        this.Hide();
                        Calisan_Anasayfa_Formu calisan = new Calisan_Anasayfa_Formu(id);
                        calisan.ShowDialog();
                        this.Show();
                    }
                }
                else
                {
                    // Kullanıcı bulunamadıysa hata mesajı göster.
                    MessageBox.Show("Kullanıcı bulunamadı!");
                }
            }
        }

        // Kullanıcı adı alanına sadece rakam girilmesini sağlayan metot.
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Girilen karakter bir kontrol karakteri veya rakam değilse:
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Tuşa basımı engelle.
                e.Handled = true;
            }
        }

    }
}
