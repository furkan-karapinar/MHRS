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
    public partial class Calisan_Anasayfa_Formu : Form
    {
        // Bir Veritabani nesnesi oluşturuyoruz ve bu nesneyi 'veritabani' adıyla tanımlıyoruz.
        Veritabani veritabani = new Veritabani();

        // Bir tamsayı değişkeni olan 'id' tanımlıyoruz.
        int id;

        // Calisan_Anasayfa_Formu sınıfının yapıcı metodu. Bir tamsayı parametresi alır.
        public Calisan_Anasayfa_Formu(int _id)
        {
            // Form bileşenlerini başlatıyoruz.
            InitializeComponent();

            // Formun label1 bileşeninin metin özelliğini belirliyoruz. Kullanıcı adı ve soyadını getiren metodu kullanarak bir metin oluşturuyoruz.
            label1.Text = "İyi günler, " + veritabani.KullaniciAdSoyadGetir(_id);

            // 'id' değişkenine parametre olarak gelen '_id' değerini atıyoruz.
            id = _id;
        }

        // button1 adlı düğmenin tıklama olayı.
        private void button1_Click(object sender, EventArgs e)
        {
            // Bu formu gizliyoruz.
            this.Hide();

            // Randevu_Kayit_Formu sınıfından bir nesne oluşturuyoruz ve 'id' değişkenini parametre olarak iletiyoruz.
            Randevu_Kayit_Formu randevu = new Randevu_Kayit_Formu(id);

            // Oluşturduğumuz formu diyalog olarak gösteriyoruz.
            randevu.ShowDialog();

            // Bu formu tekrar gösteriyoruz.
            this.Show();
        }

        // button2 adlı düğmenin tıklama olayı.
        private void button2_Click(object sender, EventArgs e)
        {
            // Bu formu gizliyoruz.
            this.Hide();

            // Randevu_Iptal_Formu sınıfından bir nesne oluşturuyoruz ve 'id' değişkenini parametre olarak iletiyoruz.
            Randevu_Iptal_Formu randevu_iptal = new Randevu_Iptal_Formu(id);

            // Oluşturduğumuz formu diyalog olarak gösteriyoruz.
            randevu_iptal.ShowDialog();

            // Bu formu tekrar gösteriyoruz.
            this.Show();
        }
    }
}
