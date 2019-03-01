using LSSDInOutLib.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LSSDInOutLib.Helpers;

namespace LSSDInOutAPI.Repositories
{
    public class PersonRepository
    {
        private readonly string _dbConnectionString;
        private const string SQL = "SELECT * FROM People";

        public PersonRepository(string dbConnectionString)
        {
            this._dbConnectionString = dbConnectionString;
        }

        private Person dataReaderToPerson(SqlDataReader dataReader)
        {
            // Parse group IDs
            List<string> groupIDString = dataReader["GroupMemberships"].ToString().Trim().Split(';').ToList();
            List<int> groupIDs = new List<int>();
            foreach(string grpIDString in groupIDString)
            {
                int grpID = grpIDString.ToInt();
                if (grpID > 0)
                {
                    if (!groupIDs.Contains(grpID))
                    {
                        groupIDs.Add(grpID);
                    }
                }
            }
            
            return new Person()
            {
                ID = dataReader["ID"].ToString().Trim().ToInt(),
                FirstName = dataReader["FirstName"].ToString().Trim(),
                LastName = dataReader["LastName"].ToString().Trim(),
                IsEnabled = dataReader["IsEnabled"].ToString().Trim().ToBool(),
                GroupsIDs = groupIDs, 
            };
        }

        public List<Person> GetAll()
        {
            List<Person> returnMe = new List<Person>();

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL
                };
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        Person u = dataReaderToPerson(dbDataReader);
                        if (u != null)
                        {
                            returnMe.Add(u);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe.OrderBy(u => u.FirstName).ThenBy(u => u.LastName).ToList();
        }


        public Person Get(int ID)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL + " WHERE People.ID=@USERID;"
                };
                sqlCommand.Parameters.AddWithValue("USERID", ID);
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        Person u = dataReaderToPerson(dbDataReader);
                        if (u != null)
                        {
                            return u;
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return null;
        }
    }
}