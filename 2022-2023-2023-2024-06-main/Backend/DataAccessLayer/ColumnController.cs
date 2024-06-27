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
    internal class ColumnController
    {
        private const string ColumnTableName = "Column";
        private readonly string _connectionString;
        private readonly string _tableName;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ColumnController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = ColumnTableName;
        }
        /// <summary>
        /// inserts column to the columns table
        /// </summary>
        /// <param name="column"></param>
        /// <returns> boolean value whether the Insert action has succeeded </returns>
        internal bool Insert(ColumnDTO column)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} ({ColumnDTO.boardIdColumnName} ,{ColumnDTO.ColumnTitleColumnName},{ColumnDTO.ColumnOrdinalColumnName},{ColumnDTO.ColumnLimitColumnName}) " +
                        $"VALUES (@boardIdVal,@titleVal,@ordinalVal,@limitVal);";

                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", column.BoardId);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", column.Title);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"ordinalVal", column.ColumnOrdinal);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limitVal", column.Limit);

                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(ordinalParam);
                    command.Parameters.Add(limitParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully inserted {column} into the data base.");
                }
                catch (Exception e)
                {
                    log.Error($"failed to Insert {column} into the data base.");
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
        /// Deletes column from the columns table
        /// </summary>
        /// <param name="column"></param>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        internal bool Delete(ColumnDTO column)
        {

            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from {_tableName} where BoardId={column.BoardId} AND ColumnOrdinal={column.ColumnOrdinal} "
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succssesfully Delete {column}");
                }
                catch (Exception e)
                {
                    log.Error($"Faild to Delete {column} , cause {e}");
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
        /// Updates column title in a specific board
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns> boolean value whether the update action has succeeded </returns>
        /// <exception cref="ArgumentException"></exception>
        public bool Update(int boardId,int columnOrdinal, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Update {ColumnTableName} set [{attributeName}] = {attributeValue} " +
                    $"where {ColumnDTO.boardIdColumnName} ={boardId} AND {ColumnDTO.ColumnOrdinalColumnName} ={columnOrdinal}"
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
        /// Updates column's location (board id)/ ordinal number/ limit
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns> boolean value whether the update action has succeeded </returns>
        public bool Update(int boardId, int columnOrdinal, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Update {ColumnTableName} set [{attributeName}] = {attributeValue} " +
                    $"where {ColumnDTO.boardIdColumnName} ={boardId} AND {ColumnDTO.ColumnOrdinalColumnName} ={columnOrdinal}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully updated {attributeName} to {attributeValue}");
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
        /// Loads all columns from a specific board
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns> List of ColumnDTO's from a specific board </returns>
        /// <exception cref="ArgumentException"></exception>
        internal List<ColumnDTO> LoadBoardColumns(int boardId)
        {
            
            List<ColumnDTO> results = new List<ColumnDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} where {ColumnDTO.boardIdColumnName} = {boardId};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                    log.Info($"Succesfully loaded to columns which are in board {boardId}");
                }
                catch (Exception e)
                {
                    log.Error($"Load failed for columns which are in board {boardId}");
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
            
        }
        /// <summary>
        /// Deletes all coloumns from columns table
        /// </summary>
        /// <returns></returns>
        public bool DeleteAllColumns()
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
        /// Converts the SQLite reader to column object user
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> ColumnDTO converted object instance </returns>
        public ColumnDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new ColumnDTO((int)reader.GetInt32(1),reader.GetString(0), (int)reader.GetInt32(3),(int)reader.GetInt32(2));
        }

    }
}
