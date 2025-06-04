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
using System.Collections;

namespace Autobérlés
{
    public partial class Form1 : Form
    {
        public class Autok
        {
            public string Rendszam;
            public string Tipus;
            public string Gyarto;
            public string Modell;
            public int Km;

            public Autok(string sor)
            {
                string[] seged = sor.Split(';');
                this.Rendszam = seged[0];
                this.Tipus = seged[1];
                this.Gyarto = seged[2];
                this.Modell = seged[3];
                this.Km = Convert.ToInt32(seged[4]);

            }


        }

        public class Ugyfelek
        {
            public string Szemelyi;
            public string Nev;
            public string Lakcim;


            public Ugyfelek(string sor)
            {
                string[] seged = sor.Split(';');
                this.Szemelyi = seged[0];
                this.Nev = seged[1];
                this.Lakcim = seged[2];

            }


        }

        public class Berles
        {
            public string Rendszam;
            public string Szemelyi;
            public string Elviteli;
            public string Visszahozatal;
            public int Elvittkm;
            public int Visszahozottkm;
            public int Osszeg;


            public Berles(string sor)
            {

                try
                {
                    string[] seged = sor.Split(';');

                    Rendszam = seged[0];
                    Szemelyi = seged[1];
                    Elviteli = seged[2];
                    Visszahozatal = seged[3];
                    Elvittkm = Convert.ToInt32(seged[4]);

                    Osszeg = Convert.ToInt32(seged[6]);
                }
                catch (Exception)
                {
                    Rendszam = "";
                    Szemelyi = "";
                    Elviteli = "";
                    Visszahozatal = "";
                    Elvittkm = 0;

                    Osszeg = 0;

                }

                try
                {
                    string[] seged = sor.Split(';');

                    Visszahozottkm = Convert.ToInt32(seged[5]);
                }
                catch (Exception)
                {

                    Visszahozottkm = 0;
                }

            }

        }


        static List<Autok> autok = new List<Autok>();
        static List<Ugyfelek> ugyfelek = new List<Ugyfelek>();
        static List<Berles> berlesek = new List<Berles>();

        static bool isLoaded = false;
        static bool isChanged = false;
        static int selectedIndex = -1;

        static bool auto = false;
        static bool ugyfel = false;

        static int autoIndex = 0;
        static int ugyfelIndex = 0;

        public Form1()
        {
            InitializeComponent();
            AutokOlvasas();
            UgyfelOlvasas();
            BerlesOlvasas();
            UpdateAutoGrid();
            UpdateUgyfelGrid();
            UpdateBerlesGrid();
            UpdateStatisztika();
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker1.MinDate = DateTime.Now;

        }

        private void UpdateStatisztika()
        {

            listBox1.Items.Clear();
            int bevetel = 0;
            List<string> szemelyek = new List<string>();
            List<string> autokak = new List<string>();
            List<string> datumok = new List<string>();

            for (int i = 0; i < berlesek.Count; i++)
            {
                bevetel += berlesek[i].Osszeg;
                string auto = berlesek[i].Rendszam;
                autokak.Add(auto);
                string szemely = berlesek[i].Szemelyi;
                szemelyek.Add(szemely);
                string datum = berlesek[i].Elviteli;
                datumok.Add(datum);
            }

;
            int freq = 0;
            string res = "";
            for (int i = 0; i < autokak.Count; i++)
            {
                int count = 0;
                for (int j = i + 1; j < autokak.Count; j++)
                {
                    if (autokak[j] == autokak[i])
                    {
                        count++;
                    }
                }
                if (count >= freq)
                {
                    res = autokak[i];
                    freq = count;
                }
            }

            int ugyfelElofordulas = 0;
            string ugyfel = "";
            for (int i = 0; i < szemelyek.Count; i++)
            {
                int count = 0;
                for (int j = i + 1; j < szemelyek.Count; j++)
                {
                    if (szemelyek[j] == szemelyek[i])
                    {
                        count++;
                    }
                }

                if (count >= ugyfelElofordulas)
                {
                    ugyfel = szemelyek[i];
                    ugyfelElofordulas = count;
                }
            }

            int datumElofordulas = 0;
            for (int i = 0; i < datumok.Count; i++)
            {
                datumElofordulas++;
            }
            
            int kulonbozo = datumok.Distinct().Count();
            listBox1.Items.Add($"Összbevétel: {bevetel} ");
            listBox1.Items.Add($"Legtöbbet kölcsönzött autó: {res} ");
            listBox1.Items.Add($"Legtöbb autót kölcsönzött ügyfél: {ugyfel} ");
            listBox1.Items.Add($"Átlag bérbeadás naponként: {datumElofordulas / kulonbozo} ");

        }

