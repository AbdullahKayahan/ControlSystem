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
namespace ControlSystem
{
    public partial class Settings : Form
    {
        MainForm mainForm;
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        public Settings( MainForm mf)
        {
            InitializeComponent();
            this.mainForm = mf;
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
        }

        private void ekleButton_Click(object sender, EventArgs e)
        {
           
           
            saveFileDialog1.InitialDirectory = Application.ExecutablePath;
            saveFileDialog1.AddExtension = true;


            Stream fs1 = new FileStream("data.dat", FileMode.Append, FileAccess.Write);

            StreamWriter Yazdirma1 = new StreamWriter(fs1);
            Yazdirma1.WriteLine(Sifrele(Sifrele(ekleTipText.Text.ToString())) + "-" + Sifrele(Sifrele(ekleAdresText.Text.ToString())));
            Yazdirma1.Close();
            fs1.Close();

            saveFileDialog1.InitialDirectory = Application.ExecutablePath;
            saveFileDialog1.AddExtension = true;
        }


      

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {//data.dat isimli dosyadan okuma yapıp adresleri datagridwiev e çekiyor
          gridDoldur();
         }

        void gridDoldur() { 
        dataGridView1.Rows.Clear();
            String file;
            String tmp;
            file = "data.dat";
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            StreamReader r = new StreamReader(fs);
            listeleTipText.Text = "";
            listeleAdresText.Text = "";
            ekleTipText.Text = "";
            ekleAdresText.Text = "";
            int i = 0;
            while (!r.EndOfStream)
            {
             
                tmp = r.ReadLine();
                
                DataGridViewRow row = (DataGridViewRow) dataGridView1.Rows[0].Clone();
                string[] data = tmp.Split('-');
                string tip = data[0];
                string adres = data[1];
                row.Cells[0].Value = i++;
                row.Cells[1].Value = Coz(Coz(tip));
                row.Cells[2].Value = Coz(Coz(adres));
                dataGridView1.Rows.Add(row);
            }
            r.Close();
            fs.Close();
           
        }
        
        static public string Sifrele(string veri)
        {
            // gelen veri byte dizisine aktarılıyor
            byte[] veriByteDizisi = System.Text.ASCIIEncoding.ASCII.GetBytes(veri);
            // base64 şifreleme algoritmasına göre şifreleniyor.
            string sifrelenmisVeri = System.Convert.ToBase64String(veriByteDizisi);
            return sifrelenmisVeri;
        }

        static public string Coz(string cozVeri)
        {
            byte[] cozByteDizi = System.Convert.FromBase64String(cozVeri);
            string orjinalVeri = System.Text.ASCIIEncoding.ASCII.GetString(cozByteDizi);
            return orjinalVeri;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                idLabel.Text = (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                listeleTipText.Text = (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                listeleAdresText.Text = (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
            catch
            {
                MessageBox.Show("Lütfen Uygun Bir Seçim Yapınız", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                listeleTipText.Text = "";
                listeleAdresText.Text = "";
            }
        }

 
        public void DosyaninSatiriniDegistir(string path, int index, string yeniKelime)
        {
          
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            string satir = "", icerik = "";
            int i = 0;
            while ((satir = sr.ReadLine()) != null)
            {
                if (i == index)
                {
                    icerik += yeniKelime + "\n";
                }
                else
                {
                    icerik += satir + "\n";
                }
                i++;
            }
            fs.Flush();
            sr.Close();
            fs.Close();
            fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            for (int j = 0; j < i; j++)
            {
                string str = icerik.Split('\n')[j];
                if (str != null && str != "" && str != " ")
                {
                    sw.WriteLine(str);
                }
            }
            fs.Flush();
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        private void guncelleButton_Click_1(object sender, EventArgs e)
        {
            islem(true);

        }
        private void silButton_Click(object sender, EventArgs e)
        {
            islem(false);
        }

        void islem(bool durum) {


            int id = Convert.ToInt32(idLabel.Text.ToString());
            string kelime = Sifrele(Sifrele(listeleTipText.Text.ToString())) + "-" + Sifrele(Sifrele(listeleAdresText.Text.ToString()));

            // DosyaninSatiriniDegistir("data.dat",id,kelime);

            dataGridView1.Rows.Clear();
            String file;
            String tmp;
            file = "data.dat";
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            StreamReader r = new StreamReader(fs);
            string satir = "", icerik = "";
            int i = 0;
            while (!r.EndOfStream)
            {

                tmp = r.ReadLine();
                if (durum) //Durum True ise güncelleme işlemi false ise silme işlemi
                {
                    if (i == id)
                    {
                        icerik += kelime + "\n";
                    }
                    else
                    {
                        icerik += tmp + "\n";
                    }
                }
                else {

                    if (i != id)
                    {
                  
                        icerik += tmp + "\n";
                    }
                    
                }
                i++;
            }
            r.Close();
            fs.Close();


            saveFileDialog1.InitialDirectory = Application.ExecutablePath;
            saveFileDialog1.AddExtension = true;


            Stream fs1 = new FileStream("data.dat", FileMode.Create, FileAccess.Write);

            StreamWriter Yazdirma1 = new StreamWriter(fs1);
            i--;
            for (int j = 0; j < icerik.Split('\n').Count(); j++)
            {
                string str = icerik.Split('\n')[j];
                if (str != null && str != "" && str != " ")
                {
                    Yazdirma1.WriteLine(str);
                }
            }

            // Yazdirma1.WriteLine(Sifrele(Sifrele(ekleTipText.Text.ToString())) + "-" + Sifrele(Sifrele(ekleAdresText.Text.ToString())));
            Yazdirma1.Close();
            fs1.Close();

            saveFileDialog1.InitialDirectory = Application.ExecutablePath;
            saveFileDialog1.AddExtension = true;

            gridDoldur();
        
        
        }


       

    }
}
