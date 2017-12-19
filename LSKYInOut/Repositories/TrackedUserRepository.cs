using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSKYInOut
{
    public class TrackedUserRepository
    {
        private const string SQL = "SELECT * FROM Users";
        private UserStatusRepository _userStatusRepo;

        public TrackedUserRepository()
        {
            _userStatusRepo = new UserStatusRepository();
        }


        private TrackedUser dataReaderToTrackedUser(SqlDataReader dataReader)
        {
            return new TrackedUser()
            {
                ID = dataReader["ID"].ToString().Trim().ToInt(),
                FirstName = dataReader["FirstName"].ToString().Trim(),
                LastName = dataReader["LastName"].ToString().Trim(),
                NavisionEmployeIDNumber = dataReader["NavisionID"].ToString().Trim(),
                LDAPUserName = dataReader["LDAPUserName"].ToString().Trim(),
                IsHidden = dataReader["IsHidden"].ToString().Trim().ToBool(),
                GroupIDs = dataReader["IsHidden"].ToString().Trim().ParseIDList(';'),
                Status = _userStatusRepo.GetStatusForUser(dataReader["ID"].ToString().Trim().ToInt())
            };
        }

        public List<TrackedUser> GetAll()
        {
            List<TrackedUser> returnMe = new List<TrackedUser>();

            using (SqlConnection connection = new SqlConnection(Settings.DBConnectionString))
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
                        TrackedUser u = dataReaderToTrackedUser(dbDataReader);
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

        public List<TrackedUser> GetByGroup(Group group)
        {
            return GetByGroup(group.ID);
        }

        public List<TrackedUser> GetByGroup(int groupID)
        {
            return GetAll().Where(u => u.GroupIDs.Contains(groupID)).ToList();
        }

        public TrackedUser Get(int ID)
        {
            using (SqlConnection connection = new SqlConnection(Settings.DBConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL + " WHERE Users.ID=@USERID;"
                };
                sqlCommand.Parameters.AddWithValue("USERID", ID);
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        TrackedUser u = dataReaderToTrackedUser(dbDataReader);
                        if (u != null)
                        {
                            return u;
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return NullUser();
        }

        public TrackedUser NullUser()
        {
            return new TrackedUser()
            {
                ID = 0,
                FirstName = "Unknown",
                LastName = "User",
                NavisionEmployeIDNumber = string.Empty,
                LDAPUserName = string.Empty,
                IsHidden = true,
                Groups = new List<Group>(),
            };
        }

    }
}