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
        MainForm mainForm; //geçiş yapılan ana sayfayı tutatacak olan ana sayfa tipindeki değişken
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();

        public Settings( MainForm mf)
        {
            InitializeComponent();
            this.mainForm = mf;//geçiş yapılan sayfa ile local değişkeni eşleştirdik
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();//pencere kapatılınca ana sayfayı tekrar göster
        }
        /// <summary>
        /// Ekle butonuna basınca yapılacak olan işlemler
        /// dosyay tip ve ip ikilisini kaydediyor
        /// şifreli bir şekilde ekleme yapıyor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


      
        /// <summary>
        /// tabControlde sayfa değişince dosyadan okuma yapıp datagridi dolduruyor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {//data.dat isimli dosyadan okuma yapıp adresleri datagridwiev e çekiyor
          gridDoldur();
         }

        /// <summary>
        /// datagrid viewi dosyayı okuyup doldurmak için kullanılan fonksiyon
        /// </summary>
        void gridDoldur() { 
        dataGridView1.Rows.Clear();
            String file;
            String tmp;
            try
            {
                file = "data.dat";
                FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
                StreamReader r = new StreamReader(fs);
                listeleTipText.Text = "";
                listeleAdresText.Text = "";
                idLabel.Text = "0";
                ekleTipText.Text = "";
                ekleAdresText.Text = "";
                int i = 0;
                while (!r.EndOfStream)
                {
                    tmp = r.ReadLine();
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    string[] data = tmp.Split('-'); //tip ve ip ayrımı
                    string tip = data[0];//ayrımın ilk değeri tip diğeri ip
                    string adres = data[1];
                    row.Cells[0].Value = i++;
                    row.Cells[1].Value = Coz(Coz(tip));
                    row.Cells[2].Value = Coz(Coz(adres));
                    dataGridView1.Rows.Add(row);
                }
                r.Close();
                fs.Close();
            }
            catch (IOException e) {
                MessageBox.Show(e.ToString());
            }
        }
        
      
        /// <summary>
        /// datagridView tıklanıldığında listeleme textlerini dolduracak olan fonksiyon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

 

        private void guncelleButton_Click_1(object sender, EventArgs e)
        {
            islem(true);

        }
        private void silButton_Click(object sender, EventArgs e)
        {
            islem(false);
        }
        /// <summary>
        /// İşlem fonksiyonu güncelleme veya silme işlemi için kullanılan fonksiyondur
        /// </summary>
        /// <param name="durum"> true= güncelleme işlmi false= silme işlmei </param>
        void islem(bool durum) {

            try{
                int id = Convert.ToInt32(idLabel.Text.ToString());
                string kelime = Sifrele(Sifrele(listeleTipText.Text.ToString())) + "-" + Sifrele(Sifrele(listeleAdresText.Text.ToString()));

                // DosyaninSatiriniDegistir("data.dat",id,kelime);

                dataGridView1.Rows.Clear();
                String file;
                String tmp;
                file = "data.dat";
                FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
                StreamReader r = new StreamReader(fs);
                string icerik = "";
                int i = 0;
                while (!r.EndOfStream)
                {

                    tmp = r.ReadLine();
                    if (durum) //Durum True ise güncelleme işlemi false ise silme işlemi
                    {
                        if (i == id)
                        {
                            icerik += kelime + "\n"; // eğer belirtilen satırsa o satırı değiştirmek için onun sonuna değişrilecek olan satırı ekle
                        }
                        else
                        {
                            icerik += tmp + "\n"; //değilse bir sonrakini sonuna ekle
                        }
                    }
                    else {//silme durumu

                        if (i != id) 
                        {
                  
                            icerik += tmp + "\n";// satır numarası tutmuyorsa ekle aynıysa eklemeden bir sonraki satıra geç aradaki satırı sildir
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
     
                for (int j = 0; j < icerik.Split('\n').Count(); j++)
                {
                    string str = icerik.Split('\n')[j];
                    if (str != null && str != "" && str != " ")
                    {
                        Yazdirma1.WriteLine(str);
                    }
                }
                Yazdirma1.Close();
                fs1.Close();

                saveFileDialog1.InitialDirectory = Application.ExecutablePath;
                saveFileDialog1.AddExtension = true;

                gridDoldur();
           }
            catch (IOException e) {
                MessageBox.Show(e.ToString());
            }
        
        }

        /// <summary>
        /// ip ve tipi dossyaya yazarken şifrelemek için kullanılan basit base64 şifrelemesistemi
        /// </summary>
        /// <param name="veri"></param>
        /// <returns></returns>
        static public string Sifrele(string veri)
        {
            // gelen veri byte dizisine aktarılıyor
            byte[] veriByteDizisi = System.Text.ASCIIEncoding.ASCII.GetBytes(veri);
            // base64 şifreleme algoritmasına göre şifreleniyor.
            string sifrelenmisVeri = System.Convert.ToBase64String(veriByteDizisi);
            return sifrelenmisVeri;
        }
        /// <summary>
        /// ip ve tipi dossyaya okurken şifre çözmek için kullanılan basit base64 şifrelemesistemi
        /// </summary>
        /// <param name="cozVeri"></param>
        /// <returns></returns>
        static public string Coz(string cozVeri)
        {
            byte[] cozByteDizi = System.Convert.FromBase64String(cozVeri);
            string orjinalVeri = System.Text.ASCIIEncoding.ASCII.GetString(cozByteDizi);
            return orjinalVeri;

        }
       

    }
}
