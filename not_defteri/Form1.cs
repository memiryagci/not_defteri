using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace not_defteri
{
    public partial class Form1 : Form
    {
        private void AlanlariTemizle()
        {
            rtbIcerik.Clear();
            txtBaslik.Clear();
            cmbKategori.SelectedIndex = -1;
        }

        List<Not> notlar = new List<Not>();

        public Form1()
        {
            InitializeComponent();
        }

        // 🔽 BURAYA ekle:
        private void NotlariKaydet()
        {
            string json = JsonConvert.SerializeObject(notlar, Formatting.Indented);
            File.WriteAllText("notlar.json", json);
        }

        private void NotlariYukle()
        {
            if (File.Exists("notlar.json"))
            {
                string json = File.ReadAllText("notlar.json");
                notlar = JsonConvert.DeserializeObject<List<Not>>(json);
                lstNotlar.Items.Clear();
                foreach (Not not in notlar)
                {
                    lstNotlar.Items.Add(not);
                }
            }
        }

    // Diğer buton tıklama olayları...



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Not gnclnot = (Not)lstNotlar.SelectedItem;
            if (lstNotlar.SelectedItem != null)
            {
                txtBaslik.Text = gnclnot.Baslik;
                cmbKategori.SelectedItem = gnclnot.Kategori;
                rtbIcerik.Text = gnclnot.Icerik;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("notdefteri.jpg"); // dosya adı neyse onu yaz
            this.BackgroundImageLayout = ImageLayout.Stretch; // veya Tile, Center, Zoom


            cmbKategori.Items.Add("Kişisel");
            cmbKategori.Items.Add("İş");
            cmbKategori.Items.Add("Okul");
            cmbKategori.Items.Add("Hobi");
            cmbKategori.Items.Add("Diğer");
            cmbKategori.Items.Add("Yemek");
            this.BackColor = Color.FromArgb(245, 245, 245); // Form arka planı

            // Yazı fontlarını ayarla
            txtBaslik.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            cmbKategori.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            rtbIcerik.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lstNotlar.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            // Buton renkleri ve stilleri
            btnEkle.BackColor = Color.LightGreen;
            btnEkle.FlatStyle = FlatStyle.Flat;
            btnEkle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            btnGuncelle.BackColor = Color.Khaki;
            btnGuncelle.FlatStyle = FlatStyle.Flat;
            btnGuncelle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            btnSil.BackColor = Color.Salmon;
            btnSil.FlatStyle = FlatStyle.Flat;
            btnSil.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            btnTemizle.BackColor = Color.LightBlue;
            btnTemizle.FlatStyle = FlatStyle.Flat;
            btnTemizle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // Liste kutusu arka planı ve rengi
            lstNotlar.BackColor = Color.WhiteSmoke;
            lstNotlar.ForeColor = Color.Black;

            // Not içeriği kutusu daha okunabilir
            rtbIcerik.BackColor = Color.White;
            rtbIcerik.ForeColor = Color.Black;

            NotlariYukle();


        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string baslik = txtBaslik.Text;
            string kategori = cmbKategori.SelectedItem?.ToString(); // seçili değilse null olabilir
            string icerik = rtbIcerik.Text;

            Not yeniNot = new Not();
            yeniNot.Baslik = baslik;
            yeniNot.Kategori = kategori;
            yeniNot.Icerik = icerik;
            yeniNot.Tarih = DateTime.Now;

            notlar.Add(yeniNot);

            lstNotlar.Items.Add(yeniNot);
            AlanlariTemizle();
            NotlariKaydet();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (lstNotlar.SelectedItem != null)
            {
                Not secilenNot = (Not)lstNotlar.SelectedItem;
                notlar.Remove(secilenNot); // Sadece seçileni kaldır
                lstNotlar.Items.Clear();   // Görüntüyü temizle
                foreach (Not not in notlar)
                {
                    lstNotlar.Items.Add(not); // Kalan notları yeniden listele
                }
                NotlariKaydet(); // Değişiklikleri kaydet

            }
            NotlariKaydet();
            AlanlariTemizle();

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            Not gnclnot = (Not)lstNotlar.SelectedItem;
            if (gnclnot != null)
            {
                gnclnot.Baslik = txtBaslik.Text;
                gnclnot.Kategori = cmbKategori.SelectedItem?.ToString();
                gnclnot.Icerik = rtbIcerik.Text;
                lstNotlar.Items.Clear(); // listeyi temizle
                foreach (Not not in notlar)
                {
                    lstNotlar.Items.Add(not); // güncellenmiş notları tekrar listeye ekle
                }
            }
            AlanlariTemizle();

            NotlariKaydet();
        }
    }
}
