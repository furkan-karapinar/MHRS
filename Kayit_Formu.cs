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
    public partial class Kayit_Formu : Form
    {
        // Veritabanı bağlantısını oluşturmak için bir nesne oluşturuyoruz.
        Veritabani veritabani = new Veritabani();

        // Kayit_Formu sınıfının kurucu metodunu tanımlıyoruz.
        public Kayit_Formu()
        {
            // Windows Forms bileşenlerini başlatıyoruz.
            InitializeComponent();
        }

        // Form yüklendiğinde çalışacak olan olay işleyicisi
        private void Kayit_Formu_Load(object sender, EventArgs e)
        {
            // Veritabanından bölüm listesini al
            List<string> bolumler = veritabani.BolumListesiAl();

            // ComboBox'a bölümleri eklemek için döngü kullanıyoruz
            foreach (string bolum in bolumler)
            {
                comboBox2.Items.Add(bolum);
            }

            // Veritabanından şehir listesini al
            List<string> sehirler = veritabani.SehirListesiAl();

            // ComboBox'a şehirleri eklemek için döngü kullanıyoruz
            foreach (string sehir in sehirler)
            {
                comboBox3.Items.Add(sehir);
            }

            // Bazı varsayılan seçenekleri seçili hale getiriyoruz
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }

        // "Geri" linkine tıklandığında çalışacak olan olay işleyicisi
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Formu kapatıyoruz
            this.Close();
        }

        // ComboBox'taki bir öğe seçildiğinde çalışacak olan olay işleyicisi
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ComboBox'ta seçilen öğeye göre bazı işlemleri yapıyoruz
            if (comboBox1.SelectedIndex == 1)
            {
                comboBox2.Enabled = false;
            }
            else
            {
                comboBox2.Enabled = true;
            }
        }

        // Kaydet butonuna tıklandığında çalışacak olan olay işleyicisi
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Gerekli alanların doldurulup doldurulmadığını kontrol ediyoruz
                if (textBox3.Text != String.Empty || textBox4.Text != String.Empty || textBox2.Text != String.Empty || textBox1.Text != String.Empty)
                {
                    // ComboBox'ta seçilen bir değere göre kullanıcı ekliyoruz
                    if (comboBox1.SelectedIndex == 1)
                    {
                        veritabani.KullaniciEkle((textBox1.Text + " " + textBox2.Text), textBox3.Text, textBox4.Text, comboBox1.Text, comboBox3.Text, comboBox4.Text);
                    }
                    else
                    {
                        var kullanici_id = veritabani.KullaniciEkle((textBox1.Text + " " + textBox2.Text), textBox3.Text, textBox4.Text, comboBox1.Text, comboBox3.Text, comboBox4.Text);
                        veritabani.DoktorEkle(kullanici_id, (comboBox2.SelectedIndex + 1));
                    }
                    // Kullanıcıya başarılı bir şekilde kaydedildiğini bildiren bir mesaj gösteriyoruz
                    MessageBox.Show("Kayıt Başarılı!");
                    // Formu kapatıyoruz
                    this.Close();
                }
                else
                {
                    // Kullanıcıya eksik alanları doldurması gerektiğini belirten bir mesaj gösteriyoruz
                    MessageBox.Show("Lütfen boş alanları doldurunuz!");
                }

            }
            catch (Exception ex)
            {
                // Herhangi bir hata oluştuğunda kullanıcıya hata mesajını gösteriyoruz
                MessageBox.Show("Hata: " + ex);
            }
        }

        // Text kutusuna tuş basıldığında çalışacak olan olay işleyicisi
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Girilen karakterin bir rakam mı yoksa kontrol tuşlarından biri mi olduğunu kontrol ediyoruz
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Eğer girilen karakter bir rakam veya kontrol tuşu değilse, tuş basma olayını iptal ediyoruz
                e.Handled = true;
            }
        }

        // ComboBox'taki bir öğe seçildiğinde çalışacak olan olay işleyicisi
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // ComboBox'ta seçilen şehre göre hastane listesini alıyoruz
                comboBox4.Items.Clear();
                List<string> hastaneler = veritabani.HastaneListesiAl(comboBox3.SelectedIndex + 1);

                // ComboBox'a hastaneleri eklemek için döngü kullanıyoruz
                foreach (string hastane in hastaneler)
                {
                    comboBox4.Items.Add(hastane);
                }
                // Varsayılan olarak ilk hastaneyi seçili hale getiriyoruz
                comboBox4.SelectedIndex = 0;
            }
            catch (Exception ex) { }
        }

    }
}
