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
    public class StatusRepository
    {
        private readonly string _dbConnectionString;
        private const string SQL = "SELECT * FROM PersonStatuses";


        public StatusRepository(string dbConnectionString)
        {
            this._dbConnectionString = dbConnectionString;
        }

        private Status dataReadyToStatus(SqlDataReader dataReader)
        {
            Status returnMe = new Status()
            {
                PersonID = dataReader["PersonID"].ToString().Trim().ToInt(),
                Expires = dataReader["Expires"].ToString().Trim().ToDateTime(),
                Content = dataReader["StatusContent"].ToString().Trim(),
                StatusType = (StatusType)dataReader["StatusTypeID"].ToString().Trim().ToInt(),
            };

            return returnMe;
        }

        public List<Status> GetAll()
        {
            List<Status> returnMe = new List<Status>();

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
                        Status u = dataReadyToStatus(dbDataReader);
                        if (u != null)
                        {
                            returnMe.Add(u);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe.ToList();
        }


        public Status GetActiveForPerson(int ID)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL + " WHERE PersonID=@USERID ORDER BY Expires ASC;"
                };
                sqlCommand.Parameters.AddWithValue("USERID", ID);
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        Status u = dataReadyToStatus(dbDataReader);
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

        public List<Status> GetAllForPerson(int id)
        {
            List<Status> returnMe = new List<Status>();

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL + " WHERE PersonID=@USERID ORDER BY Expires ASC;"
                };
                sqlCommand.Parameters.AddWithValue("USERID", id);
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        Status u = dataReadyToStatus(dbDataReader);
                        if (u != null)
                        {
                            returnMe.Add(u);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe.ToList();
        }

    }
}