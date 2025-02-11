using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace kayıtOl_giris
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size = 4096; user id = MuhammetTRZ_SQLLogin_1; pwd=9hbeo7oosu;data source = Muhammet.mssql.somee.com; persist security info=False;initial catalog = Muhammet; TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);

        private void button1_Click(object sender, EventArgs e)
        {
            //girisYap girisForm=new girisYap();
            //girisForm.Show();
            //this.Hide();
        }
            
        private void button2_Click(object sender, EventArgs e)
        {
            //kayıtcs kayıtForm=new kayıtcs();
            //kayıtForm.Show();
            //this.Hide();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            girisYap girisForm = new girisYap();
            girisForm.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            kayıtcs kayıtForm = new kayıtcs();
            kayıtForm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button7.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || textBox1.Text == "Aramanızı giriniz")
            {
                MessageBox.Show("Aramak istediğiniz bilgiyi yazınız.");
                return;
            }

            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();

                // Kitap var mı kontrolü
                string kitapVarmiSql = @"
            SELECT COUNT(*) FROM kitaplar 
            WHERE 
                eser_adı LIKE @search OR
                yazar LIKE @search OR
                yayın_tarihi LIKE @search OR
                yayınlayan LIKE @search OR
                dil LIKE @search OR	
                CAST(isbn AS NVARCHAR) LIKE @search OR
                yayın_gelis_tarihi LIKE @search";

                using (SqlCommand kitapVarmiKomut = new SqlCommand(kitapVarmiSql, connect))
                {
                    kitapVarmiKomut.Parameters.AddWithValue("@search", "%" + textBox1.Text + "%");
                    int kitapSayisi = (int)kitapVarmiKomut.ExecuteScalar();

                    if (kitapSayisi == 0)
                    {
                        MessageBox.Show("Aradığınız kitap sistemde bulunmamaktadır.", "Sonuç yok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Kitap var ama adet 0 mı?
                string adetSifirMiSql = @"
            SELECT eser_adı, yazar FROM kitaplar 
            WHERE 
                (
                    eser_adı LIKE @search OR
                    yazar LIKE @search OR
                    yayın_tarihi LIKE @search OR
                    yayınlayan LIKE @search OR
                    dil LIKE @search OR	
                    CAST(isbn AS NVARCHAR) LIKE @search OR
                    yayın_gelis_tarihi LIKE @search
                )
                AND adet = 0";

                using (SqlCommand adetSifirKomut = new SqlCommand(adetSifirMiSql, connect))
                {
                    adetSifirKomut.Parameters.AddWithValue("@search", "%" + textBox1.Text + "%");

                    using (SqlDataReader reader = adetSifirKomut.ExecuteReader())
                    {
                        string mesaj = "";
                        while (reader.Read())
                        {
                            mesaj += "Eser: " + reader["eser_adı"].ToString() +
                                     " | Yazar: " + reader["yazar"].ToString() + "\n";
                        }

                        if (!string.IsNullOrEmpty(mesaj))
                        {
                            MessageBox.Show("Kitap mevcut, ancak şu anda ödünçte:\n\n" + mesaj, "Kitap Mevcut Ancak Müsait Değil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Kitap mevcut ve ödünç alınabilir.", "Kitap Mevcut", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if(textBox1.Text== "Aramanızı giriniz")
            {
                textBox1.Text = "";
                textBox1.ForeColor= Color.Black;
            }
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            sifreDegistir sifreDegistirForm = new sifreDegistir();
            sifreDegistirForm.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            kullanıcıSorguİslemleri kullanıcıSorguİslemleriForm = new kullanıcıSorguİslemleri();
            kullanıcıSorguİslemleriForm.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            telNoGuncelle TNGForm = new telNoGuncelle();
            TNGForm.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    MessageBox.Show("Bağlantı başarılı!");
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Bağlantı başarısız!",ex.Message);
            }
        }
    }
}
