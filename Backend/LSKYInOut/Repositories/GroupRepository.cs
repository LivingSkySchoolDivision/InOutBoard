using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSKYInOut
{
    public class GroupRepository
    {
        private const string SQL = "SELECT * FROM Groups";

        private Dictionary<int, Group> _cache = new Dictionary<int, Group>();


        private Group dataReaderToGroup(SqlDataReader dataReader)
        {
            return new Group()
            {
                ID = dataReader["ID"].ToString().Trim().ToInt(),
                Name = dataReader["Name"].ToString().Trim(),
                IsHidden = dataReader["Hidden"].ToString().Trim().ToBool(),
                IsSystemAdministrator = dataReader["IsSystemAdministrator"].ToString().Trim().ToBool()
            };
        }

        public GroupRepository()
        {
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
                        Group g = dataReaderToGroup(dbDataReader);
                        if (g != null)
                        {
                            _cache.Add(g.ID, g);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }
        }

        public Group Get(int ID)
        {
            if (_cache.ContainsKey(ID))
            {
                return _cache[ID];
            }
            return null;
        }

        public List<Group> GetAll()
        {
            return _cache.Values.OrderBy(x => x.Name).ToList();
        }

        public List<Group> GetAllVisible()
        {
            return GetAll().Where(x => x.IsHidden == false).ToList();
        }

        public List<Group> Get(List<int> IDs)
        {
            List<Group> returnMe = new List<Group>();

            foreach(int id in IDs)
            {
                if (_cache.ContainsKey(id))
                {
                    returnMe.Add(_cache[id]);
                }
            }

            return returnMe;
        }

    }
}