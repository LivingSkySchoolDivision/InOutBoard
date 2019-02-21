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

        private Person dataReaderToTrackedUser(SqlDataReader dataReader)
        {
            Person returnMe = new Person()
            {
                ID = dataReader["ID"].ToString().Trim().ToInt(),
                FirstName = dataReader["FirstName"].ToString().Trim(),
                LastName = dataReader["LastName"].ToString().Trim(),
                IsEnabled = dataReader["IsEnabled"].ToString().Trim().ToBool(),
            };

            return returnMe;
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
                        Person u = dataReaderToTrackedUser(dbDataReader);
                        if (u != null)
                        {
                            returnMe.Add(u);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();
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
                        Person u = dataReaderToTrackedUser(dbDataReader);
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