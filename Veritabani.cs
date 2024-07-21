using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MHRS
{
    internal class Veritabani
    {
        // MySqlConnection sınıfından "baglanti" adında özel bir nesne tanımlıyoruz.
        private MySqlConnection baglanti;

        // Veritabanı bağlantısı için kullanılacak bağlantı dizesini belirliyoruz.
        string connectionString = "Server=127.0.0.1;Database=mhrs_otomasyon;Uid=root;Pwd=;";

        // Veritabanı sınıfının yapıcı metodunu tanımlıyoruz.
        public Veritabani()
        {
            // "baglanti" nesnesini belirtilen bağlantı dizesiyle başlatıyoruz.
            baglanti = new MySqlConnection(connectionString);
        }

        // Veritabanından bölüm listesini almak için bir metod tanımlıyoruz.
        public List<string> BolumListesiAl()
        {
            // Bölüm isimlerini depolamak için bir liste oluşturuyoruz.
            List<string> bolumler = new List<string>();

            // Veritabanından bölüm adlarını seçmek için bir sorgu oluşturuyoruz.
            string sorgu = "SELECT bolum_adi FROM bolumler";

            // MySqlCommand sınıfından bir komut nesnesi oluşturuyoruz ve bu nesneyi sorgu ve bağlantı ile ilişkilendiriyoruz.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                try
                {
                    // Veritabanı bağlantısını açıyoruz.
                    baglanti.Open();

                    // Sorgudan dönen verileri okumak için bir veri okuyucu oluşturuyoruz.
                    using (MySqlDataReader okuyucu = komut.ExecuteReader())
                    {
                        // Veri okuyucu sonuçları satır satır okuyarak işliyor.
                        while (okuyucu.Read())
                        {
                            // Her bir bölüm adını "bolumler" listesine ekliyoruz.
                            bolumler.Add(okuyucu.GetString("bolum_adi"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Bir hata oluştuğunda, hata mesajını konsola yazdırıyoruz.
                    Console.WriteLine("Bölüm listesi alınamadı: " + ex.Message);
                }
                finally
                {
                    // İşlem tamamlandığında veritabanı bağlantısını kapatıyoruz.
                    baglanti.Close();
                }
            }

            // Bölüm listesini geri döndürüyoruz.
            return bolumler;
        }


        // Bu metodun dönüş tipi List<string> yani bir string listesi.
        public List<string> SehirListesiAl()
        {
            // Boş bir string listesi oluşturuluyor.
            List<string> sehirler = new List<string>();

            // Veritabanından şehir adlarını almak için kullanılacak olan SQL sorgusu.
            string sorgu = "SELECT sehir_adi FROM sehirler";

            // Veritabanı bağlantısı için MySqlCommand nesnesi oluşturuluyor ve sorgu ile ilişkilendiriliyor.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                try
                {
                    // Veritabanı bağlantısı açılıyor.
                    baglanti.Open();

                    // Veritabanından okuma yapmak için MySqlDataReader nesnesi oluşturuluyor.
                    using (MySqlDataReader okuyucu = komut.ExecuteReader())
                    {
                        // Okuyucu sona gelene kadar her bir kaydı okuyup işliyor.
                        while (okuyucu.Read())
                        {
                            // Her bir şehir adı, okuyucudan alınıp "sehirler" listesine ekleniyor.
                            sehirler.Add(okuyucu.GetString("sehir_adi"));
                        }
                    }
                }
                // Hata durumunda Exception sınıfından türetilen bir nesne yakalanıyor.
                catch (Exception ex)
                {
                    // Hata mesajı ekrana yazdırılıyor.
                    Console.WriteLine("Şehir listesi alınamadı: " + ex.Message);
                }
                // Her durumda veritabanı bağlantısı kapatılıyor.
                finally { baglanti.Close(); }
            }

            // Şehirler listesi doldurulduktan sonra geri döndürülüyor.
            return sehirler;
        }


        // Bu fonksiyon, veritabanından belirli bir şehir için hastane listesini alır ve bir string Listesi olarak döndürür.
        public List<string> HastaneListesiAl(int sehir_id)
        {
            // Yeni bir string Listesi oluşturuluyor. Bu liste hastane adlarını içerecek.
            List<string> hasteneler = new List<string>();

            // Veritabanından hastane adlarını almak için kullanılacak SQL sorgusu oluşturuluyor.
            string sorgu = "SELECT hastane_adi FROM hastaneler WHERE sehir_id = " + sehir_id;

            // MySqlCommand nesnesi oluşturuluyor ve bu nesne SQL sorgusu ve bağlantıyı alıyor.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                try
                {
                    // Bağlantı açılıyor.
                    baglanti.Open();

                    // SQL sorgusunu çalıştırmak için bir MySqlDataReader oluşturuluyor.
                    using (MySqlDataReader okuyucu = komut.ExecuteReader())
                    {
                        // Okuyucu, sonraki satıra geçilebilirse, yani bir sonraki hastane adı varsa döngü çalışmaya devam eder.
                        while (okuyucu.Read())
                        {
                            // Okuyucudan hastane adını alıp, hastaneler listesine ekliyoruz.
                            hasteneler.Add(okuyucu.GetString("hastane_adi"));
                        }
                    }
                }
                // Eğer herhangi bir hata oluşursa, bu kısmı çalıştırır ve konsola bir hata mesajı yazdırır.
                catch (Exception ex)
                {
                    Console.WriteLine("Hastane listesi alınamadı: " + ex.Message);
                }
                // Bağlantı kapatılır, bu kod her durumda çalışır.
                finally { baglanti.Close(); }
            }

            // Son olarak, hastaneler listesi döndürülür.
            return hasteneler;
        }


        public int KullaniciEkle(string adSoyad, string tc, string sifre, string tur, string sehir, string hastane)
        {
            int eklenenId = -1; // Eklenen kullanıcının ID'sini tutacak değişken, başlangıçta -1 olarak ayarlanmıştır.

            // Veritabanına ekleme sorgusunu oluşturuyoruz.
            string sorgu = "INSERT INTO kullanicilar (ad_soyad, tc, sifre, tur, sehir, hastane) VALUES (@adSoyad, @tc, @sifre, @tur, @sehir, @hastane); SELECT LAST_INSERT_ID();";

            // MySqlCommand kullanarak sorguyu çalıştırıyoruz.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Sorguya parametreler ekliyoruz.
                komut.Parameters.AddWithValue("@adSoyad", adSoyad);
                komut.Parameters.AddWithValue("@tc", tc);
                komut.Parameters.AddWithValue("@sifre", sifre);
                komut.Parameters.AddWithValue("@tur", tur);
                komut.Parameters.AddWithValue("@sehir", sehir);
                komut.Parameters.AddWithValue("@hastane", hastane);

                try
                {
                    // Veritabanı bağlantısını açıyoruz.
                    baglanti.Open();
                    // ExecuteScalar metodu ile sorguyu çalıştırıp eklenen kullanıcının ID'sini alıyoruz.
                    eklenenId = Convert.ToInt32(komut.ExecuteScalar());
                    // Kullanıcı başarıyla eklenmişse, ekrana bilgi mesajı yazdırıyoruz.
                    Console.WriteLine("Kullanıcı eklendi: " + adSoyad + ", ID: " + eklenenId);
                }
                catch (Exception ex)
                {
                    // Hata oluştuğunda hata mesajını yazdırıyoruz.
                    Console.WriteLine("Kullanıcı eklenemedi: " + ex.Message);
                }
                finally
                {
                    // Veritabanı bağlantısını kapatıyoruz.
                    baglanti.Close();
                }
            }

            // Eklenen kullanıcının ID'sini döndürüyoruz.
            return eklenenId;
        }


        public List<string> DoktorAdSoyadlariniGetir(int bolum,string hastane)
        {
            // Geri döndürülecek ad-soyad listesi için bir List<string> oluşturuyoruz.
            List<string> adSoyadListesi = new List<string>();

            // Veritabanından doktorların ad ve soyadlarını almak için kullanılacak sorguyu oluşturuyoruz.
            string sorgu = "SELECT k.ad_soyad " +
                           "FROM doktorlar d " +
                           "JOIN kullanicilar k ON d.doktor_id = k.id " +
                           "WHERE d.bolum = @bolum AND k.hastane = @hastane";

            // Veritabanına sorguyu gönderecek MySqlCommand nesnesini oluşturuyoruz.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Sorgunun içindeki @bolum parametresine bolum değişkeninin değerini atıyoruz.
                komut.Parameters.AddWithValue("@bolum", bolum);
                komut.Parameters.AddWithValue("@hastane", hastane);

                try
                {
                    // Veritabanı bağlantısını açıyoruz.
                    baglanti.Open();
                    // Sorguyu çalıştırıp verileri okuyacak MySqlDataReader nesnesini oluşturuyoruz.
                    using (MySqlDataReader okuyucu = komut.ExecuteReader())
                    {
                        // Veritabanından her bir kaydı okuyup işlemek için while döngüsü kullanıyoruz.
                        while (okuyucu.Read())
                        {
                            // Okunan her bir kaydın "ad_soyad" sütunundaki değerini adSoyadListesi'ne ekliyoruz.
                            adSoyadListesi.Add(okuyucu.GetString("ad_soyad"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Eğer bir hata olursa, bu hatayı konsola yazdırıyoruz.
                    Console.WriteLine("Ad soyadlar alınamadı: " + ex.Message);
                }
                finally
                {
                    // İşlemler tamamlandıktan sonra veritabanı bağlantısını kapatıyoruz.
                    baglanti.Close();
                }
            }

            // Doktorların ad ve soyadlarını içeren listeyi geri döndürüyoruz.
            return adSoyadListesi;
        }



        // Bu fonksiyon, bir kullanıcının kimlik numarası (tc) ve şifresini alır ve kullanıcıyı doğrular.
        public int KullaniciDogrula(string tc, string sifre)
        {
            // Eşleşen kullanıcının ID'sini tutacak değişken, başlangıçta -1 olarak ayarlanmıştır.
            int kullaniciId = -1;

            // SQL sorgusu, kullanicilar tablosundan belirtilen tc ve şifreye sahip kullanıcının ID'sini seçer.
            string sorgu = "SELECT id FROM kullanicilar WHERE tc = @tc AND sifre = @sifre";

            // MySqlCommand nesnesini kullanarak sorguyu veritabanına göndeririz.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Parametrelerimizi SQL sorgusuna ekleriz.
                komut.Parameters.AddWithValue("@tc", tc);
                komut.Parameters.AddWithValue("@sifre", sifre);

                try
                {
                    // Veritabanı bağlantısını açarız.
                    baglanti.Open();
                    // Sorguyu çalıştırırız ve sonucu alırız.
                    object sonuc = komut.ExecuteScalar();
                    // Eğer sonuç null değilse, kullanıcı doğrulanmıştır ve kullanıcının ID'sini alırız.
                    if (sonuc != null)
                    {
                        kullaniciId = Convert.ToInt32(sonuc);
                        // Kullanıcı doğrulandı mesajını ekrana yazdırırız.
                        Console.WriteLine("Kullanıcı doğrulandı, ID: " + kullaniciId);
                    }
                    // Eğer sonuç null ise, kullanıcı doğrulanamamıştır ve hata mesajını ekrana yazdırırız.
                    else
                    {
                        Console.WriteLine("Kullanıcı doğrulanamadı: TC veya şifre yanlış.");
                    }
                }
                // Herhangi bir hata oluşursa, hata mesajını ekrana yazdırırız.
                catch (Exception ex)
                {
                    Console.WriteLine("Kullanıcı doğrulama hatası: " + ex.Message);
                }
                // Her durumda, veritabanı bağlantısını kapatırız.
                finally
                {
                    baglanti.Close();
                }
            }

            // Fonksiyon, kullanıcının ID'sini döndürür.
            return kullaniciId;
        }


        public string KullaniciTurunuGetir(int id)
        {
            string kullaniciTur = ""; // Kullanıcının türünü tutacak değişken

            // Veritabanından kullanıcının türünü sorgulamak için SQL sorgusunu hazırlıyoruz.
            string sorgu = "SELECT tur FROM kullanicilar WHERE id = @id";

            // MySQL komutunu oluşturuyoruz ve bağlantıyı kullanarak sorguyu çalıştırmaya hazırlıyoruz.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Sorguya parametre ekleyerek güvenli bir sorgu oluşturuyoruz.
                komut.Parameters.AddWithValue("@id", id);

                try
                {
                    baglanti.Open(); // Veritabanı bağlantısını açıyoruz.
                    object sonuc = komut.ExecuteScalar(); // Sorguyu çalıştırarak bir sonuç alıyoruz.

                    // Eğer sorgudan bir sonuç döndüyse:
                    if (sonuc != null)
                    {
                        kullaniciTur = sonuc.ToString(); // Sonucu kullanıcı türü değişkenine atıyoruz.
                        Console.WriteLine("Kullanıcı türü alındı, Tür: " + kullaniciTur);
                    }
                    else
                    {
                        Console.WriteLine("Kullanıcı bulunamadı."); // Eğer kullanıcı bulunamadıysa hata mesajı gösteriyoruz.
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Kullanıcı türü alınamadı: " + ex.Message); // Hata oluşursa hata mesajını gösteriyoruz.
                }
                finally
                {
                    baglanti.Close(); // İşlem tamamlandığında veritabanı bağlantısını kapatıyoruz.
                }
            }

            return kullaniciTur; // Kullanıcı türünü döndürüyoruz.
        }



        // Bu metod, belirli bir MHRS doktorunun belirli bir tarihte uygun saatleri getirir ve bunları bir liste olarak döndürür.
        public List<string> MHRSSaatleriGetir(string mhrs_doktor, DateTime mhrs_tarih)
        {
            // Boş bir string listesi oluşturuyoruz. Bu liste, uygun saatleri saklayacak.
            List<string> saatler = new List<string>();

            // Veritabanında belirli doktorun belirli bir tarihte işlem yapılacak saatleri almak için kullanılacak sorguyu oluşturuyoruz.
            string sorgu = "SELECT DISTINCT mhrs_saat FROM mhrs WHERE mhrs_doktor = @doktor AND DATE(mhrs_tarih) = @tarih";

            // MySqlCommand nesnesini oluşturarak sorguyu çalıştırmak için kullanılır. "baglanti" burada bir veritabanı bağlantı nesnesi olmalıdır.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Sorguda kullanılacak parametreleri belirliyoruz. Bu, SQL injection saldırılarına karşı güvenli bir yaklaşımdır.
                komut.Parameters.AddWithValue("@doktor", mhrs_doktor);
                komut.Parameters.AddWithValue("@tarih", mhrs_tarih.ToString("yyyy-MM-dd"));

                try
                {
                    // Veritabanı bağlantısını açıyoruz.
                    baglanti.Open();

                    // Veritabanından verileri okumak için bir okuyucu oluşturuyoruz.
                    using (MySqlDataReader okuyucu = komut.ExecuteReader())
                    {
                        // Okuyucu veri okumaya devam ettiği sürece döngüyü çalıştırır.
                        while (okuyucu.Read())
                        {
                            // Veritabanından alınan saat bilgisini TimeSpan nesnesine dönüştürerek "saat" değişkenine atıyoruz.
                            TimeSpan saat = okuyucu.GetTimeSpan("mhrs_saat");

                            // Saati belirli bir formata dönüştürerek listeye ekliyoruz.
                            saatler.Add(saat.ToString(@"hh\:mm"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hata oluştuğunda kullanıcıya bir hata mesajı gösteriyoruz.
                    Console.WriteLine("Saatler alınamadı: " + ex.Message);
                }
                finally
                {
                    // İşlem bittiğinde, veritabanı bağlantısını kapatıyoruz.
                    baglanti.Close();
                }
            }

            // Uygun saatleri içeren listeyi döndürüyoruz.
            return saatler;
        }


        // Bu fonksiyon, verilen şehir ve hastane adına göre bugünden sonraki MHRS tarihlerini getirir.
        public DataTable BugundenSonrakiMHRSTarihleriniGetir(string sehir, string hastane)
        {
            // Bir DataTable nesnesi oluşturuyoruz.
            DataTable table = new DataTable();

            // Veritabanından sorgu yapmak için kullanacağımız SQL sorgusu.
            string sorgu = "SELECT mhrs.mhrs_id, mhrs.mhrs_tarih, mhrs.mhrs_saat, mhrs.mhrs_doktor, mhrs.mhrs_klinik, mhrs.mhrs_hasta_id, hasta.hasta_ad_soyad FROM mhrs INNER JOIN hasta ON mhrs.mhrs_hasta_id = hasta.hasta_id WHERE mhrs_tarih >= @bugun AND mhrs_sehir = @sehir AND mhrs_hastane = @hastane";

            // MySQL komutunu oluşturuyoruz ve SQL sorgusundaki parametreleri ekliyoruz.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                komut.Parameters.AddWithValue("@bugun", DateTime.Now.Date.ToString("yyyy-MM-dd")); // @bugun parametresi bugünün tarihini alır.
                komut.Parameters.AddWithValue("@sehir", sehir); // @sehir parametresi verilen şehri alır.
                komut.Parameters.AddWithValue("@hastane", hastane); // @hastane parametresi verilen hastane adını alır.

                try
                {
                    baglanti.Open(); // Veritabanı bağlantısını açıyoruz.
                    MySqlDataAdapter adapter = new MySqlDataAdapter(komut); // Veritabanı sorgusunu çalıştırmak için bir adaptör oluşturuyoruz.
                    adapter.Fill(table); // Adaptörle verileri DataTable nesnesine dolduruyoruz.
                }
                catch (Exception ex)
                {
                    // Hata durumunda kullanıcıya bir hata mesajı gösteriyoruz.
                    Console.WriteLine("Veriler alınamadı: " + ex.Message);
                }
                finally
                {
                    baglanti.Close(); // Veritabanı bağlantısını kapatıyoruz.
                }
            }

            return table; // Oluşturulan DataTable nesnesini geri döndürüyoruz.
        }


        public DataTable DoktorMHRSGetir(string sehir, string hastane, string doktor)
        {
            // Boş bir DataTable oluşturuyoruz.
            DataTable table = new DataTable();

            // Veritabanından veri çekmek için kullanacağımız SQL sorgusunu oluşturuyoruz.
            string sorgu = "SELECT mhrs.mhrs_id, mhrs.mhrs_tarih, mhrs.mhrs_saat, mhrs.mhrs_doktor, mhrs.mhrs_klinik, mhrs.mhrs_hasta_id, hasta.hasta_ad_soyad FROM mhrs INNER JOIN hasta ON mhrs.mhrs_hasta_id = hasta.hasta_id WHERE mhrs_tarih >= @bugun AND mhrs_sehir = @sehir AND mhrs_hastane = @hastane AND mhrs_doktor = @doktor";

            // MySqlCommand nesnesi oluşturuyoruz ve SQL sorgusunu ve bağlantıyı belirtiyoruz.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // SQL sorgusunda kullanacağımız parametreleri belirliyoruz.
                komut.Parameters.AddWithValue("@bugun", DateTime.Now.Date.ToString("yyyy-MM-dd"));
                komut.Parameters.AddWithValue("@sehir", sehir);
                komut.Parameters.AddWithValue("@hastane", hastane);
                komut.Parameters.AddWithValue("@doktor", doktor);

                try
                {
                    // Bağlantıyı açıyoruz.
                    baglanti.Open();
                    // SQL sorgusunu çalıştırarak verileri almak için MySqlDataAdapter nesnesi oluşturuyoruz.
                    MySqlDataAdapter adapter = new MySqlDataAdapter(komut);
                    // DataTable'a verileri dolduruyoruz.
                    adapter.Fill(table);
                }
                catch (Exception ex)
                {
                    // Eğer bir hata oluşursa, hatayı konsola yazdırıyoruz.
                    Console.WriteLine("Veriler alınamadı: " + ex.Message);
                }
                finally
                {
                    // Bağlantıyı her durumda kapatıyoruz.
                    baglanti.Close();
                }
            }

            // DataTable'ı döndürüyoruz.
            return table;
        }


        public string KullaniciAdSoyadGetir(int kullaniciId)
        {
            // Boş bir dize oluşturuyoruz, bu dize sonunda kullanıcı adı ve soyadını tutacak.
            string adSoyad = "";

            // SQL sorgusunu oluşturuyoruz. Bu sorgu, veritabanından belirli bir kullanıcının adını ve soyadını alacak.
            string sorgu = "SELECT ad_soyad FROM kullanicilar WHERE id = @kullaniciId";

            // MySqlCommand nesnesi oluşturuyoruz ve sorguyu ve bağlantıyı belirtiyoruz.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Sorguya kullanıcı ID'sini parametre olarak ekliyoruz.
                komut.Parameters.AddWithValue("@kullaniciId", kullaniciId);

                try
                {
                    // Veritabanı bağlantısını açıyoruz.
                    baglanti.Open();

                    // Sorguyu veritabanında çalıştırıyoruz ve tek bir değer döndüren ExecuteScalar metoduyla sonucu alıyoruz.
                    object sonuc = komut.ExecuteScalar();

                    // Eğer sonuç null değilse, yani bir kullanıcı bulunduysa:
                    if (sonuc != null)
                    {
                        // AdSoyad değişkenine sonucu string olarak atıyoruz.
                        adSoyad = sonuc.ToString();

                        // Kullanıcı adı soyadı alındı mesajını konsola yazdırıyoruz.
                        Console.WriteLine("Kullanıcı adı soyadı alındı: " + adSoyad);
                    }
                    else
                    {
                        // Eğer sonuç null ise, yani kullanıcı bulunamadıysa:
                        Console.WriteLine("Kullanıcı bulunamadı: ID geçersiz.");
                    }
                }
                catch (Exception ex)
                {
                    // Bir hata oluştuğunda, hatayı konsola yazdırıyoruz.
                    Console.WriteLine("Kullanıcı adı soyadı alınamadı: " + ex.Message);
                }
                finally
                {
                    // Veritabanı bağlantısını kapatıyoruz, böylece kaynakları serbest bırakıyoruz.
                    baglanti.Close();
                }
            }

            // Son olarak, aldığımız ad ve soyadı döndürüyoruz.
            return adSoyad;
        }



        // Bu metod, bir kullanıcının şehrini veritabanından getirir.
        public string KullaniciSehirGetir(int kullaniciId)
        {
            // İlk olarak, şehir bilgisini saklayacak bir değişken oluşturulur.
            string sehir = "";

            // Veritabanında kullanılacak sorgu stringi oluşturulur.
            string sorgu = "SELECT sehir FROM kullanicilar WHERE id = @kullaniciId";

            // MySqlCommand nesnesi oluşturulur ve bu nesneye sorgu stringi ve bağlantı bilgisi verilir.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Sorguya kullanıcı ID'si parametresi eklenir.
                komut.Parameters.AddWithValue("@kullaniciId", kullaniciId);

                try
                {
                    // Veritabanı bağlantısı açılır.
                    baglanti.Open();

                    // ExecuteScalar metodu kullanılarak sorgu çalıştırılır ve sonuç alınır.
                    object sonuc = komut.ExecuteScalar();

                    // Eğer sonuç null değilse (yani veritabanında bir şehir bulunduysa):
                    if (sonuc != null)
                    {
                        // Şehir değişkenine sonucun string değeri atanır.
                        sehir = sonuc.ToString();
                        // Kullanıcının şehri başarıyla alındığına dair bir mesaj yazdırılır.
                        Console.WriteLine("Kullanıcı şehri alındı: " + sehir);
                    }
                    // Eğer sonuç null ise (yani veritabanında kullanıcı bulunamadıysa):
                    else
                    {
                        // Kullanıcı bulunamadığına dair bir hata mesajı yazdırılır.
                        Console.WriteLine("Kullanıcı bulunamadı: ID geçersiz.");
                    }
                }
                // Eğer bir hata oluşursa:
                catch (Exception ex)
                {
                    // Oluşan hatanın mesajı ekrana yazdırılır.
                    Console.WriteLine("Kullanıcı şehri alınamadı: " + ex.Message);
                }
                // İşlem tamamlandıktan sonra veritabanı bağlantısı kapatılır.
                finally
                {
                    baglanti.Close();
                }
            }

            // Metodun geri dönüş değeri olarak şehir bilgisi verilir.
            return sehir;
        }



        public string KullaniciHastaneGetir(int kullaniciId)
        {
            // Boş bir string (hastane adı) oluşturuluyor.
            string hastane = "";

            // Veritabanından hastane adını almak için kullanılacak sorgu hazırlanıyor.
            string sorgu = "SELECT hastane FROM kullanicilar WHERE id = @kullaniciId";

            // MySqlCommand nesnesi oluşturuluyor ve sorgu ile bağlantı veritabanına gönderiliyor.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Sorguda kullanılacak parametre atanıyor.
                komut.Parameters.AddWithValue("@kullaniciId", kullaniciId);

                try
                {
                    // Veritabanı bağlantısı açılıyor.
                    baglanti.Open();

                    // Sorgudan tek bir değer döndürülmesi bekleniyor.
                    object sonuc = komut.ExecuteScalar();

                    // Eğer sorgudan bir sonuç döndüyse:
                    if (sonuc != null)
                    {
                        // Dönen sonucu string'e çevirerek "hastane" değişkenine atıyoruz.
                        hastane = sonuc.ToString();

                        // Kullanıcının hastanesini aldığımızı belirten bir mesaj yazdırılıyor.
                        Console.WriteLine("Kullanıcı hastanesi alındı: " + hastane);
                    }
                    else
                    {
                        // Eğer sorgudan herhangi bir sonuç dönmediyse, kullanıcının bulunamadığına dair bir mesaj yazdırılıyor.
                        Console.WriteLine("Kullanıcı bulunamadı: ID geçersiz.");
                    }
                }
                catch (Exception ex)
                {
                    // Herhangi bir hata durumunda hatayı yakalayarak ekrana yazdırıyoruz.
                    Console.WriteLine("Kullanıcı hastanesi alınamadı: " + ex.Message);
                }
                finally
                {
                    // Veritabanı bağlantısı kapatılıyor.
                    baglanti.Close();
                }
            }

            // Son olarak, kullanıcının hastane adını içeren string'i geri döndürüyoruz.
            return hastane;
        }



        // MHRSSil metodu, belirli bir MRHS (Merkezi Hekim Randevu Sistemi) kaydını veritabanından silmek için kullanılır.
        public void MHRSSil(string mhrs_id)
        {
            // SQL sorgusunu oluşturuyoruz: mhrs_id'si verilen kaydı sil.
            string sorgu = "DELETE FROM mhrs WHERE mhrs_id = @mhrs_id";

            // MySqlCommand nesnesini kullanarak sorguyu veritabanına göndermek için bağlantıyı kullanıyoruz.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Sorgunun parametre değerini belirleyerek SQL enjeksiyonunu önlemek için kullanıcı girdisini temizliyoruz.
                komut.Parameters.AddWithValue("@mhrs_id", mhrs_id);

                try
                {
                    // Veritabanı bağlantısını açıyoruz.
                    baglanti.Open();
                    // ExecuteNonQuery metoduyla sorguyu veritabanında çalıştırıyoruz ve etkilenen satır sayısını alıyoruz.
                    int etkilenenSatirSayisi = komut.ExecuteNonQuery();
                    // Eğer en az bir satır etkilendiyse, silme işleminin başarılı olduğunu kullanıcıya bildiriyoruz.
                    if (etkilenenSatirSayisi > 0)
                    {
                        Console.WriteLine("Kayıt başarıyla silindi.");
                    }
                    // Eğer hiç satır etkilenmediyse, silme işleminin başarısız olduğunu ve kaydın bulunamadığını kullanıcıya bildiriyoruz.
                    else
                    {
                        Console.WriteLine("Silme işlemi gerçekleştirilemedi: Kayıt bulunamadı.");
                    }
                }
                // Eğer herhangi bir hata oluşursa, hata mesajını kullanıcıya gösteriyoruz.
                catch (Exception ex)
                {
                    Console.WriteLine("Silme işlemi gerçekleştirilemedi: " + ex.Message);
                }
                // Her durumda, veritabanı bağlantısını kapatıyoruz.
                finally
                {
                    baglanti.Close();
                }
            }
        }




        // Bu metod, yeni bir doktorun veritabanına eklenmesini sağlar.
        // Parametreler:
        // - doktorId: Eklenecek doktorun ID'si.
        // - bolumId: Eklenecek doktorun hangi bölüme ait olduğunu belirten ID.
        public void DoktorEkle(int doktorId, int bolumId)
        {
            // Veritabanına ekleme sorgusunu oluşturuyoruz.
            string sorgu = "INSERT INTO doktorlar (doktor_id, bolum) VALUES (@doktorId, @bolumId)";

            // MySqlCommand sınıfı kullanılarak sorguyu ve bağlantıyı oluşturuyoruz.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Sorgudaki parametreleri belirliyoruz ve değerlerini atıyoruz.
                komut.Parameters.AddWithValue("@doktorId", doktorId);
                komut.Parameters.AddWithValue("@bolumId", bolumId);

                try
                {
                    // Veritabanı bağlantısını açıyoruz.
                    baglanti.Open();
                    // Komutu veritabanında çalıştırıyoruz (yani yeni doktoru ekliyoruz).
                    komut.ExecuteNonQuery();
                    // Başarılı bir şekilde doktor eklendiğini konsola yazdırıyoruz.
                    Console.WriteLine("Doktor eklendi: " + doktorId);
                }
                catch (Exception ex)
                {
                    // Hata oluştuğunda konsola hatayı yazdırıyoruz.
                    Console.WriteLine("Doktor eklenemedi: " + ex.Message);
                }
                finally
                {
                    // Bağlantıyı kapatıyoruz.
                    baglanti.Close();
                }
            }
        }


        public int HastaIdGetir(string hasta_tc)
        {
            int hastaId = -1; // Başlangıçta geçersiz bir hasta ID'si atayalım

            // Veritabanından hasta ID'sini almak için kullanılacak SQL sorgusu
            string sorgu = "SELECT hasta_id FROM hasta WHERE hasta_tc = @hasta_tc";

            // MySQL sorgusunu çalıştırmak için MySqlCommand nesnesini kullanıyoruz
            // "using" bloğu, komut nesnesinin kullanımı bittiğinde kaynakları serbest bırakır
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Sorgudaki parametreyi hasta_tc değeriyle değiştirir
                komut.Parameters.AddWithValue("@hasta_tc", hasta_tc);

                try
                {
                    // Veritabanı bağlantısını aç
                    baglanti.Open();

                    // Sorguyu veritabanında çalıştır ve ilk sütunun ilk satırını döndür
                    object sonuc = komut.ExecuteScalar();

                    // Eğer sonuç null değilse (yani bir hasta ID'si bulunmuşsa):
                    if (sonuc != null)
                    {
                        // Sonucu integer'a dönüştürerek hastaId'ye atar
                        hastaId = Convert.ToInt32(sonuc);
                        Console.WriteLine("Hasta ID alındı: " + hastaId);
                    }
                    else
                    {
                        // Hasta bulunamadıysa hata mesajını gösterir
                        Console.WriteLine("Hasta bulunamadı: TC geçersiz.");
                    }
                }
                catch (Exception ex)
                {
                    // Herhangi bir hata olursa hata mesajını gösterir
                    Console.WriteLine("Hasta ID alınamadı: " + ex.Message);
                }
                finally
                {
                    // Veritabanı bağlantısını her durumda kapatır
                    baglanti.Close();
                }
            }

            // Son olarak hastaId'yi döndürür
            return hastaId;
        }



        // Bu metod, bir hasta ekler ve eklenen hastanın ID'sini döndürür.
        public int HastaEkleVeIdDondur(string hasta_ad_soyad, string hasta_tc, string hasta_sifre)
        {
            // Başlangıçta geçersiz bir hasta ID'si atayalım
            int hastaId = -1;

            // SQL sorgumuzu oluşturalım; hasta tablosuna yeni bir kayıt eklerken INSERT INTO kullanıyoruz.
            // Ayrıca, eklediğimiz kaydın ID'sini almak için SELECT LAST_INSERT_ID() kullanıyoruz.
            string sorgu = "INSERT INTO hasta (hasta_ad_soyad, hasta_tc, hasta_sifre) VALUES (@hasta_ad_soyad, @hasta_tc, @hasta_sifre); SELECT LAST_INSERT_ID();";

            // MySqlCommand nesnesini kullanarak sorguyu veritabanına göndereceğiz
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // Sorguda kullanacağımız parametreleri belirliyoruz
                komut.Parameters.AddWithValue("@hasta_ad_soyad", hasta_ad_soyad);
                komut.Parameters.AddWithValue("@hasta_tc", hasta_tc);
                komut.Parameters.AddWithValue("@hasta_sifre", hasta_sifre);

                try
                {
                    // Veritabanı bağlantısını açıyoruz
                    baglanti.Open();

                    // Sorguyu çalıştırarak tek bir değer döndürmesini bekliyoruz
                    object sonuc = komut.ExecuteScalar();

                    // Eğer sorgudan dönen sonuç null değilse:
                    if (sonuc != null)
                    {
                        // Dönen sonucu bir integer'a dönüştürerek hastaId'ye atıyoruz
                        hastaId = Convert.ToInt32(sonuc);
                        // Kullanıcıya, hasta başarıyla eklendiğini ve eklenen hastanın ID'sini gösteren bir mesaj yazdırıyoruz
                        Console.WriteLine("Hasta başarıyla eklendi, ID: " + hastaId);
                    }
                    else
                    {
                        // Eğer sorgudan dönen sonuç null ise, ID alınamadığını belirten bir mesaj yazdırıyoruz
                        Console.WriteLine("Hasta eklenemedi: ID alınamadı.");
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda kullanıcıya hata mesajını gösteriyoruz
                    Console.WriteLine("Hasta eklenemedi: " + ex.Message);
                }
                finally
                {
                    // Veritabanı bağlantısını her durumda kapatıyoruz
                    baglanti.Close();
                }
            }

            // Eklenen hastanın ID'sini döndürüyoruz
            return hastaId;
        }



        // MHRSEkle adında bir fonksiyon tanımlanıyor.
        // Bu fonksiyon, veritabanına yeni bir MHRS (Merkezi Hekim Randevu Sistemi) randevusu ekler.
        // Parametreler:
        // - mhrs_hasta_id: Randevu atanacak hastanın kimliği.
        // - mhrs_sehir: Randevunun yapılacağı şehir.
        // - mhrs_tarih: Randevunun tarihi.
        // - mhrs_saat: Randevunun saati.
        // - mhrs_hastane: Randevunun yapılacağı hastane.
        // - mhrs_doktor: Randevunun atanacağı doktorun adı.
        // - mhrs_klinik: Randevunun yapıldığı klinik.
        public void MHRSEkle(int mhrs_hasta_id, string mhrs_sehir, string mhrs_tarih, string mhrs_saat, string mhrs_hastane, string mhrs_doktor, string mhrs_klinik)
        {
            // Veritabanına ekleme işlemi için SQL sorgusu oluşturuluyor.
            string sorgu = "INSERT INTO mhrs (mhrs_hasta_id, mhrs_sehir, mhrs_tarih, mhrs_saat, mhrs_hastane, mhrs_doktor, mhrs_klinik) " +
                           "VALUES (@mhrs_hasta_id, @mhrs_sehir, @mhrs_tarih, @mhrs_saat, @mhrs_hastane, @mhrs_doktor, @mhrs_klinik)";

            // MySqlCommand nesnesi oluşturuluyor ve SQL sorgusu ve bağlantı bilgisi ile initialize ediliyor.
            using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
            {
                // SQL sorgusunda kullanılan parametreler atanıyor.
                komut.Parameters.AddWithValue("@mhrs_hasta_id", mhrs_hasta_id);
                komut.Parameters.AddWithValue("@mhrs_sehir", mhrs_sehir);
                komut.Parameters.AddWithValue("@mhrs_tarih", mhrs_tarih);
                komut.Parameters.AddWithValue("@mhrs_saat", mhrs_saat);
                komut.Parameters.AddWithValue("@mhrs_hastane", mhrs_hastane);
                komut.Parameters.AddWithValue("@mhrs_doktor", mhrs_doktor);
                komut.Parameters.AddWithValue("@mhrs_klinik", mhrs_klinik);

                try
                {
                    // Veritabanı bağlantısı açılıyor.
                    baglanti.Open();
                    // Komut çalıştırılıyor ve veritabanına yeni randevu ekleniyor.
                    komut.ExecuteNonQuery();
                    // Ekleme işlemi başarılı olduğunda kullanıcıya bilgi mesajı gösteriliyor.
                    Console.WriteLine("Randevu başarıyla eklendi.");
                }
                catch (Exception ex)
                {
                    // Ekleme işlemi sırasında bir hata oluştuğunda hata mesajı gösteriliyor.
                    Console.WriteLine("Randevu eklenemedi: " + ex.Message);
                }
                finally
                {
                    // İşlem tamamlandığında veritabanı bağlantısı kapatılıyor.
                    baglanti.Close();
                }
            }
        }



    }
}

