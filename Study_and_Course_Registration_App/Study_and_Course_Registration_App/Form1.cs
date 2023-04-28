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
using System.IO;

namespace Study_and_Course_Registration_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-I9TTNVF;Initial Catalog=DbStudyandCourseReg;Integrated Security=True");

        void dersListesi()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TBLDERSLER", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbDers.ValueMember = "DERSID";
            cmbDers.DisplayMember = "DERSAD";
            cmbDers.DataSource = dt;
        }

        void EtutListesi ()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("EXECUTE ETUT", baglanti);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView1.DataSource = dt3;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dersListesi();
            EtutListesi();
        }

        private void cmbDers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM TBLOGRETMEN WHERE BRANSID="+cmbDers.SelectedValue, baglanti);

            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            cmbOgretmen.ValueMember = "OGRTID";
            cmbOgretmen.DisplayMember = "AD";
            cmbOgretmen.DataSource = dt2;
        }

        private void btnOlustur_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO TBLETUT (DERSID, OGRETMENID, TARIH, SAAT) VALUES (@P1, @P2, @P3, @P4)",baglanti);
            komut.Parameters.AddWithValue("@P1", cmbDers.SelectedValue);
            komut.Parameters.AddWithValue("@P2", cmbOgretmen.SelectedValue);
            komut.Parameters.AddWithValue("@P3", mskTarih.Text);
            komut.Parameters.AddWithValue("@P4", mskSaat.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Etüt Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            txtEtutId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
        }

        private void btbEtutVer_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("UPDATE TBLETUT SET OGRENCIID=@P1, DURUM=@P2 WHERE ID=@P3", baglanti);
            komut.Parameters.AddWithValue("@P1", txtOgrenciId.Text);
            komut.Parameters.AddWithValue("@P2", "True");
            komut.Parameters.AddWithValue("@P3", txtEtutId.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci ile etüt gerçekleştirildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void btnFotografYukle_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
        }

        private void btnOgrenciEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO TBLOGRECI (AD, SOYAD, FOTOGRAF, SINIF, TELEFON, MAIL) VALUES (@P1, @P2, @P3, @P4, @P5, @P6)",baglanti);
            komut.Parameters.AddWithValue("@P1", txtOgrenciAd.Text);
            komut.Parameters.AddWithValue("@P2", txtOgrenciSoyad.Text);
            komut.Parameters.AddWithValue("@P3", pictureBox1.ImageLocation);
            komut.Parameters.AddWithValue("@P4", txtOgrenciSinif.Text);
            komut.Parameters.AddWithValue("@P5", mskOgrenciTel.Text);
            komut.Parameters.AddWithValue("@P6", txtOgrenciMail.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci Sisteme Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
