using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSKYInOut
{
    public class StatusRepository
    {
        private readonly Dictionary<int, Status> _cache;

        public StatusRepository()
        {
            _cache = new Dictionary<int, Status>();

            using (SqlConnection connection = new SqlConnection(Settings.DBConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM Statuses;"
                };
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        Status s = dataReaderToUserStatus(dbDataReader);
                        if (s != null)
                        {
                            _cache.Add(s.ID, s);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }            
        }

        private Status dataReaderToUserStatus(SqlDataReader dataReader)
        {
            return new Status()
            {
                ID = dataReader["ID"].ToString().Trim().ToInt(),
                Name = dataReader["Name"].ToString().Trim(),
                Color = dataReader["Color"].ToString().Trim(),
                IsInOffice = dataReader["IsInOffice"].ToString().Trim().ToBool(),
                IsAtWork = dataReader["IsAtWork"].ToString().Trim().ToBool(),
                SortOrder = dataReader["SortOrder"].ToString().Trim().ToInt(),
                IsBusy = dataReader["IsBusy"].ToString().Trim().ToBool()
            };
        }

        private Status NullStatus()
        {
            return new Status()
            {
                ID = 0,
                Name = "",
                Color = "",
                IsInOffice = false,
                IsAtWork = false,
                SortOrder = 0
            };
        }

        public List<Status> GetAll()
        {
            return _cache.Values.OrderBy( s => s.SortOrder).ThenBy( s => s.Name).ToList();
        }
        
        public Status Get(int ID)
        {
            if (_cache.ContainsKey(ID))
            {
                return _cache[ID];
            } else
            {
                return NullStatus();
            }
        }
    }
}