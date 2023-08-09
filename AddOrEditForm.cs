using _3_Tier_Contacts_App__Windows_Forms_.Properties;
using Business_Layer_Contacts_App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _3_Tier_Contacts_App__Windows_Forms_
{
    public partial class AddOrEditForm : Form
    {
        public enum eMode { AddNew, Update }
        public eMode Mode;
        public clsContact contact;


        public AddOrEditForm(int ContactID)
        {
            InitializeComponent();
            contact = clsContact.FindContactByContactID(ContactID);

            if (contact == null)
            {
                this.Mode = eMode.AddNew;
                contact = new clsContact();
            }
            else
            {
                //the object already store data
                this.Mode = eMode.Update;
            }
        }

        private void _GetDataFromControls()
        {
            contact.FirstName = txtboxFirstName.Text;
            contact.LastName = txtboxLastName.Text;
            contact.Email = txtboxEmail.Text;
            contact.Phone = txtboxPhone.Text;
            contact.Address = txtboxAddress.Text;
            contact.DateOfBirth = Convert.ToDateTime(dateTimePicker1.Value);
            contact.CountryID = comboboxCountries.SelectedIndex + 1;
            //test this code :
            if (pictureBox1.ImageLocation == "" || pictureBox1.Image == default) 
                contact.Image = "";
            else
                contact.Image = pictureBox1.ImageLocation;
        }

        private void _ShowData()
        {
            txtboxFirstName.Text = contact.FirstName;
            txtboxLastName.Text = contact.LastName;
            txtboxEmail.Text = contact.Email;
            txtboxPhone.Text = contact.Phone;
            txtboxAddress.Text = contact.Address;
            comboboxCountries.Text = comboboxCountries.Items[contact.CountryID - 1].ToString();
            if(contact.Image != "")
            {
                pictureBox1.Image = Image.FromFile(contact.Image);
                pictureBox1.ImageLocation = contact.Image;
                linkLabelRemoveImage.Visible = true;
            }
        }

        private void Save()
        {
            switch (this.Mode)
            {
                case eMode.AddNew:
                    _GetDataFromControls();
                    contact.Save();
                    break;
                case eMode.Update:
                    _GetDataFromControls();
                    contact.Save();
                    break;
            }
        }

        private void linkLabelSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Title = "Set Contact Image";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = ".jpg|*.jpg|.png|*.png|All Files |*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                contact.Image = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.ImageLocation = openFileDialog1.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            _ShowData();
            Mode = eMode.Update;
            lblMode.Text = "Edit Contact With ID = " + contact.ContactId;
            //this.Close();
        }

        private void AddOrEditForm_Load(object sender, EventArgs e)
        {
            _GetCountriesToComboBox();
            if (this.Mode == eMode.AddNew)
            {
                lblMode.Text = "Add New Contact";
            }
            else
            {
                lblMode.Text = "Edit Contact With ID = " + contact.ContactId;
                _ShowData();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabelRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBox1.Image = default;
        }

        private void _GetCountriesToComboBox()
        {
            DataTable DT = clsCountry.GetAllCountries();
            foreach (DataRow dr in DT.Rows) 
            {
                comboboxCountries.Items.Add(dr["CountryName"].ToString());
            }
        }
    }
}     