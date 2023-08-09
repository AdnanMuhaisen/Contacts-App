using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer_Contacts_APP
{
    public static class DataAccess
    {
        static string ConnectionString = "Server=.;Database = Contacts_DB;User ID=sa;Password=sa123456;";

        public static bool GetContactByID(int ID,ref string FirstName, ref string LastName, ref string Email, ref string Phone,
           ref string Address, ref DateTime DateOfBirth, ref int CountryID, ref string Image)
        {
            bool IsContactExist = false;
            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = "select * from Contacts where ContactID=@ID";
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    FirstName = reader["FirstName"].ToString();
                    LastName = reader["LastName"].ToString();
                    Email = reader["Email"].ToString();
                    Phone = reader["Phone"].ToString();
                    Address = reader["Address"].ToString();
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    CountryID = Convert.ToInt16(reader["CountryID"]);
                    Image = (reader["ImagePath"].ToString().Equals( DBNull.Value)) ? default : reader["ImagePath"].ToString();
                    IsContactExist = true;
                }
                else
                    IsContactExist = false;
                reader.Close();
            }
            catch(Exception) { IsContactExist = false; }
            finally { connection.Close(); }

            return IsContactExist;
        }

        public static int AddNewContact( string FirstName,  string LastName, string Email, string Phone,
            string Address,  DateTime DateOfBirth,  int CountryID, string Image)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = @"insert into Contacts values (@FirstName,@LastName,@Email,@Phone,@Address,@DateOfBirth,@CountryID,@ImagePath);
                           select scope_identity(); ";
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            if (Image == "")
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            else
                command.Parameters.AddWithValue("@ImagePath", Image);
            int ID = -1;

            try
            {
                connection.Open();
                ID = Convert.ToInt16(command.ExecuteScalar());
            }
            catch (Exception) { return ID; }
            finally{ connection.Close(); }
            return ID;
        }

        public static byte DeleteContact(int ContactID)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = "delete from Contacts where ContactID = @ContactID";
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ContactID", ContactID);
            byte RowsAffected = 0;

            try
            {
                connection.Open();
                RowsAffected = Convert.ToByte(command.ExecuteNonQuery());
            }
            catch (Exception) { return 0; }
            finally { connection.Close(); }

            return RowsAffected;
        }

        public static byte UpdateContact(int ContactID, string FirstName, string LastName, string Email, string Phone,
            string Address, DateTime DateOfBirth, int CountryID, string Image)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = @"Update Contacts
                            set FirstName = @FirstName,LastName=@LastName,Email=@Email,Phone=@Phone,Address=@Address,DateOfBirth=@DateOfBirth,
                            CountryID=@CountryID,ImagePath=@Image
                            where ContactID=@ContactID;";
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ContactID", ContactID);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@CountryID", CountryID);

            if (Image.Equals(default))
                command.Parameters.AddWithValue("@Image", DBNull.Value);
            else
                command.Parameters.AddWithValue("@Image", Image);

            byte RowsAffected = 0;

            try
            {
                connection.Open();
                RowsAffected = Convert.ToByte(command.ExecuteNonQuery());
                
            }
            catch (Exception) { return 0; }
            finally { connection.Close(); }

            return RowsAffected;
        }

        public static DataTable GetAllContacts()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = "select * from Contacts";
            SqlCommand command = new SqlCommand(Query, connection);
            DataTable Dt = new DataTable();

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    Dt.Load(reader);

                reader.Close();
            }
            catch (Exception) { return null; }
            finally { connection.Close(); }

            return Dt;
        }
        
        public static bool IsContactExist(int ContactID)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = "select ContactID from Contacts where ContactID = @ContactID";
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ContactID", ContactID);
            byte ID = 0;

            try
            {
                connection.Open();
                ID = Convert.ToByte(command.ExecuteScalar());
            }
            catch (Exception) { return false; }
            finally { connection.Close(); }

            return (ID == ContactID);
        }

        public static bool FindCountryByName(ref string CountryName,ref int CountryID,ref string Code,ref string PhoneCode)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = "select * from Countries where CountryName = @CountryName";
            SqlCommand command = new SqlCommand(Query, connection);
            bool IsFound = false;

            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CountryName = reader["CountryName"].ToString();
                    CountryID = Convert.ToInt16(reader["CountryID"]);

                    if (reader["Code"] == DBNull.Value)
                        Code = default;
                    else
                        Code = reader["Code"].ToString();

                    if (reader["PhoneCode"] == DBNull.Value)
                        PhoneCode = default;
                    else
                        PhoneCode = reader["PhoneCode"].ToString();
                
                    IsFound = true;
                }
                reader.Close();
            }
            catch (Exception) { return IsFound; }
            finally { connection.Close(); }

            return IsFound;
        }

        public static int AddNewCountry(string CountryName,string Code,string PhoneCode)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = @"insert into Countries values (@CountryName,@Code,@PhoneCode);
                            select Scope_Identity();";
            SqlCommand command = new SqlCommand(Query, connection);
            int CountryID = 0;

            command.Parameters.AddWithValue("@CountryName", CountryName);
            command.Parameters.AddWithValue("@Code", Code);
            command.Parameters.AddWithValue("@PhoneCode", PhoneCode);

            try
            {
                connection.Open();
                CountryID = Convert.ToInt16(command.ExecuteScalar());
            }
            catch (Exception) { return CountryID; }
            finally { connection.Close(); }

            return CountryID;
        }

        public static bool IsCountryExist(string CountryName)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = "select Found = 1 from Countries Where CountryName = @CountryName";
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@CountryName", CountryName);
            byte Found = 0;

            try
            {
                connection.Open();
                Found = Convert.ToByte(command.ExecuteScalar());
            }
            catch (Exception) { return false; }
            finally { connection.Close(); }

            return (Found != 0);
        }

        public static DataTable GetAllCountries()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string Query = "Select * From Countries";
            SqlCommand command = new SqlCommand(Query, connection);
            DataTable DT = new DataTable();

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    DT.Load(reader);

                reader.Close();
            }
            catch (Exception) { return null; }
            finally { connection.Close(); }

            return DT;
        }
    }
}
