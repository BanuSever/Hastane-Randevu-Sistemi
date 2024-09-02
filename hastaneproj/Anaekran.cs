using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;
using Npgsql;
using NpgsqlTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace hastaneproj
{
    public partial class AnaEkran : Form
    {
        private int hastaid;

        public AnaEkran()
        {
            InitializeComponent();
        }
        public string tcno;
        NpgsqlConnection bag = new NpgsqlConnection("server=localhost; port=5432; Database=HASTANE; user Id=postgres; password=3785");

             

        private void AnaEkran_Load(object sender, EventArgs e)
        {
            bag.Open();
            label8.Text = tcno;
            hastaid = 0;
            NpgsqlCommand komut5 = new NpgsqlCommand("select h_id from hasta where h_tc=@p1", bag);
            komut5.Parameters.AddWithValue("@p1", label8.Text);
            NpgsqlDataReader dr9 = komut5.ExecuteReader();
            while (dr9.Read())
            {
                hastaid = Convert.ToInt32(dr9[0]);
            }
            dr9.Close();


            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("select d_adi,p_adi,r_gun,r_saat from doktor,poliklinik,randevu " +
                "where d_id=r_doktorid and p_id=r_polid and r_hastaid=12", bag);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            da.Fill(dt);
            randevudata.DataSource = dt;
            randevudata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;



            NpgsqlCommand komut = new NpgsqlCommand("select h_adi,h_soyadi,h_telefon from hasta where h_tc=@p1",bag);
            komut.Parameters.AddWithValue("@p1", label8.Text);
            NpgsqlDataReader dr=komut.ExecuteReader();
            while(dr.Read())
            {
                label5.Text = (string)dr[0];
                label6.Text = (string)dr[1];
                label7.Text = (string)dr[2];

            }
            dr.Close();
            NpgsqlCommand komut2 = new NpgsqlCommand("Select p_adi from poliklinik", bag);
            NpgsqlDataReader dr2 =komut2.ExecuteReader();
            while( dr2.Read())
            {
                comboBox1.Items.Add(dr[0]);
            }
            dr2.Close();
            RefreshDataGridView();
            bag.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Text = string.Empty;
            //comboBox3.Items.Clear();
            comboBox3.Text = string.Empty;
            //comboBox4.Items.Clear();
            comboBox4.Text = string.Empty;

            bag.Open();
            NpgsqlCommand komut3 = new NpgsqlCommand("Select d_adi,d_soyadi from doktor,poliklinik where p_id=d_polid and p_adi=@p1", bag);
            komut3.Parameters.AddWithValue("@p1", comboBox1.Text);
            NpgsqlDataReader dr3=komut3.ExecuteReader();
            while (dr3.Read())
            {
                comboBox2.Items.Add(dr3[0] + " " + dr3[1]);
                
            }
            dr3.Close();
            bag.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bilgidüzenle bilgidüzenle = new bilgidüzenle();
            bilgidüzenle.tcnum = tcno;
            bilgidüzenle.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            HastaneGiris Giris = new HastaneGiris();
            Giris.Show();
            this.Close();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(comboBox2.Text) || string.IsNullOrEmpty(comboBox3.Text) || string.IsNullOrEmpty(comboBox4.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string doktorAdSoyad = comboBox2.SelectedItem.ToString();
            string[] adSoyad = doktorAdSoyad.Split(' ');
            string doktorAdi = adSoyad[0];
            string doktorSoyadi = adSoyad[1];
            int hastaid = 0;
            int doktorid = 0;
            int polid = 0;

            string polAdi = comboBox1.SelectedItem.ToString();
            string hastaAdi = label5.Text;
            string randevuGun = comboBox3.Text;
            string randevuSaat = comboBox4.Text;

            using (NpgsqlConnection bag = new NpgsqlConnection("server=localhost; port=5432; Database=HASTANE; user Id=postgres; password=3785"))
            {
                bag.Open();

                NpgsqlCommand komut = new NpgsqlCommand("select h_id from hasta where h_tc=@p1", bag);
                komut.Parameters.AddWithValue("@p1", label8.Text);
                NpgsqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    hastaid = Convert.ToInt32(dr[0]);
                }
                dr.Close();

                NpgsqlCommand komut2 = new NpgsqlCommand("select d_id from doktor where d_adi like @p2 and d_soyadi like @p3", bag);
                komut2.Parameters.AddWithValue("@p2", "%" + doktorAdi + "%");
                komut2.Parameters.AddWithValue("@p3", "%" + doktorSoyadi + "%");
                NpgsqlDataReader dr2 = komut2.ExecuteReader();
                while (dr2.Read())
                {
                    doktorid = Convert.ToInt32(dr2[0]);
                }
                dr2.Close();

                NpgsqlCommand komut3 = new NpgsqlCommand("select p_id from poliklinik where p_adi like @p4", bag);
                komut3.Parameters.AddWithValue("@p4", "%" + polAdi + "%");
                NpgsqlDataReader dr3 = komut3.ExecuteReader();
                 while (dr3.Read())
                {
                    polid = Convert.ToInt32(dr3[0]);
                }
                dr3.Close();

                NpgsqlCommand kontrolKomutu = new NpgsqlCommand("select count(*) from randevu where r_doktorid=@doktorid and r_gun=@randevuGun and r_saat=@randevuSaat", bag);
                kontrolKomutu.Parameters.AddWithValue("@doktorid", doktorid);
                kontrolKomutu.Parameters.AddWithValue("@randevuGun", randevuGun);
                kontrolKomutu.Parameters.Add("@randevuSaat", NpgsqlDbType.Time).Value = TimeSpan.ParseExact(randevuSaat, "h\\:mm", CultureInfo.InvariantCulture);
                int randevuSayisi = Convert.ToInt32(kontrolKomutu.ExecuteScalar());
                if (randevuSayisi > 0)
                {
                    MessageBox.Show("Bu saatte doktorunuzun başka bir randevusu bulunmaktadır. Lütfen başka bir saat seçin.");
                    return;
                }

                string query = "insert into randevu(r_hastaid,r_doktorid,r_polid,r_gun,r_saat)" + "values('" + hastaid + "', '" + doktorid + "','" + polid + "','" + comboBox3.Text + "','" + comboBox4.Text + "')";
                NpgsqlCommand sqlCommand = new NpgsqlCommand(query, bag);
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Başarıyla Randevu Alınmıştır");    
                RefreshDataGridView();
                
                comboBox1.Text =string.Empty;
                
                comboBox2.Text =string.Empty;
                comboBox3.Text=string.Empty;
                comboBox4.Text=string.Empty;
                bag.Close();


            }
            



        }

        private void randevudata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && randevudata.Columns[e.ColumnIndex].Name == "sil")
                {
                    if (MessageBox.Show("Emin misiniz?", "Mesaj", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bag.Open();
                        int row = e.RowIndex;
                        string doktorAdi = randevudata.Rows[row].Cells[1].Value.ToString();
                        string gun = randevudata.Rows[row].Cells[3].Value.ToString();
                        string saatstr = randevudata.Rows[row].Cells[4].Value.ToString();

                        TimeSpan saat = TimeSpan.Parse(saatstr);
                        NpgsqlCommand komut3 = new NpgsqlCommand("DELETE FROM randevu WHERE r_saat=@p1 AND r_gun=@p2 AND r_doktorid IN (SELECT d_id FROM doktor WHERE d_adi=@p3)", bag);
                        komut3.Parameters.AddWithValue("@p1", saat);
                        komut3.Parameters.AddWithValue("@p2", gun);
                        komut3.Parameters.AddWithValue("@p3", doktorAdi);

                        int affectedRows = komut3.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Randevu başarıyla silindi.");
                            RefreshDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Randevu silinemedi.");
                        }

                        bag.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }



        private void RefreshDataGridView()
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("select d_adi, p_adi, r_gun, r_saat from doktor, poliklinik, randevu " +
                "where d_id=r_doktorid and p_id=r_polid and r_hastaid=@p1", bag);
            cmd.Parameters.AddWithValue("@p1", hastaid);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            da.Fill(dt);

            randevudata.DataSource = dt;

            
            randevudata.Columns[1].HeaderText = "Doktor Adı";
            randevudata.Columns[2].HeaderText = "Poliklinik Adı";
            randevudata.Columns[3].HeaderText = "Randevu Günü";
            randevudata.Columns[4].HeaderText = "Randevu Saati";

        }
    }
}

