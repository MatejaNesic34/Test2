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


namespace Vezbanje2
{
    public partial class Form1 : Form
    {
        SqlConnection konekcija = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mateja\Desktop\Primenjene Vezbanje\Zadatak2\Vezbanje2\A3.mdf;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonUpis_Click(object sender, EventArgs e)
        {
            if (textBoxSifra.Text != "" && textBoxNaziv.Text != "" && textBoxDatum.Text != "" && textBoxBudzet.Text != "" && textBoxOpis.Text != "")
            {
                try
                {
                    SqlCommand command = new SqlCommand("INSERT INTO Projekat(ProjekatID, Naziv, DatumPocetka, Budzet, ProjekatZavrsen, Opis) VALUES(@Sifra, @Naziv, @Datum, @Budzet, @Zavrsen, @Opis)", konekcija);
                    konekcija.Open();
                    command.Parameters.AddWithValue("@Sifra", int.Parse(textBoxSifra.Text));
                    command.Parameters.AddWithValue("@Naziv", textBoxNaziv.Text);
                    command.Parameters.AddWithValue("@Datum", DateTime.ParseExact(textBoxDatum.Text, "dd/MM/yyyy", null));
                    command.Parameters.AddWithValue("@Budzet", float.Parse(textBoxBudzet.Text));
                    command.Parameters.AddWithValue("@Zavrsen", checkBoxZavrsen.Checked);
                    command.Parameters.AddWithValue("@Opis", textBoxOpis.Text);
                    command.ExecuteNonQuery();
                    konekcija.Close();
                    MessageBox.Show("Podaci uspesno upisani");
                    PrikazLV();
                    Ciscenje();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Popunite sve podatke!");
            }
        }

        private void buttonBrisi_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null)
            {
                try
                {
                    SqlCommand command = new SqlCommand("DELETE Projekat WHERE ProjekatID = @Id", konekcija);
                    konekcija.Open();
                    command.Parameters.AddWithValue("@Id", int.Parse(listView1.SelectedItems[0].SubItems[0].Text));
                    command.ExecuteNonQuery();
                    konekcija.Close();
                    MessageBox.Show("Podaci uspesno obrisani");
                    PrikazLV();
                    Ciscenje();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Izaberite red koji brišete");
            }
        }
        public void Ciscenje()
        {
            textBoxSifra.Clear();
            textBoxNaziv.Clear();
            textBoxDatum.Clear();
            textBoxBudzet.Clear();
            checkBoxZavrsen.Checked = false;
            textBoxOpis.Clear();
        }
        public void PrikazLV()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.Items.Clear();
            try
            {
                DataTable dataTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Projekat", konekcija);
                adapter.Fill(dataTable);
                foreach (DataRow row in dataTable.Rows)
                {
                    ListViewItem listItem = new ListViewItem(row["ProjekatID"].ToString());
                    listItem.SubItems.Add(row["Naziv"].ToString());
                    listItem.SubItems.Add(row["DatumPocetka"].ToString());
                    listItem.SubItems.Add(row["Budzet"].ToString());
                    listItem.SubItems.Add(row["ProjekatZavrsen"].ToString());
                    listItem.SubItems.Add(row["Opis"].ToString());
                    listView1.Items.Add(listItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PrikazLV();
        }

        private void buttonIzadji_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 statistika = new Form2();
            statistika.ShowDialog();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            textBoxSifra.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBoxNaziv.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBoxDatum.Text = listView1.SelectedItems[0].SubItems[2].Text;
            textBoxBudzet.Text = listView1.SelectedItems[0].SubItems[3].Text;
            textBoxOpis.Text = listView1.SelectedItems[0].SubItems[5].Text;
            if (listView1.SelectedItems[0].SubItems[4].Text == "True")
                checkBoxZavrsen.Checked = true;
            else
                checkBoxZavrsen.Checked = false;
        }
    }
}