        private void UpdateBerlesGrid()
        {
            berbeGrid.Rows.Clear();

            for (int i = 0; i < berlesek.Count; i++)
            {
                berbeGrid.Rows.Add();
                berbeGrid.Rows[berbeGrid.Rows.Count - 1].Cells[0].Value = berlesek[i].Rendszam;
                berbeGrid.Rows[berbeGrid.Rows.Count - 1].Cells[1].Value = berlesek[i].Szemelyi;
                berbeGrid.Rows[berbeGrid.Rows.Count - 1].Cells[2].Value = berlesek[i].Elviteli;
                berbeGrid.Rows[berbeGrid.Rows.Count - 1].Cells[3].Value = berlesek[i].Visszahozatal;
                berbeGrid.Rows[berbeGrid.Rows.Count - 1].Cells[4].Value = berlesek[i].Elvittkm;

                if (berlesek[i].Visszahozottkm == 0)
                {
                    berbeGrid.Rows[berbeGrid.Rows.Count - 1].Cells[5].Value = "";
                    berbeGrid.Rows[berbeGrid.Rows.Count - 1].Cells[6].Value = "";
                }
                else
                {
                    berbeGrid.Rows[berbeGrid.Rows.Count - 1].Cells[5].Value = berlesek[i].Visszahozottkm;
                    berbeGrid.Rows[berbeGrid.Rows.Count - 1].Cells[6].Value = berlesek[i].Visszahozottkm - berlesek[i].Elvittkm;
                }

            }


            SetDefaultState();

            isLoaded = true;
        }

        private void BerlesOlvasas()
        {
            try
            {
                StreamReader olvas = new StreamReader("berles.txt");
                while (!olvas.EndOfStream)
                {
                    berlesek.Add(new Berles(olvas.ReadLine()));
                }
                olvas.Close();
            }
            catch (IOException)
            {
                MessageBox.Show("Gond van");
            }
        }

        private void UgyfelOlvasas()
        {
            try
            {
                StreamReader olvas = new StreamReader("ugyfelek.txt");
                while (!olvas.EndOfStream)
                {
                    ugyfelek.Add(new Ugyfelek(olvas.ReadLine()));
                }
                olvas.Close();
            }
            catch (IOException)
            {
                MessageBox.Show("Gond van");
            }
        }

        private void AutokOlvasas()
        {
            try
            {
                StreamReader olvas = new StreamReader("autok.txt");
                while (!olvas.EndOfStream)
                {
                    autok.Add(new Autok(olvas.ReadLine()));
                }
                olvas.Close();
            }
            catch (IOException)
            {
                MessageBox.Show("Gond van");
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Nem adtál meg minden adatot");
            }
            else
            {

                Autok tetel = autok.Find(termek => termek.Rendszam == textBox1.Text);
                if (tetel != null)
                {
                    MessageBox.Show("Ilyen már van");
                }
                else
                {
                    isLoaded = false;
                    isChanged = true;

                    string row = textBox1.Text + ';' + textBox2.Text + ';' + textBox3.Text + ';' + textBox4.Text + ';' + textBox5.Text;

                    autok.Add(new Autok(row));

                    UpdateAutoGrid();
                    MessageBox.Show("Adat felvéve!");
                }
            }
        }

        private void UpdateAutoGrid()
        {

            autoGrid.Rows.Clear();
            autok.ForEach(item =>
            {
                autoGrid.Rows.Add();
                autoGrid.Rows[autoGrid.Rows.Count - 1].Cells[0].Value = item.Rendszam;
                autoGrid.Rows[autoGrid.Rows.Count - 1].Cells[1].Value = item.Tipus;
                autoGrid.Rows[autoGrid.Rows.Count - 1].Cells[2].Value = item.Gyarto;
                autoGrid.Rows[autoGrid.Rows.Count - 1].Cells[3].Value = item.Modell;
                autoGrid.Rows[autoGrid.Rows.Count - 1].Cells[4].Value = item.Km;



            });

            SetDefaultState();

            isLoaded = true;


        }

        private void SetDefaultState()
        {
            autoGrid.ClearSelection();
            ugyfelGrid.ClearSelection();
            berbeGrid.ClearSelection();

            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = false;

            textBox1.Text = "";
            textBox3.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
        }

        private void autoGrid_SelectionChanged(object sender, EventArgs e)
        {
            int index;
            if (isLoaded)
            {
                if (selectedIndex == -1)
                {
                    index = autoGrid.CurrentRow.Index;
                }
                else
                {
                    index = selectedIndex;
                }


                if (index > -1)
                {

                    AutoVisible();

                    textBox1.Text = autoGrid.Rows[index].Cells[0].Value.ToString();
                    textBox2.Text = autoGrid.Rows[index].Cells[1].Value.ToString();
                    textBox3.Text = autoGrid.Rows[index].Cells[2].Value.ToString();
                    textBox4.Text = autoGrid.Rows[index].Cells[3].Value.ToString();
                    textBox5.Text = autoGrid.Rows[index].Cells[4].Value.ToString();

                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = true;





                }


            }
        }

