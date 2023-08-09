using Data_Access_Layer_Contacts_APP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer_Contacts_App
{
    public class clsCountry
    {
        public string CountryName { set; get; }
        public int CountryID { set; get; }
        public string Code { set; get; }
        public string PhoneCode { set; get; }

        public enum eMode { UpdateMode,NewMode}
        public eMode Mode;
        public clsCountry() 
        {
            this.CountryID = default;
            this.CountryName = default;
            this.Code = this.PhoneCode = default;
            this.Mode = eMode.NewMode;
        }

        private clsCountry(string CountryName,int CountryID,string Code,string PhoneCode)
        {
            this.CountryName = CountryName;
            this.CountryID = CountryID;
            this.Code = Code;
            this.PhoneCode = PhoneCode;
            this.Mode = eMode.UpdateMode;
        }


        public static clsCountry FindCountryByName(string CountryName)
        {
            int CountryID = default;
            string Code = default, PhoneCode = default;

            if (DataAccess.FindCountryByName(ref CountryName, ref CountryID,ref Code,ref PhoneCode))
            {
                return new clsCountry(CountryName, CountryID, Code, PhoneCode);
            }
            else
                return null;
        }

        private bool _AddNewCountry()
        {
            return (DataAccess.AddNewCountry(this.CountryName, this.Code, this.PhoneCode) > 0);
        }

        public bool Save()
        {
            switch (this.Mode) 
            {
                case eMode.NewMode:
                    if (_AddNewCountry())
                    {
                        this.Mode = eMode.UpdateMode;
                        return true;
                    }
                    else
                        return false;
            }
            return false;
        }

        public static bool IsCountryExist(string CountryName)
        {
            return DataAccess.IsCountryExist(CountryName);
        }

        public static DataTable GetAllCountries()
        {
            return DataAccess.GetAllCountries();
        }
    }
}
