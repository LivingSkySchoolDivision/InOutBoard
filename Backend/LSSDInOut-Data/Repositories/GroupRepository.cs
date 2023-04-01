using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using LSSD.InOut.Lib.Model;
using LSSD.InOut.Lib.Extensions;

namespace LSSD.InOut.Data.Repositories
{
    public class GroupRepository
    {
        private readonly string _dbConnectionString;
        private const string SQL = "SELECT * FROM Groups";

        public GroupRepository(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        private Group dataReaderToGroup(SqlDataReader dataReader)
        {
            return new Group()
            {
                ID = dataReader["ID"].ToString().Trim().ToInt(),
                Name = dataReader["Name"].ToString().Trim(),
                IsHidden = dataReader["Hidden"].ToString().Trim().ToBool()
            };
        }

        public List<Group> GetAll()
        {
            List<Group> returnMe = new List<Group>();

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL + " WHERE Hidden=0"
                })
                {
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            Group g = dataReaderToGroup(dbDataReader);
                            if (g != null)
                            {
                                returnMe.Add(g);
                            }
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return returnMe.OrderBy(g => g.Name).ToList();
        }

        public Group Get(int groupID)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL + " WHERE ID=@ID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("ID", groupID);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            Group g = dataReaderToGroup(dbDataReader);
                            if (g != null)
                            {
                                return g;
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }

            return null;
        }

        public void Update(Group group) 
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "UPDATE Groups SET Name=@GRPNAME WHERE ID=@GRPID;"
                })
                {
                    sqlCommand.Parameters.AddWithValue("GRPID", group.ID);
                    sqlCommand.Parameters.AddWithValue("GRPNAME", group.Name);

                    sqlCommand.Connection.Open();                    
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        public void Delete(Group group) 
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "DELETE FROM Groups WHERE Groups.ID=@GRPID;"
                })
                {
                    sqlCommand.Parameters.AddWithValue("GRPID", group.ID);
                    sqlCommand.Connection.Open();                    
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

    }
}