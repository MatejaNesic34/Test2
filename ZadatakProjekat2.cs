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
    public partial class Form2 : Form
    {
        SqlConnection konekcija = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mateja\Desktop\Primenjene Vezbanje\Zadatak2\Vezbanje2\A3.mdf;Integrated Security=True");
        public Form2()
        {
            InitializeComponent();
        }
        private void PrikazDG()
        {
            try
            {
                DataTable dataTable = new DataTable();
                SqlCommand komanda = new SqlCommand("SELECT YEAR(DatumPocetka) as Godina, ProjekatID, Naziv FROM Projekat where YEAR(GETDATE()) - YEAR(DatumPocetka) <= @param ORDER BY Godina DESC", konekcija);
                komanda.Parameters.AddWithValue("@param", numericUpDown1.Value);
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception)
            {
                MessageBox.Show("Došlo je do greške");
            }
        }
        private void PrikazLV()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.Items.Clear();
            try
            {
                DataTable dataTable = new DataTable();
                SqlCommand komanda = new SqlCommand("SELECT YEAR(DatumPocetka) as Godina, ProjekatID, Naziv FROM Projekat where YEAR(GETDATE()) - YEAR(DatumPocetka) <= @param ORDER BY YEAR(DatumPocetka) DESC", konekcija);
                komanda.Parameters.AddWithValue("@param", numericUpDown1.Value);
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                adapter.Fill(dataTable);
                foreach (DataRow row in dataTable.Rows)
                {
                    ListViewItem listItem = new ListViewItem(row["Godina"].ToString());
                    listItem.SubItems.Add(row["ProjekatID"].ToString());
                    listItem.SubItems.Add(row["Naziv"].ToString());
                    listView1.Items.Add(listItem); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            PrikazDG();
            PrikazLV();
        }
    }
}
