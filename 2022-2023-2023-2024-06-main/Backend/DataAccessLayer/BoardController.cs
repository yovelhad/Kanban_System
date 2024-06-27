using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardController
    {
        private const string BoardTableName = "Board";
        private readonly string _connectionString;
        private readonly string _tableName;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BoardController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = BoardTableName;
        }
        /// <summary>
        /// Inserts board to the boards table
        /// </summary>
        /// <param name="board"></param>
        /// <returns> boolean value whether the Insert action has succeeded </returns>
        internal bool Insert(BoardDTO board) 
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {

                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName} ({BoardDTO.boardNameColumnName} ,{BoardDTO.boardIdColumnName},{BoardDTO.ownerEmailColumnName}) " +
                        $"VALUES (@boardNameVal,@boardIdVal,@ownerEmailVal);";

                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", board.BoardId);
                    SQLiteParameter boardNameParam = new SQLiteParameter(@"boardNameVal", board.BoardName);
                    SQLiteParameter ownerEmailParam = new SQLiteParameter(@"ownerEmailVal", board.ownerEmail);

                    command.Parameters.Add(boardNameParam);
                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(ownerEmailParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully inserted {board} into the data base.");
                }
                catch (Exception e)
                {
                    log.Error($"failed to Insert {board} into the data base.");
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
        /// <summary>
        /// Deletes board from the boards table
        /// </summary>
        /// <param name="board"></param>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        internal bool Delete(BoardDTO board) {

            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from {_tableName} where BoardId={board.BoardId}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succssesfully Delete {board}");
                }
                catch (Exception e)
                {
                    log.Error($"Faild to Delete {board} , cause {e}");
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        /// <summary>
        /// Updates a board's name/ owner email
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns> boolean value whether the update action has succeeded </returns>
        /// <exception cref="ArgumentException"></exception>
        public bool Update(int boardId, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where BoardId={boardId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully updated {attributeName} to {attributeValue}");
                }
                catch (Exception e)
                {
                    log.Error($"failed to update {attributeName} to {attributeValue}");
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        /// <summary>
        /// Updates board id in a specific board
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns> boolean value whether the update action has succeeded </returns>
        /// <exception cref="ArgumentException"></exception>
        public bool Update(int boardId, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where BoardId={boardId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeName));
                    connection.Open();
                    res =  command.ExecuteNonQuery();
                    log.Info($"Succesfully updated {attributeName} to {attributeValue}");
                }
                catch (Exception e)
                {
                    log.Error($"failed to update {attributeName} to {attributeValue}");
                    throw new ArgumentException(e.ToString());
                }
            
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }
        /// <summary>
        /// Loads all boards from boards table
        /// </summary>
        /// <returns> List of BoardDTO's from boards table </returns>
        internal List<BoardDTO> LoadAllBoards() 
        {
            List<BoardDTO> results = new List<BoardDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                log.Info("Connection to db was done.");
                command.CommandText = $"SELECT * FROM {_tableName}";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    log.Info("Selecting boards from database.");
                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                }
                finally
                {
                    if (dataReader != null)
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
        /// Deletes all boards from boards table
        /// </summary>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        internal bool DeleteAllBoards()
        {

            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteDataReader dataReader = null;
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from {_tableName}"
                };
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    if (!dataReader.HasRows)
                        return true;
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }
        /// <summary>
        /// Converts the SQLite reader to board object user
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> BoardDTO converted object instance </returns>
        public BoardDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new BoardDTO((int)reader.GetInt32(1),reader.GetString(0),reader.GetString(2));
        }
    }
}
