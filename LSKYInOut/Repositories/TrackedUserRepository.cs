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
        private GroupRepository _groupRepo;

        public TrackedUserRepository()
        {
            _userStatusRepo = new UserStatusRepository();
            _groupRepo = new GroupRepository();
        }

        private TrackedUser dataReaderToTrackedUser(SqlDataReader dataReader)
        {
            TrackedUser returnMe = new TrackedUser()
            {
                ID = dataReader["ID"].ToString().Trim().ToInt(),
                FirstName = dataReader["FirstName"].ToString().Trim(),
                LastName = dataReader["LastName"].ToString().Trim(),
                NavisionEmployeIDNumber = dataReader["NavisionID"].ToString().Trim(),
                LDAPUserName = dataReader["LDAPUserName"].ToString().Trim(),
                IsHidden = dataReader["IsHidden"].ToString().Trim().ToBool(),
                GroupIDs = dataReader["GroupMemberships"].ToString().Trim().ParseIDList(';'),
                Statuses = _userStatusRepo.GetStatusForUser(dataReader["ID"].ToString().Trim().ToInt())
            };

            returnMe.Groups = _groupRepo.Get(returnMe.GroupIDs);

            return returnMe;
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

            return returnMe.OrderBy(u => u.DisplayName).ToList();
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

        public UserStatus UpdateUserStatus(UserStatus newUserStatus)
        {
            using (SqlConnection connection = new SqlConnection(Settings.DBConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "DELETE FROM UserStatuses WHERE Expires<GETDATE();INSERT INTO UserStatuses(UserID,StatusID,Expires,thumbprint) VALUES(@USERID,@STATUSID,@EXPIRYDATE,@THUMB);"
                };
                sqlCommand.Parameters.AddWithValue("USERID", newUserStatus.UserID);
                sqlCommand.Parameters.AddWithValue("STATUSID", newUserStatus.Status.ID);
                sqlCommand.Parameters.AddWithValue("EXPIRYDATE", newUserStatus.Expires);
                sqlCommand.Parameters.AddWithValue("THUMB", newUserStatus.Thumbprint);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }

            return newUserStatus;
        }

        public UserStatus UpdateUserStatus(TrackedUser user, Status status, DateTime expiry)
        {
            UserStatus newUserStatus = new UserStatus()
            {
                UserID = user.ID,
                Status = status,
                Expires = expiry,
                Thumbprint = Helpers.Crypto.SHA256(DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString() + user.DisplayName + status.Name + expiry.ToLongDateString() + expiry.ToLongTimeString())
            };

            return UpdateUserStatus(newUserStatus);           
        }

        public UserStatus UpdateUserStatus(TrackedUser user, Status status, FriendlyTimeSpan expiry)
        {
            DateTime expiryDate = DateTime.Now.Add(expiry.TimeSpan);
            if (expiryDate <= DateTime.Now) { expiryDate.AddMinutes(1); }
            return UpdateUserStatus(user, status, expiryDate);
        }

        public void ClearUserStatus(TrackedUser user)
        {
            using (SqlConnection connection = new SqlConnection(Settings.DBConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "DELETE FROM UserStatuses WHERE Expires<GETDATE();DELETE FROM UserStatuses WHERE UserID=@USERID;"
                };
                sqlCommand.Parameters.AddWithValue("USERID", user.ID);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
            }
        }

    }
}