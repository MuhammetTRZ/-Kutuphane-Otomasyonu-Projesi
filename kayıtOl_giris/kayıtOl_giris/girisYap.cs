using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;
namespace kayıtOl_giris

{
    public partial class girisYap : Form
    {
        public girisYap()
        {
            InitializeComponent();
        }
        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size=4096;user id=MuhammetTRZ_SQLLogin_1;pwd=9hbeo7oosu;data source=Muhammet.mssql.somee.com;persist security info=False;initial catalog=Muhammet;TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);    


        public static bool SifreDogrula(string girilenSifre, string veriTabanındakiSifre)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(girilenSifre, veriTabanındakiSifre);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                return false;
            }


        }
        private void button1_Click(object sender, EventArgs e)
        {
            //kullanıcı bilgileri eksik mi girmiş kontrol eder.
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Lütfen bilgilerinizi eksiksiz giriniz.");
                return;
            }

            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                
                connect.Open();
                // Kullanıcının TC kimlik numarasına göre veritabanındaki hashlenmiş şifreyi al
                string sifreAlanSqlKod = "SELECT sifre FROM try_to_login WHERE tc = @tc";
                using (SqlCommand komut2 = new SqlCommand(sifreAlanSqlKod, connect))
                {
                    komut2.Parameters.Add("@tc", SqlDbType.BigInt).Value = Convert.ToInt64(textBox1.Text);
                    object sonuc = komut2.ExecuteScalar();//tek bir değer dönmesini bekleyen döngü

                    if (sonuc != null)
                    {
                        string veriTabanındakiHash = sonuc.ToString();

                        // Kullanıcının girdiği şifreyi hash ile doğrula
                        if (SifreDogrula(textBox2.Text,veriTabanındakiHash))
                        {
                            // Şifre doğruysa sadece tc ile giriş yap
                            string girisSqlKod = "SELECT kimlik, tc FROM try_to_login WHERE tc=@tc";
                            using (SqlCommand komut = new SqlCommand(girisSqlKod, connect))
                            {
                                komut.Parameters.AddWithValue("@tc", textBox1.Text);

                                using (SqlDataReader reader = komut.ExecuteReader())
                                {
                                    if (reader.Read())//kullanıcı bilgileri var ise
                                    {
                                        int kimlik = (int)reader["kimlik"];
                                        long tc = (long)reader["tc"];

                                        //// Kullanıcının girdiği TC kimlik numarası ile veritabanındaki eşleşiyor mu kontrol et
                                        if (long.TryParse(textBox1.Text, out long tcFromTextbox))
                                        {
                                            if(tc == tcFromTextbox)
                                            {
                                                MessageBox.Show("Girişiniz yapılmıştır.");
                                                anaSayfa anaSayfaForm = new anaSayfa(kimlik, tc);//anaSayfaForm'a iki değer götür
                                                anaSayfaForm.Show();
                                                this.Hide();
                                            }
                                            else
                                            {
                                                MessageBox.Show("TC kimlik numaranız eşleşmiyor.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Kullanıcı bulunamadı.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Hatalı şifre veya TC girdiniz. Şifre: " + textBox2.Text + ", TC: " + textBox1.Text, "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Böyle bir kullanıcı bulunamadı.");
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            kayıtcs kayıtForm = new kayıtcs();
            kayıtForm.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form1 menüForm = new Form1();
            menüForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Hide();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back){
                e.Handled = true;
            }
        }

        private void girisYap_Load(object sender, EventArgs e)
        {
           
            textBox1.MaxLength = 11;

            //85362
   

            

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //string pattern = @"^(\d{11})";
            //if (Regex.IsMatch(textBox1.Text, pattern))
            //{
            //    pictureBox5.Visible = true;
            //    pictureBox3.Visible = false;
            //}
            //else
            //{
            //    pictureBox5.Visible = false;
            //    pictureBox3.Visible = true;
            //}
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //string pattern = @"^\d{5}";
            //string pattern2 = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[-.\/]).{8,}$";
            //if (Regex.IsMatch(textBox2.Text,pattern)||Regex.IsMatch(textBox2.Text,pattern2))
            //{
            //    pictureBox2.Visible = true;
            //    pictureBox4.Visible = false;
            //}
            //else
            //{
            //    pictureBox2.Visible = false;
            //    pictureBox4.Visible = true;
            //}
        }

        private void button11_Click(object sender, EventArgs e)
        {
            sifreDegistir sifreDegistirform = new sifreDegistir();
            sifreDegistirform.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            telNoGuncelle TNGForm = new telNoGuncelle();
            TNGForm.Show();
            this.Hide();
        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click_1(object sender, EventArgs e)
        {
        }

        private void button13_Click_2(object sender, EventArgs e)
        {
            string veriTabanındakiHash = "$2a$11$8G3Z7LSkV3llUSjTaN7KrOXIGuwMrKwRvx4TtCglY5yuonVEwc.yS";
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();
                string databasedenHaslemisSifreAlanSqlKodu = "select sifre from try_to_login where tc=@tc";
                using (SqlCommand komut = new SqlCommand(databasedenHaslemisSifreAlanSqlKodu, connect))
                {
                    komut.Parameters.AddWithValue("@tc",textBox1.Text);
                    object sonuc = komut.ExecuteScalar();
                    if (sonuc != null)
                    {
                        string haslenmisSifre=sonuc.ToString();
                        if (haslenmisSifre == veriTabanındakiHash)
                        {
                            MessageBox.Show("eşittir");
                        }
                        else
                        {
                            MessageBox.Show("değildir");
                        }
                    }
                }
            }
        }
    }
}