        private void AutoVisible()
        {
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;

            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;

            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;

            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;

            button7.Visible = false;
            button9.Visible = false;


            textBox6.Visible = false;
            textBox7.Visible = false;
            textBox8.Visible = false;

            dateTimePicker1.Visible = false;
            textBox11.Visible = false;

            label7.Visible = false;
            label9.Visible = false;
            label11.Visible = false;
            label12.Visible = false;

            label8.Visible = false;
            label10.Visible = false;
            label14.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Nem adtál meg minden adatot");
            }
            else
            {


                isLoaded = false;
                isChanged = true;
                int index = autoGrid.CurrentRow.Index;

                autok[index].Rendszam = textBox1.Text;
                autok[index].Tipus = textBox2.Text;
                autok[index].Gyarto = textBox3.Text;
                autok[index].Modell = textBox4.Text;
                autok[index].Km = Convert.ToInt32(textBox5.Text);


                UpdateAutoGrid();
                MessageBox.Show("Adat módosítva!");

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztosan törlöd az adatot?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                isLoaded = false;
                isChanged = true;

                autok.RemoveAt(autoGrid.CurrentRow.Index);
                UpdateAutoGrid();
                MessageBox.Show("Adat törölve");

            }
        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SetDefaultState();
            }
        }

        private void ugyfelGrid_SelectionChanged(object sender, EventArgs e)
        {
            int index;
            if (isLoaded)
            {
                if (selectedIndex == -1)
                {
                    index = ugyfelGrid.CurrentRow.Index;
                }
                else
                {
                    index = selectedIndex;
                }


                if (index > -1)
                {

                    UgyfelVisible();



                    textBox6.Text = ugyfelGrid.Rows[index].Cells[0].Value.ToString();
                    textBox7.Text = ugyfelGrid.Rows[index].Cells[1].Value.ToString();
                    textBox8.Text = ugyfelGrid.Rows[index].Cells[2].Value.ToString();


                    button4.Enabled = false;
                    button5.Enabled = true;
                    button6.Enabled = true;

                }
            }
        }

        private void UgyfelVisible()
        {
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;

            textBox6.Visible = true;
            textBox7.Visible = true;
            textBox8.Visible = true;

            label7.Visible = true;
            label9.Visible = true;
            label11.Visible = true;
            label12.Visible = true;





            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;

            button7.Visible = false;
            button9.Visible = false;


            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;

            dateTimePicker1.Visible = false;
            textBox11.Visible = false;


            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;

            label8.Visible = false;
            label10.Visible = false;
            label14.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("Nem adtál meg minden adatot");
            }
            else
            {
                Ugyfelek tetel = ugyfelek.Find(termek => termek.Szemelyi == textBox6.Text);
                if (tetel != null)
                {
                    MessageBox.Show("Ilyen már van");
                }
                else
                {
                    isLoaded = false;
                    isChanged = true;

                    string row = textBox6.Text + ';' + textBox7.Text + ';' + textBox8.Text;


                    ugyfelek.Add(new Ugyfelek(row));

                    UpdateUgyfelGrid();
                    MessageBox.Show("Adat módosítva!");
                }
            }
        }

        private void UpdateUgyfelGrid()
        {
            ugyfelGrid.Rows.Clear();
            ugyfelek.ForEach(item =>
            {
                ugyfelGrid.Rows.Add();
                ugyfelGrid.Rows[ugyfelGrid.Rows.Count - 1].Cells[0].Value = item.Szemelyi;
                ugyfelGrid.Rows[ugyfelGrid.Rows.Count - 1].Cells[1].Value = item.Nev;
                ugyfelGrid.Rows[ugyfelGrid.Rows.Count - 1].Cells[2].Value = item.Lakcim;

            });

            SetDefaultState();

            isLoaded = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("Nem adtál meg minden adatot");
            }
            else
            {

                isLoaded = false;
                isChanged = true;
                int index = ugyfelGrid.CurrentRow.Index;

                ugyfelek[index].Szemelyi = textBox6.Text;
                ugyfelek[index].Nev = textBox7.Text;
                ugyfelek[index].Lakcim = textBox8.Text;



                UpdateUgyfelGrid();
                MessageBox.Show("Adat módosítva!");

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Biztosan törlöd az adatot?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                isLoaded = false;
                isChanged = true;

                ugyfelek.RemoveAt(ugyfelGrid.CurrentRow.Index);
                UpdateUgyfelGrid();
                MessageBox.Show("Adat törölve");

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isChanged)
            {
                SaveToFile();
            }
        }

        private void SaveToFile()
        {
            try
            {
                StreamWriter file = new StreamWriter("autok.txt");
                foreach (var item in autok)
                {
                    file.WriteLine("{0};{1};{2};{3};{4}",
                        item.Rendszam,
                        item.Tipus,
                        item.Gyarto,
                        item.Modell,
                        item.Km);
                }
                file.Close();
                MessageBox.Show("Adatok mentve!");
            }
            catch (IOException ex)
            {
                MessageBox.Show("Hiba a mentéskor!", ex.Message);

            }

            try
            {
                StreamWriter file = new StreamWriter("ugyfelek.txt");

                foreach (var item in ugyfelek)
                {
                    file.WriteLine("{0};{1};{2}",
                        item.Szemelyi,
                        item.Nev,
                        item.Lakcim);
                }
                file.Close();

            }
            catch (IOException ex)
            {
                MessageBox.Show("Hiba a mentéskor!", ex.Message);

            }

            try
            {
                StreamWriter file = new StreamWriter("berles.txt");

                foreach (var item in berlesek)
                {
                    file.WriteLine("{0};{1};{2};{3};{4};{5};{6}",
                        item.Rendszam,
                        item.Szemelyi,
                        item.Elviteli,
                        item.Visszahozatal,
                        item.Elvittkm,
                        item.Visszahozottkm,
                        item.Osszeg);
                }
                file.Close();
                

            }
            catch (IOException ex)
            {
                MessageBox.Show("Hiba a mentéskor!", ex.Message);

            }


        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                AutoVisible();
                auto = true;
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                UgyfelVisible();
                ugyfel = true;
            }
            else
            {
                BerlesVisible();
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            
            try
            {
                autoIndex = autoGrid.CurrentRow.Index;
                ugyfelIndex = ugyfelGrid.CurrentRow.Index;
            }
            catch (Exception)
            {
                autoIndex = 0;
                ugyfelIndex = 0;

            }
            isChanged = true;

            BerlesHozzadas(autoIndex,ugyfelIndex);
            UpdateStatisztika();


        }

        private void BerlesHozzadas(int autoIndex, int ugyfelIndex)
        {
            
            isChanged = true;
            string elviteliDate = DateTime.Now.ToString("yyyy. MM. dd.");
            string visszahozatalDate = "";
            string visszahozottKm = "";

            string row = $"{autok[autoIndex].Rendszam};{ugyfelek[ugyfelIndex].Szemelyi};{elviteliDate};{visszahozatalDate};{autok[autoIndex].Km};{visszahozottKm};0";

            berlesek.Add(new Berles(row));

            UpdateBerlesGrid();

 
        }


        private void berbeGrid_SelectionChanged(object sender, EventArgs e)
        {

            int index;
            if (isLoaded)
            {
                if (selectedIndex == -1)
                {
                    index = berbeGrid.CurrentRow.Index;
                }
                else
                {
                    index = selectedIndex;
                }


                if (index > -1)
                {

                    BerlesVisible();

                    button7.Enabled = true;
                    button9.Enabled = true;

                }


            }
        }

        private void BerlesVisible()
        {
            button7.Visible = true;
            button9.Visible = true;

            dateTimePicker1.Visible = true;
            textBox11.Visible = true;

            label8.Visible = true;
            label10.Visible = true;
            label14.Visible = true;



            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;


            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            textBox8.Visible = false;


            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label9.Visible = false;
            label11.Visible = false;
            label12.Visible = false;


        }

        private void button7_Click(object sender, EventArgs e)
        {
            BerlesLeadas();
            UpdateStatisztika();

        }

        private void BerlesLeadas()
        {
            int parsedValue;
            if (!int.TryParse(textBox11.Text, out parsedValue))
            {
                MessageBox.Show("Üres a textbox vagy nem megfelelő szám");
                return;
            }

            int index = berbeGrid.CurrentRow.Index;

            if (Convert.ToInt32(textBox11.Text) <= Convert.ToInt32(berbeGrid.Rows[index].Cells[4].Value))
            {
                MessageBox.Show("A visszahozottKm nem lehet kisebb mint az eredeti");
            }
            else
            {
                isChanged = true;

                string visszahozottDate = dateTimePicker1.Value.ToShortDateString();

                berbeGrid.Rows[index].Cells[3].Value = visszahozottDate;
                berbeGrid.Rows[index].Cells[5].Value = textBox11.Text;
                berbeGrid.Rows[index].Cells[6].Value = Convert.ToInt32(textBox11.Text) - Convert.ToInt32(berbeGrid.Rows[index].Cells[4].Value.ToString());

                berlesek[index].Visszahozatal = visszahozottDate;
                berlesek[index].Visszahozottkm = Convert.ToInt32(textBox11.Text);
                berlesek[index].Osszeg = Convert.ToInt32(textBox11.Text) - Convert.ToInt32(berbeGrid.Rows[index].Cells[4].Value.ToString());
            }
           


        }
    }
}