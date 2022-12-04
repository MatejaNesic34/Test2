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

namespace Vezbanje1
{
    public partial class Form1 : Form
    {
        SqlConnection konekcija = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mateja\Desktop\Primenjene Vezbanje\Zadatak1\Vezbanje1\A1.mdf;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonIzadji_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void PrikazLV()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.Items.Clear();
            try
            {
                DataTable dataTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Citalac", konekcija);
                adapter.Fill(dataTable);              
                foreach (DataRow row in dataTable.Rows)
                {
                    ListViewItem listItem = new ListViewItem(row["CitalacID"].ToString());
                    listItem.SubItems.Add(row["MaticniBroj"].ToString());
                    listItem.SubItems.Add(row["Ime"].ToString());
                    listItem.SubItems.Add(row["Prezime"].ToString());
                    listItem.SubItems.Add(row["Adresa"].ToString());
                    listView1.Items.Add(listItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Ciscenje()
        {
            textBoxClanska.Clear();
            textBoxJmbg.Clear();
            textBoxIme.Clear();
            textBoxPrezime.Clear();
            textBoxAdresa.Clear();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            PrikazLV();
        }

        private void buttonUpis_Click(object sender, EventArgs e)
        {
            if (textBoxClanska.Text != "" && textBoxJmbg.Text != "" && textBoxIme.Text != "" && textBoxPrezime.Text != "" && textBoxAdresa.Text != "")
            {
                try
                {
                    SqlCommand command = new SqlCommand("INSERT INTO Citalac(CitalacID, MaticniBroj, Ime, Prezime, Adresa) VALUES(@Clanska, @Maticni, @Ime, @Prezime, @Adresa)", konekcija);
                    konekcija.Open();
                    command.Parameters.AddWithValue("@Clanska", textBoxClanska.Text);
                    command.Parameters.AddWithValue("@Maticni", textBoxJmbg.Text);
                    command.Parameters.AddWithValue("@Ime", textBoxIme.Text);
                    command.Parameters.AddWithValue("@Prezime", textBoxPrezime.Text);
                    command.Parameters.AddWithValue("@Adresa", textBoxAdresa.Text);
                    command.ExecuteNonQuery();
                    konekcija.Close();
                    MessageBox.Show("Podaci uspesno upisani");
                    PrikazLV();
                    Ciscenje();
                }
                catch (Exception)
                {
                    MessageBox.Show("Došlo je do greške");
                }
            }
            else
            {
                MessageBox.Show("Popunite sve podatke!");
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonObrisi_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null)
            {
                try
                {
                    SqlCommand command = new SqlCommand("DELETE Citalac WHERE CitalacID = @Id", konekcija);
                    konekcija.Open();
                    command.Parameters.AddWithValue("@Id", int.Parse(listView1.SelectedItems[0].SubItems[0].Text));
                    command.ExecuteNonQuery();
                    konekcija.Close();
                    MessageBox.Show("Podaci uspesno obrisani");
                    PrikazLV();
                    Ciscenje();
                }
                catch (Exception)
                {
                    MessageBox.Show("Došlo je do greške");
                }
            }
            else
            {
                MessageBox.Show("Izaberite red koji brišete");
            }

        }

        private void buttonIzmena_Click(object sender, EventArgs e)
        {
                if (textBoxClanska.Text != "" && textBoxJmbg.Text != "" && textBoxIme.Text != "" && textBoxPrezime.Text != "" && textBoxAdresa.Text != "")
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("UPDATE Citalac SET MaticniBroj = @jmbg, Ime =@Imee, Prezime = @Prez, Adresa = @Adresa WHERE CitalacId = @Id", konekcija);

                        konekcija.Open();
                        command.Parameters.AddWithValue("@Id", int.Parse(textBoxClanska.Text));
                        command.Parameters.AddWithValue("@Jmbg", textBoxJmbg.Text);
                        command.Parameters.AddWithValue("@Imee", textBoxIme.Text);
                        command.Parameters.AddWithValue("@Prez", textBoxPrezime.Text);
                        command.Parameters.AddWithValue("@Adresa", textBoxAdresa.Text);
                        command.ExecuteNonQuery();
                        konekcija.Close();
                        MessageBox.Show("Podaci uspesno izmenjeni");
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

        private void comboBoxCitaoci_Click(object sender, EventArgs e)
        {
            String strSQL = "select concat(Ime,' ',Prezime) as Citalac from Citalac";
            SqlCommand komanda = new SqlCommand(strSQL, konekcija);
            SqlDataAdapter adapter = new SqlDataAdapter(komanda);
            DataSet ds = new DataSet();
            try
            {
                konekcija.Open();
                adapter.Fill(ds, "Citalac");
                konekcija.Close();
                comboBoxCitaoci.DataSource = ds.Tables["Citalac"];
                comboBoxCitaoci.DisplayMember = "Citalac";
                comboBoxCitaoci.DropDownStyle = ComboBoxStyle.DropDownList;

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void buttonPrikaz_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@param";
                param.Value = comboBoxCitaoci.Text;
                String upitsql = "SELECT concat(Ime,' ',Prezime) as Citalac, DatumUzimanja, DatumVracanja FROM Citalac inner join Na_Citanju on Citalac.CitalacID = Na_Citanju.CitalacID where concat(Ime,' ',Prezime) = @param";
                SqlCommand komanda = new SqlCommand(upitsql, konekcija);
                komanda.Parameters.Add(param);
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            textBoxClanska.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBoxJmbg.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBoxIme.Text = listView1.SelectedItems[0].SubItems[2].Text;
            textBoxPrezime.Text = listView1.SelectedItems[0].SubItems[3].Text;
            textBoxAdresa.Text = listView1.SelectedItems[0].SubItems[4].Text;
        }
    }
}
