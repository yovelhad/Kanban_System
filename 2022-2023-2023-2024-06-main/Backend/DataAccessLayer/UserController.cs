using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.SQLite;
using System.Text;
using log4net;
using System.Threading.Tasks;
using System.Reflection;
using IntroSE.Kanban.Backend.BusinessLayer;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserController
    {
        private const string UserTableName = "User";
        private readonly string _connectionString;
        private readonly string _tableName;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public UserController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = UserTableName;
        }
        /// <summary>
        /// Loads all users from the users table
        /// </summary>
        /// <returns> List of UserDTO's from the whole users table </returns>
        public List<UserDTO> LoadAllUsers()
        {
            List<UserDTO> results = new List<UserDTO>();
            using(var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                log.Info("Connection to db was done.");
                command.CommandText = $"SELECT * FROM {_tableName}";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    log.Info("Selecting users from database.");
                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                    log.Info("Succesfully loaded all users.");
                }
                catch (Exception e)
                {
                    log.Error("Load failed.");
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    if(dataReader != null)
                    {
                        dataReader.Close();
                    }
                    command.Dispose();
                    connection.Close();
                    log.Info("Connection to database has been closed.");
                }
            }
            return results;
        }
        /// <summary>
        /// Inserts user to the users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns> boolean value whether the Insert action has succeeded </returns>
        public bool Insert(UserDTO user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    log.Info("Connection to db was done.");
                    command.CommandText = $"INSERT INTO {UserTableName} ('{UserDTO.EmailColumnName}', {UserDTO.PasswordColumnName},{UserDTO.isLoggedColumnName}) " + $"VALUES (@emailVal,@passwordVal,@isloggedVal);";                
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password);
                    SQLiteParameter isloggedParam = new SQLiteParameter(@"isloggedVal", user.isLogged);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Parameters.Add(isloggedParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    log.Info("Inserting user to database.");
                }
                catch (Exception ex)
                {
                    log.Debug(ex.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    log.Info("Connection to database has been closed.");
                }
                return res > 0;
            }
        }
        /// <summary>
        /// Updates the the users state whether he's logged in or not
        /// </summary>
        /// <param name="email"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns> boolean value whether the update action has succeeded </returns>
        public bool Update(string email, string isLoggedInColumn, int attributeValue)
        {   
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"UPDATE {UserTableName} set [{isLoggedInColumn}]={attributeValue} where {UserDTO.EmailColumnName}='{email}'";
                log.Info("Connection to db was done.");
                try
                {
                    command.Parameters.Add(new SQLiteParameter(isLoggedInColumn, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("Updating user's fields to database.");
                }
                catch (Exception ex)
                {
                    log.Debug(ex.ToString());
                    throw new ArgumentException(ex.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    log.Info("Connection to database has been closed.");
                }
                return res > 0;
            }
        }
        /// <summary>
        /// Deletes user from the users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        public bool Delete(UserDTO user)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE Email='{user.Email}'"
                };
                log.Info("Connection to db was done.");
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("Deleting user from database.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    log.Info("Connection to database has been closed.");
                }
            }
            return res > 0;
        }
        /// <summary>
        /// Deletes all users from the users table
        /// </summary>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        public bool DeleteAllUsers()
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName}"
                };
                log.Info("Connection to db was done.");
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("Deleting all the users from database.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    log.Info("Connection to database has been closed.");
                }
            }
            return res > 0;
        }
        /// <summary>
        /// Converts the SQLite reader to user object user
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> UserDTO converted object instance </returns>
        public UserDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            log.Info("Converting the row to an object.");
            return new UserDTO(reader.GetString(0),reader.GetString(1),reader.GetInt32(2));
        }
    }
}