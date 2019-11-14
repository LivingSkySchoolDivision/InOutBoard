using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using LSSD.InOut.Lib.Model;
using LSSD.InOut.Lib.Extensions;

namespace LSSD.InOut.Data.Repositories
{
    public class PersonRepository
    {
        private readonly string _dbConnectionString;
        private const string SQL = "SELECT * FROM People";

        private readonly StatusRepository _statusRepository;

        public PersonRepository(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
            _statusRepository = new StatusRepository(dbConnectionString);
        }

        private Person dataReaderToPerson(SqlDataReader dataReader)
        {
            // Parse group IDs
            List<string> groupIDString = dataReader["GroupMemberships"].ToString().Trim().Split(';').ToList();
            List<int> groupIDs = new List<int>();
            foreach (string grpIDString in groupIDString)
            {
                int grpID = grpIDString.ToInt();
                if (grpID > 0)
                {
                    if (!groupIDs.Contains(grpID))
                    {
                        groupIDs.Add(grpID);
                    }
                }
            }

            int personID = dataReader["ID"].ToString().Trim().ToInt();
            return new Person()
            {
                ID = personID,
                FirstName = dataReader["FirstName"].ToString().Trim(),
                LastName = dataReader["LastName"].ToString().Trim(),
                IsEnabled = dataReader["IsEnabled"].ToString().Trim().ToBool(),
                GroupsIDs = groupIDs,
                CurrentStatus = _statusRepository.GetActiveForPerson(personID)
            };
        }


        public List<Person> GetEnabled()
        {
            return GetAll().Where(x => x.IsEnabled == true).ToList();
        }

        public List<Person> GetAll()
        {
            List<Person> returnMe = new List<Person>();

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL
                })
                {
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            Person u = dataReaderToPerson(dbDataReader);
                            if (u != null)
                            {
                                returnMe.Add(u);
                            }
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return returnMe.OrderBy(u => u.FirstName).ThenBy(u => u.LastName).ToList();
        }


        public Person Get(int ID)
        {
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL + " WHERE People.ID=@USERID;"
                })
                {
                    sqlCommand.Parameters.AddWithValue("USERID", ID);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            Person u = dataReaderToPerson(dbDataReader);
                            if (u != null)
                            {
                                return u;
                            }
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return null;
        }

        public List<Person> GetGroupMembers(int groupID)
        {
            List<Person> returnMe = new List<Person>();

            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQL + " WHERE GroupMemberships LIKE '%" + groupID + "%'"
                })
                {
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            Person u = dataReaderToPerson(dbDataReader);
                            if (u != null)
                            {
                                if (u.GroupsIDs.Contains(groupID))
                                {
                                    returnMe.Add(u);
                                }
                            }
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return returnMe.OrderBy(u => u.FirstName).ThenBy(u => u.LastName).ToList();
        }
    }
}