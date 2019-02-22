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
                StatusID = dataReader["StatusID"].ToString().Trim().ToInt(),
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

        public Status GetActiveForPerson(int PersonID)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL + " WHERE PersonID=@USERID ORDER BY Expires ASC;"
                };
                sqlCommand.Parameters.AddWithValue("USERID", PersonID);
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

        public Status Get(int StatusID)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL + " WHERE StatusID=@STATUSID"
                };
                sqlCommand.Parameters.AddWithValue("STATUSID", StatusID);
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

        public List<Status> GetAllForPerson(int PersonID)
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
                sqlCommand.Parameters.AddWithValue("USERID", PersonID);
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
                
        public void DeleteExpiredStatuses()
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "DELETE FROM PersonStatuses WHERE Expires<GETDATE();"
                };
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }
        }

        public void AddStatus(Status status)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "INSERT INTO PersonStatuses(PersonID, Expires, StatusContent, StatusTypeID) VALUES(@PERSONID,@EXPIRYDATE,@STATUSCONTENT,@STATUSTYPE);"
                };
                sqlCommand.Parameters.AddWithValue("PERSONID", status.PersonID);
                sqlCommand.Parameters.AddWithValue("EXPIRYDATE", status.Expires);
                sqlCommand.Parameters.AddWithValue("STATUSCONTENT", status.Content);
                sqlCommand.Parameters.AddWithValue("STATUSTYPE", status.StatusType);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }
        }

        public void ClearStatusesForPerson(Person person)
        {
            ClearStatusesForPerson(person.ID);
        }

        public void ClearStatusesForPerson(int PersonID)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "DELETE FROM PersonStatuses WHERE PersonID=@PERSONID;"
                };
                sqlCommand.Parameters.AddWithValue("PERSONID", PersonID);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }
        }

        public void DeleteStatus(Status status)
        {
            DeleteStatus(status.StatusID);
        }

        public void DeleteStatus(int StatusID)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "DELETE FROM PersonStatuses WHERE StatusID=@StatusID;"
                };
                sqlCommand.Parameters.AddWithValue("USERID", StatusID);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }
        }

    }
}