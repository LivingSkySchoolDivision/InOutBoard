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

        private UserStatus dataReaderToUserStatus(SqlDataReader dataReader)
        {
            // Check if the status is legit
            Status status = _statusRepo.Get(dataReader["StatusID"].ToString().Trim().ToInt());
            if (status.ID > 0)
            {
                return new UserStatus()
                {
                    Status = status,
                    Expires = Parsers.ToDateTime(dataReader["Expires"].ToString().Trim())
                };
            }
            return null;
        }

        public List<UserStatus> GetStatusForUser(int UserID) 
        {
            List<UserStatus> returnMe = new List<UserStatus>();

            using (SqlConnection connection = new SqlConnection(Settings.DBConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM UserStatuses WHERE UserID=@USERID AND Expires>GETDATE() ORDER BY Expires ASC;"
                };
                sqlCommand.Parameters.AddWithValue("USERID", UserID);
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        UserStatus parsedStatus = dataReaderToUserStatus(dbDataReader);
                        if (parsedStatus != null)
                        {
                            returnMe.Add(parsedStatus);
                        }                        
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe;
        }
    }
}