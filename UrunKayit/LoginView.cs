using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace UrunKayit
{
    public partial class LoginView : Form
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            string kullanıcıAdı = txtKullaniciAdi.Text;
            string sifre = txtSifre.Text;


            // Veritabanı bağlantısı
            string connectionString = "Data Source=ELBER\\SQLEXPRESS;Initial Catalog=StokKayit;Integrated Security=True";

            string query = "SELECT ID FROM Kullanicilar WHERE Kadi = @Kadi AND Sifre = @Sifre";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Kadi", kullanıcıAdı);
                command.Parameters.AddWithValue("@Sifre",sifre);

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    int kullanıcıID = Convert.ToInt32(result);
                    MessageBox.Show($"Giriş başarılı! Kullanıcı ID: {kullanıcıID}");
                    // Kullanıcıyı ana ekrana yönlendirebilirsiniz ve kullanıcıID'yi geçirebilirsiniz.
                    Form1 ekran = new Form1();
                    ekran.Show();
                    
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı..Kayıt Değilseniz Kayıt Olun!");
                }
            }
        }

        private void btnKayitOl_Click(object sender, EventArgs e)
        {
            string kullanıcıAdı = txtKullaniciAdi.Text;
            string şifre = txtSifre.Text;


            // Veritabanı bağlantısı
            string connectionString = "Server=ELBER\\SQLEXPRESS;Initial Catalog=StokKayit;Integrated Security=True;";
            string query = "INSERT INTO dbo.Kullanicilar (Kadi, Sifre) VALUES (@KullanıcıAdı, @Şifre)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                // Parametreleri ekleyin
                command.Parameters.Add(new SqlParameter("@KullanıcıAdı", SqlDbType.NVarChar)).Value = kullanıcıAdı;
                command.Parameters.Add(new SqlParameter("@Şifre", SqlDbType.NVarChar)).Value = şifre;

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Kullanıcı başarıyla kaydedildi.");
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı kaydı başarısız.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}");
                }
            }
        }

        private void LoginView_Load(object sender, EventArgs e)
        {

        }
    }
}
