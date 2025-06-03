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
            btnTemizle.Click += BtnTemizle_Click;
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            AlanlariTemizle();
        }

        private void NotlariFiltrele(string aramaMetni = "")
        {
            lstNotlar.Items.Clear();
            var filtrelenmisNotlar = notlar;
            
            if (!string.IsNullOrEmpty(aramaMetni))
            {
                filtrelenmisNotlar = notlar.Where(n => n.IceriyorMu(aramaMetni)).ToList();
            }

            if (cmbKategoriFiltre.SelectedItem != null && cmbKategoriFiltre.SelectedItem.ToString() != "Tümü")
            {
                filtrelenmisNotlar = filtrelenmisNotlar.Where(n => n.Kategori == cmbKategoriFiltre.SelectedItem.ToString()).ToList();
            }

            foreach (Not not in filtrelenmisNotlar)
            {
                lstNotlar.Items.Add(not);
            }
        }

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
                NotlariFiltrele();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("notdefteri.jpg"))
                {
                    this.BackgroundImage = Image.FromFile("notdefteri.jpg");
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch (Exception)
            {
                // Arka plan resmi yüklenemezse sessizce devam et
            }

            // Kategori listesini oluştur
            string[] kategoriler = new[] { "Tümü", "Kişisel", "İş", "Okul", "Hobi", "Diğer", "Yemek" };
            cmbKategori.Items.AddRange(kategoriler.Skip(1).ToArray()); // "Tümü" hariç
            cmbKategoriFiltre.Items.AddRange(kategoriler);
            cmbKategoriFiltre.SelectedItem = "Tümü";

            this.BackColor = Color.FromArgb(245, 245, 245);

            // Yazı fontlarını ayarla
            txtBaslik.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            cmbKategori.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            rtbIcerik.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lstNotlar.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            txtArama.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            cmbKategoriFiltre.Font = new Font("Segoe UI", 10, FontStyle.Regular);

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

            lstNotlar.BackColor = Color.WhiteSmoke;
            lstNotlar.ForeColor = Color.Black;

            rtbIcerik.BackColor = Color.White;
            rtbIcerik.ForeColor = Color.Black;

            NotlariYukle();
        }

        private void lstNotlar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstNotlar.SelectedItem != null)
            {
                Not secilenNot = (Not)lstNotlar.SelectedItem;
                txtBaslik.Text = secilenNot.Baslik;
                cmbKategori.SelectedItem = secilenNot.Kategori;
                rtbIcerik.Text = secilenNot.Icerik;
            }
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            NotlariFiltrele(txtArama.Text);
        }

        private void cmbKategoriFiltre_SelectedIndexChanged(object sender, EventArgs e)
        {
            NotlariFiltrele(txtArama.Text);
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBaslik.Text) || cmbKategori.SelectedItem == null)
            {
                MessageBox.Show("Lütfen başlık ve kategori alanlarını doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Not yeniNot = new Not
            {
                Baslik = txtBaslik.Text,
                Kategori = cmbKategori.SelectedItem.ToString(),
                Icerik = rtbIcerik.Text,
                Tarih = DateTime.Now
            };

            notlar.Add(yeniNot);
            NotlariFiltrele(txtArama.Text);
            NotlariKaydet();
            AlanlariTemizle();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (lstNotlar.SelectedItem != null)
            {
                if (MessageBox.Show("Seçili notu silmek istediğinize emin misiniz?", "Onay", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Not secilenNot = (Not)lstNotlar.SelectedItem;
                    notlar.Remove(secilenNot);
                    NotlariFiltrele(txtArama.Text);
                    NotlariKaydet();
                    AlanlariTemizle();
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir not seçin.", "Uyarı", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (lstNotlar.SelectedItem == null)
            {
                MessageBox.Show("Lütfen güncellenecek bir not seçin.", "Uyarı", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBaslik.Text) || cmbKategori.SelectedItem == null)
            {
                MessageBox.Show("Lütfen başlık ve kategori alanlarını doldurunuz.", "Uyarı", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Not secilenNot = (Not)lstNotlar.SelectedItem;
            secilenNot.Baslik = txtBaslik.Text;
            secilenNot.Kategori = cmbKategori.SelectedItem.ToString();
            secilenNot.Icerik = rtbIcerik.Text;
            
            NotlariFiltrele(txtArama.Text);
            NotlariKaydet();
            AlanlariTemizle();
        }
    }
}
