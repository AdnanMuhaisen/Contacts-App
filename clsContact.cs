using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer_Contacts_APP;


namespace Business_Layer_Contacts_App
{
    public class clsContact
    {
        public enum eMode { EmptyContact,NewContact,UpdateMode}
        public eMode Mode = eMode.EmptyContact;
        public int ContactId { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Address { set; get; }
        public DateTime DateOfBirth { set; get; }
        public int CountryID { set; get; }
        public string Image { set; get; }

        public clsContact() 
        {
            this.ContactId = this.CountryID = default;
            this.FirstName = this.LastName = this.Email = this.Phone = this.Address = this.Image = default;
            this.DateOfBirth = default;
            Mode = eMode.NewContact;
        }
        private clsContact(int contactId, string firstName, string lastname, string email,
            string phone, string address, DateTime dateOfBirth, int countryID, string imageContact)
        {
            this.ContactId = contactId;
            this.FirstName = firstName;
            this.LastName = lastname;
            this.Email = email;
            this.Phone = phone;
            this.Address = address;
            this.DateOfBirth = dateOfBirth;
            this.CountryID = countryID;
            this.Image = imageContact;
            this.Mode = eMode.UpdateMode;
        }
            

        public static clsContact FindContactByContactID(int ID)
        {
            string FirstName = default, LastName = default, Email = default, Phone = default, Address = default, Image = default;
            DateTime DateOfBirth = default;
            int CountryID = default;

            if (DataAccess.GetContactByID(ID, ref FirstName,ref LastName,ref Email,ref Phone,
            ref Address,ref  DateOfBirth,ref CountryID,ref Image))
            {
                return new clsContact(ID, FirstName, LastName, Email, Phone, Address, DateOfBirth, CountryID,Image);
            }
            else
                return null;
        }

        private bool _AddNewContact()
        {
            this.ContactId = DataAccess.AddNewContact(this.FirstName, this.LastName, this.Email, this.Phone, this.Address, this.DateOfBirth,
                this.CountryID, this.Image);
            return (this.ContactId != -1);
        }
        private bool _Update()
        {
            if (DataAccess.UpdateContact(this.ContactId, this.FirstName, this.LastName, this.Email, this.Phone, this.Address, this.DateOfBirth,
                this.CountryID, this.Image) == 1)
            {
                return true;
            }
            else
                return false;
        }

        public bool Save()
        {
            switch (this.Mode) 
            {
                case eMode.NewContact:
                    if (_AddNewContact())
                    {
                        this.Mode = eMode.UpdateMode;
                        return true;
                    }
                    else
                        return false;
                case eMode.UpdateMode:
                    return (_Update()) ? true : false;
                default:
                    return false;
            }
        }

        public static bool Delete(int ContactID)
        {
            return (DataAccess.DeleteContact(ContactID) > 0 ? true : false); 
        }

        public static DataTable GetAllContacts()
        {
            return DataAccess.GetAllContacts();
        }
        public static bool IsContactExist(int ContactID)
        {
            return DataAccess.IsContactExist(ContactID);
        }

    }

}
