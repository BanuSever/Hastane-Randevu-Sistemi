using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace hastaneproj
{
    public partial class UyeOl : Form
    {
        
        public UyeOl()
        {
            InitializeComponent();
        }

      

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

       
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    bag.Open();
        //    NpgsqlCommand sqlCommand = new NpgsqlCommand("Insert into hasta(h_tc,h_Adi,h_soyadi,h_cinsiyet,h_telefon,h_mail,h_kangrubu) values('" + textBox3.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + textBox6.Text + "','" + textBox7.Text + "','" + textBox8.Text + "','" + textBox5.Text + "',)");
        //    sqlCommand.ExecuteNonQuery();
        //    bag.Close();
        //    MessageBox.Show("Başarıyla Kayıt Oldunuz.");
        //}
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    string metin = textBox6.Text.ToUpper();
        //    string ad = textBox1.Text.ToUpper();
        //    string soyad = textBox2.Text.ToUpper();
        //    string kan = textBox5.Text.ToUpper();

        //    using (NpgsqlConnection bag = new NpgsqlConnection("server=localHost; port=5432; Database=HastaneProje; user Id=postgres; password=123"))
        //    {
        //        bag.Open();
        //        NpgsqlCommand sqlCommand = new NpgsqlCommand("INSERT INTO hasta (h_tc, h_Adi, h_soyadi, h_cinsiyet, h_telefon, h_mail, h_kangrubu,h_sifre) VALUES (@tc, @adi, @soyadi, @cinsiyet, @telefon, @mail, @kangrubu, @sifre)", bag);
        //        sqlCommand.Parameters.AddWithValue("@tc", textBox3.Text);
        //        sqlCommand.Parameters.AddWithValue("@adi", ad);
        //        sqlCommand.Parameters.AddWithValue("@soyadi", soyad);
        //        sqlCommand.Parameters.AddWithValue("@cinsiyet", metin);
        //        sqlCommand.Parameters.AddWithValue("@telefon", textBox7.Text);
        //        sqlCommand.Parameters.AddWithValue("@mail", textBox8.Text);
        //        sqlCommand.Parameters.AddWithValue("@kangrubu", textBox5.Text);
        //        sqlCommand.Parameters.AddWithValue("@sifre", textBox4.Text);
        //        sqlCommand.ExecuteNonQuery();
        //        MessageBox.Show("Başarıyla Kayıt Oldunuz.");
        //    }
        //    Application.Exit();
        //}
        private bool IsDuplicate(string tcKimlik, string telefon)
        {
            string query = "SELECT COUNT(*) FROM hasta WHERE h_tc = @tcKimlik OR h_telefon = @telefon";
            using (NpgsqlConnection connection = new NpgsqlConnection("server=localhost; port=5432; Database=HASTANE; user Id=postgres; password=3785"))
            {
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tcKimlik", tcKimlik);
                    command.Parameters.AddWithValue("@telefon", telefon);
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }
            string metin = comboBox1.Text;
            string ad = textBox1.Text.ToUpper();
            string soyad = textBox2.Text.ToUpper();
            string kan = comboBox2.Text.ToUpper();
            string dogumTarihi=dateTimePicker1.Value.ToShortDateString();
            if (IsDuplicate(textBox3.Text, textBox7.Text))
            {
                MessageBox.Show("Bu TC Kimlik numarası veya telefon numarası zaten kayıtlı.");
                return;     
            }

            using (NpgsqlConnection bag = new NpgsqlConnection("server=localhost; port=5432; Database=HASTANE; user Id=postgres; password=3785"))
            {
                bag.Open();
                string query = "INSERT INTO hasta (h_tc, h_Adi, h_soyadi, h_cinsiyet,h_dogumtarihi, h_telefon, h_mail, h_kangrubu, h_sifre) " +
                               "VALUES ('" + textBox3.Text + "', '" + ad + "', '" + soyad + "', '" + metin + "','"+dogumTarihi+"', '" + textBox7.Text + "', '" + textBox8.Text + "', '" + kan + "', '" + textBox4.Text + "')";
                NpgsqlCommand sqlCommand = new NpgsqlCommand(query, bag);
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Başarıyla Kayıt Oldunuz.");
            }
            this.Hide();
            HastaneGiris Giris = new HastaneGiris();
            Giris.Show();
            this.Close();
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text.Length < 11)
            {
                MessageBox.Show("TC Kimlik Numarası 11 rakamdan az olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                
                textBox3.Focus();
            }
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            string metin = textBox8.Text;
            if (!metin.Contains("@"))
            {
                
                MessageBox.Show("Lütfen geçerli bir e-posta adresi girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

               
                textBox8.Focus();
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
