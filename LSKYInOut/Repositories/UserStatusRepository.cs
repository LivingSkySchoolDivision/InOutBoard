using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSKYInOut
{
    public class UserStatusRepository
    {
        private StatusRepository _statusRepo;

        public UserStatusRepository()
        {
            _statusRepo = new StatusRepository();
        }

        public Status GetStatusForUser(int UserID) 
        {
            using (SqlConnection connection = new SqlConnection(Settings.DBConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM UserStatuses WHERE UserID=@USERID AND Expires>@NOWDATE"
                };
                sqlCommand.Parameters.AddWithValue("USERID", UserID);
                sqlCommand.Parameters.AddWithValue("NOWDATE", DateTime.Now);
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        return _statusRepo.Get(dbDataReader["StatusID"].ToString().Trim().ToInt());                        
                    }
                }

                sqlCommand.Connection.Close();
            }

            return _statusRepo.Get(-1);
        }
    }
}