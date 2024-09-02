using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace hastaneproj
{
    public partial class HastaneGiris : Form
    {
        AnaEkran anaEkran;
        public HastaneGiris()
        {
            InitializeComponent();
        }
        NpgsqlConnection bag = new NpgsqlConnection("server=localhost; port=5432; Database=HASTANE; user Id=postgres; password=3785");
        bool isThere;
        

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if(textBox1.Text=="Tc Kimlik Numarası")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Tc Kimlik Numarası";
                textBox1.ForeColor = Color.DimGray;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Şifre")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
                textBox2.PasswordChar= '*';
            }

        }
        char? none = null;

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Şifre";
                textBox2.ForeColor = Color.DimGray;
                textBox2.PasswordChar = Convert.ToChar(none);
            }

        }
            
        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            UyeOl uyeOl = new UyeOl();
            uyeOl.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            string tckimlik = textBox1.Text;
            string sifre = textBox2.Text;

            bag.Open();
            NpgsqlCommand command = new NpgsqlCommand("select * from hasta", bag);
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (tckimlik == reader["h_tc"].ToString() && sifre == reader["h_sifre"].ToString())
                {
                    isThere = true;
                    break;
                }
                else
                {
                    isThere = false;
                }
            }
            bag.Close();
            if (isThere)
            {
                MessageBox.Show("Başarıyla Giriş Yaptınız!", "Program");
                AnaEkran anaekran = new AnaEkran();
                anaekran.tcno = tckimlik;
                anaekran.Show();
                this.Hide();
                
            }
            else
            {
                MessageBox.Show("Tc Kimlik Veya Şifre Hatalı");
            }


        }
    }
}
