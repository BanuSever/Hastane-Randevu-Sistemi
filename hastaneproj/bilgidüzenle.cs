using Npgsql;
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

namespace hastaneproj
{
    public partial class bilgidüzenle : Form
    {
        public bilgidüzenle()
        {
            InitializeComponent();
        }
        NpgsqlConnection bag = new NpgsqlConnection("server=localhost; port=5432; Database=HASTANE; user Id=postgres; password=3785");
        public string tcnum;
        private void bilgidüzenle_Load(object sender, EventArgs e)
        {
            bag.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("select * from hasta where h_tc=@p1", bag);
            cmd.Parameters.AddWithValue("@p1", tcnum);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                textBox1.Text = dr[2].ToString();
                textBox2.Text= dr[3].ToString();
                comboBox1.Text= dr[4].ToString();
                textBox7.Text= dr[6].ToString();
                textBox8.Text= dr[7].ToString();
                comboBox2.Text= dr[8].ToString();
                textBox4.Text= dr[9].ToString();


            }
            bag.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bag.Open();
            NpgsqlCommand cmd2 = new NpgsqlCommand("update hasta set h_adi=@p2,h_soyadi=@p3,h_cinsiyet=@p4,h_telefon=@p5,h_mail=@p6,h_kangrubu=@p7,h_sifre=@p8 where h_tc=@p1",bag);
            cmd2.Parameters.AddWithValue("@p1", tcnum);
            cmd2.Parameters.AddWithValue("@p2", textBox1.Text);
            cmd2.Parameters.AddWithValue("@p3",textBox2.Text);
            cmd2.Parameters.AddWithValue("@p4",comboBox1.Text);
            cmd2.Parameters.AddWithValue("@p5",textBox7.Text);
            cmd2.Parameters.AddWithValue("@p6",textBox8.Text);
            cmd2.Parameters.AddWithValue("@p7",comboBox2.Text);
            cmd2.Parameters.AddWithValue("@p8",textBox4.Text);
            cmd2.ExecuteNonQuery();
            bag.Close();
            MessageBox.Show("Bilgileriniz Güncellendi");
            this.Close();
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
    }
}
