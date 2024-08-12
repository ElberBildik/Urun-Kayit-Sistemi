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


namespace UrunKayit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadData();
            txtAdet.ReadOnly = true;
            label10.Visible = false;
            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
            
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            // Retrieve values from text boxes
            string urunKodu = txtUrunKod.Text;
            string stokMiktarıText = txtStokMik.Text;
            string kayitYapan = txtKayitYapan.Text;
            string stokTarihi = txtSokTarihi.Text;
            string enText = txtEn.Text;
            string boyText = txtBoy.Text;
            string yogunlukText = txtYogunluk.Text;
            string masuraText = txtMasura.Text;
            string adetKgText = txtAdet.Text;

            // Check if any required field is empty
            if (string.IsNullOrWhiteSpace(urunKodu) ||
                string.IsNullOrWhiteSpace(stokMiktarıText) ||
                string.IsNullOrWhiteSpace(kayitYapan) ||
                string.IsNullOrWhiteSpace(stokTarihi) ||
                string.IsNullOrWhiteSpace(enText) ||
                string.IsNullOrWhiteSpace(boyText) ||
                string.IsNullOrWhiteSpace(yogunlukText) ||
                string.IsNullOrWhiteSpace(masuraText) ||
                string.IsNullOrWhiteSpace(adetKgText))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return; // Exit the method to prevent further execution
            }

            // Convert values to appropriate types
            int stokMiktarı = Convert.ToInt32(stokMiktarıText);
            decimal en = Convert.ToDecimal(enText);
            decimal boy = Convert.ToDecimal(boyText);
            decimal yogunluk = Convert.ToDecimal(yogunlukText);
            decimal masura = Convert.ToDecimal(masuraText);
            decimal adetKg = Convert.ToDecimal(adetKgText);

            string connectionString = "Server=ELBER\\SQLEXPRESS;Initial Catalog=StokKayit;Integrated Security=True;";
            string query = "INSERT INTO dbo.UrunTbl (UrunKodu, StokMiktari, StokTarihi, KayitYapan, En, Boy, Yogunluk, Masura, AdetKG) VALUES (@UrunKodu, @StokMiktari, @StokTarihi, @KayitYapan, @En, @Boy, @Yogunluk, @Masura, @AdetKg)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UrunKodu", urunKodu);
                command.Parameters.AddWithValue("@StokMiktari", stokMiktarı);
                command.Parameters.AddWithValue("@StokTarihi", stokTarihi);
                command.Parameters.AddWithValue("@KayitYapan", kayitYapan);
                command.Parameters.AddWithValue("@En", en);
                command.Parameters.AddWithValue("@Boy", boy);
                command.Parameters.AddWithValue("@Yogunluk", yogunluk);
                command.Parameters.AddWithValue("@Masura", masura);
                command.Parameters.AddWithValue("@AdetKg", adetKg);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected > 0 ? "Ürün başarıyla eklendi." : "Ürün eklenemedi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}");
                }
            }

            // Update DataGridView (optional)
            LoadData();
        }

        private void LoadData()
        {
            string connectionString = "Server=ELBER\\SQLEXPRESS;Initial Catalog=StokKayit;Integrated Security=True;";
            string query = "SELECT * FROM dbo.UrunTbl";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is valid and not a header cell
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Populate the text boxes with values from the selected row
                txtUrunKod.Text = selectedRow.Cells["UrunKodu"].Value.ToString();
                txtStokMik.Text = selectedRow.Cells["StokMiktari"].Value.ToString();
                txtKayitYapan.Text = selectedRow.Cells["KayitYapan"].Value.ToString();
                txtSokTarihi.Text = selectedRow.Cells["StokTarihi"].Value.ToString();
                txtEn.Text = selectedRow.Cells["En"].Value.ToString();
                txtBoy.Text = selectedRow.Cells["Boy"].Value.ToString();
                txtYogunluk.Text = selectedRow.Cells["Yogunluk"].Value.ToString();
                txtMasura.Text = selectedRow.Cells["Masura"].Value.ToString();
                txtAdet.Text = selectedRow.Cells["AdetKG"].Value.ToString();
                label10.Text = selectedRow.Cells["Id"].Value.ToString();
            }
        }


        private void btnHesapla_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve values from text boxes
                decimal en = Convert.ToDecimal(txtEn.Text);
                decimal boy = Convert.ToDecimal(txtBoy.Text);
                decimal yogunluk = Convert.ToDecimal(txtYogunluk.Text);
                decimal masura = Convert.ToDecimal(txtMasura.Text);
                int stokMiktarı = Convert.ToInt32(txtStokMik.Text);

                // Calculate adet kg using the provided formula
                decimal adetKg = ((en * stokMiktarı * boy * yogunluk) / 100) + masura;

                // Update the TextBox with the calculated value
                txtAdet.Text = adetKg.ToString();
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Hatalı format: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            // Get the ID of the selected record from label10
            string id = label10.Text;

            // Validate that label10 has a value and all text boxes are filled
            if (string.IsNullOrWhiteSpace(id) ||
                string.IsNullOrWhiteSpace(txtUrunKod.Text) ||
                string.IsNullOrWhiteSpace(txtStokMik.Text) ||
                string.IsNullOrWhiteSpace(txtKayitYapan.Text) ||
                string.IsNullOrWhiteSpace(txtSokTarihi.Text) ||
                string.IsNullOrWhiteSpace(txtEn.Text) ||
                string.IsNullOrWhiteSpace(txtBoy.Text) ||
                string.IsNullOrWhiteSpace(txtYogunluk.Text) ||
                string.IsNullOrWhiteSpace(txtMasura.Text) ||
                string.IsNullOrWhiteSpace(txtAdet.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return; // Exit the method to prevent further execution
            }

            // Retrieve values from text boxes
            string urunKodu = txtUrunKod.Text;
            int stokMiktarı = Convert.ToInt32(txtStokMik.Text);
            string kayitYapan = txtKayitYapan.Text;
            string stokTarihi = txtSokTarihi.Text;
            decimal en = Convert.ToDecimal(txtEn.Text);
            decimal boy = Convert.ToDecimal(txtBoy.Text);
            decimal yogunluk = Convert.ToDecimal(txtYogunluk.Text);
            decimal masura = Convert.ToDecimal(txtMasura.Text);
            decimal adetKg = Convert.ToDecimal(txtAdet.Text);

            // Connection string for your database
            string connectionString = "Server=ELBER\\SQLEXPRESS;Initial Catalog=StokKayit;Integrated Security=True;";

            // SQL UPDATE statement
            string query = "UPDATE dbo.UrunTbl SET UrunKodu = @UrunKodu, StokMiktari = @StokMiktari, StokTarihi = @StokTarihi, KayitYapan = @KayitYapan, En = @En, Boy = @Boy, Yogunluk = @Yogunluk, Masura = @Masura, AdetKG = @AdetKg WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UrunKodu", urunKodu);
                command.Parameters.AddWithValue("@StokMiktari", stokMiktarı);
                command.Parameters.AddWithValue("@StokTarihi", stokTarihi);
                command.Parameters.AddWithValue("@KayitYapan", kayitYapan);
                command.Parameters.AddWithValue("@En", en);
                command.Parameters.AddWithValue("@Boy", boy);
                command.Parameters.AddWithValue("@Yogunluk", yogunluk);
                command.Parameters.AddWithValue("@Masura", masura);
                command.Parameters.AddWithValue("@AdetKg", adetKg);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected > 0 ? "Ürün başarıyla güncellendi." : "Ürün güncellenemedi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}");
                }
            }

            // Update DataGridView (optional)
            LoadData();
        }


        private void btnSil_Click(object sender, EventArgs e)
        {
            // Get the ID of the record to delete from label10
            string id = label10.Text; // Ensure label10 contains the correct ID

            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Silinecek kaydın ID'si eksik veya geçersiz.");
                return; // Exit the method to prevent further execution
            }

            // Connection string for your database
            string connectionString = "Server=ELBER\\SQLEXPRESS;Initial Catalog=StokKayit;Integrated Security=True;";

            // SQL DELETE statement
            string query = "DELETE FROM dbo.UrunTbl WHERE Id = @Id"; // Replace `Id` with your actual column name

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id); // Replace with your actual primary key

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected > 0 ? "Ürün başarıyla silindi." : "Silinecek ürün bulunamadı.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}");
                }
            }

            // Update DataGridView (optional)
            LoadData();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            // Optionally, confirm with the user before closing
            var result = MessageBox.Show("Uygulamayı kapatmak istediğinizden emin misiniz?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                           // Close the current form
                this.Close();
            }
        }

    }
}

