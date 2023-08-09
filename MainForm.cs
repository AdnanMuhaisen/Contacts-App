using Business_Layer_Contacts_App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3_Tier_Contacts_App__Windows_Forms_
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            _LoadDataInDataGrid();
        }


        private void _LoadDataInDataGrid()
        {
            dataGridView1.DataSource = clsContact.GetAllContacts();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt16(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
            if (MessageBox.Show($"Are You Want To Delete Contact With ID = {ID}",
                "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                clsContact.Delete(ID);
                dataGridView1.ClearSelection();
                _LoadDataInDataGrid();
                MessageBox.Show("Deleted");
            }
        }
        AddOrEditForm frm;
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt16(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value);
            frm = new AddOrEditForm(ID);
            frm.ShowDialog();
            dataGridView1.DataSource = clsContact.GetAllContacts();
        }

        private void btnAddNewContact_Click(object sender, EventArgs e)
        {
            frm = new AddOrEditForm(-1);
            frm.ShowDialog();
            dataGridView1.DataSource = clsContact.GetAllContacts();
        }
    }
}
