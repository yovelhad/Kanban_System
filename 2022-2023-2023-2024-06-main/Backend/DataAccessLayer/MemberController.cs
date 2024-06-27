using System;
using System.Data.SQLite;
using System.Collections.Generic;
using log4net;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class MemberController
    {
        private const string MemberTableName = "Member";
        private readonly string _connectionString;
        private readonly string _tableName;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MemberController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = MemberTableName;
        }
        /// <summary>
        /// Inserts member to the board members table
        /// </summary>
        /// <param name="memberDTO"></param>
        /// <returns> boolean value whether the Insert action has succeeded </returns>
        public bool Insert(MemberDTO memberDTO)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    log.Info("Connection to db was done.");
                    command.CommandText = $"INSERT INTO {MemberTableName} ('{MemberDTO.EmailColumnName}', {MemberDTO.BoardIDColumnName})" + $"VALUES (@emailVal,@boardIdVal);";
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", memberDTO.Email);
                    SQLiteParameter boardIDParam = new SQLiteParameter(@"boardIdVal", memberDTO.BoardID);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(boardIDParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    log.Info("Inserting member to database.");
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
        /// Deletes member from the board members table
        /// </summary>
        /// <param name="memberDTO"></param>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        public bool Delete(MemberDTO memberDTO)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE {MemberDTO.EmailColumnName}='{memberDTO.Email}' AND {MemberDTO.BoardIDColumnName}={memberDTO.BoardID};"
                };
                log.Info("Connection to db was done.");
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("Deleting member from database.");
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
        /// Loads all memebers from a specific board
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns> List of MemberDTO's from the specific board </returns>
        public List<MemberDTO> LoadBoardMembers(int boardID)
        {
            List<MemberDTO> results = new List<MemberDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT '{MemberDTO.EmailColumnName}' FROM {_tableName} WHERE {MemberDTO.BoardIDColumnName}={boardID};";
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();
                    log.Info("Connection to db was done.");
                    reader = command.ExecuteReader();
                    log.Info($"Selecting members from board ID {boardID} to database.");
                    while (reader.Read())
                    {
                        results.Add(ConvertReaderToObject(reader));
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        log.Info("reader has been closed.");
                    }
                    command.Dispose();
                    connection.Close();
                    log.Info("Connection to database has been closed.");
                }
            }
            return results;
        }
        /// <summary>
        /// Loads all members from the members table
        /// </summary>
        /// <returns> List of MemberDTO's from all boards </returns>
        public List<MemberDTO> LoadBoardMembers()
        {
            List<MemberDTO> results = new List<MemberDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {_tableName};";
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();
                    log.Info("Connection to db was done.");
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        results.Add(ConvertReaderToObject(reader));
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        log.Info("reader has been closed.");
                    }
                    command.Dispose();
                    connection.Close();
                    log.Info("Connection to database has been closed.");
                }
            }
            return results;
        }
        /// <summary>
        /// Deletes all memebers from a members table
        /// </summary>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        public bool DeleteAllMembers()
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
                    log.Info("Deleting all the members from database.");
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
        /// Deletes all memebers from a specific board
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        public bool DeleteAllMembersInBoard(int boardID)
        {
            bool ans = false;
            List<MemberDTO> members = LoadBoardMembers(boardID);
            foreach(MemberDTO member in members)
            {
                ans = this.Delete(member);
            }
            log.Info("All members were deleted from db");
            return ans;
        }
        /// <summary>
        /// converts the SQLite reader to member object user
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> MemberDTO converted object instance</returns>
        public MemberDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            log.Info("Converting the row to an object.");
            return new MemberDTO(reader.GetString(0),(int)reader.GetInt32(1));
        }
    }
}
