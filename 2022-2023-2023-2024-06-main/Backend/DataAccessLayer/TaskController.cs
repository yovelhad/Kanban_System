using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskController
    {
        private const string TaskTableName = "Tasks";
        private readonly string _connectionString;
        private readonly string _tableName;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public TaskController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            _connectionString = $"Data Source={path}; Version=3;";
            _tableName = TaskTableName;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        /// <summary>
        /// Deletes all tasks from tasks table
        /// </summary>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        public bool DeleteAllTasks()
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
        /// Loads all tasks from a specific column in a specific board
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns> List of TaskDTO's in a specific column </returns>
        /// <exception cref="ArgumentException"></exception>
        public List<TaskDTO> LoadColumnTasks(int boardId, int columnOrdinal)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} where {TaskDTO.BoardIdColumnName} = {boardId} and {TaskDTO.TaskOrdinalColumnName} = {columnOrdinal} ;";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                    log.Info($"Succesfully loaded to tasks to column {columnOrdinal} in board {boardId}");
                }
                catch (Exception e)
                {
                    log.Error($"Load failed for tasks to column {columnOrdinal} in board {boardId}");
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
        /// Deletes task from the tasks table
        /// </summary>
        /// <param name="taskDTO"></param>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        internal bool Delete(TaskDTO taskDTO)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from {_tableName} where TaskId={taskDTO.TaskId} AND BoardId={taskDTO.BoardId}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succssesfully Delete {taskDTO}");
                }
                catch (Exception e)
                {
                    log.Error($"Faild to Delete {taskDTO} , cause {e}");
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
        /// Deletes all tasks in a specific board column
        /// </summary>
        /// <param name="ordinal"></param>
        /// <param name="boardId"></param>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        /// <exception cref="ArgumentException"></exception>
        internal bool RemoveColumnTasks(int ordinal, int boardId)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from {_tableName} where BoardId={boardId} and ColumnOrdinal={ordinal}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succssesfully Delete");
                }
                catch (Exception e)
                {
                    log.Error($"Faild to Delete, cause {e}");
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
        /// Inserts task to the tasks table
        /// </summary>
        /// <param name="taskDTO"></param>
        /// <returns> boolean value whether the Insert action has succeeded </returns>
        internal bool Insert(TaskDTO taskDTO)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({TaskDTO.TaskIdColumnName},{TaskDTO.BoardIdColumnName},{TaskDTO.TaskOrdinalColumnName},{TaskDTO.TaskAssigneeColumnName},{TaskDTO.TaskTitleColumnName},{TaskDTO.TaskDescriptionColumnName},{TaskDTO.TaskCreationTimeColumnName},{TaskDTO.TaskDueDateColumnName}) " +
                        $"VALUES (@taskIdVal,@boardIdVal,@columnOrdinalVal,@assigneeVal,@titleVal,@descriptionVal,@creationTimeVal,@dueDateVal);";

                    SQLiteParameter taskIdParam = new SQLiteParameter(@"taskIdVal", taskDTO.TaskId);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", taskDTO.BoardId);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinalVal", taskDTO.ColumnOrdinal);
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assigneeVal", taskDTO.Assignee);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", taskDTO.TaskTitle);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", taskDTO.TaskDescription);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTimeVal", taskDTO.TaskCreationTime);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", taskDTO.TaskDueDate);

                    command.Parameters.Add(taskIdParam);
                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(columnOrdinalParam);
                    command.Parameters.Add(assigneeParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(dueDateParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully inserted to database the task of the board. TaskId:'{taskDTO.TaskId}', BoardId:'{taskDTO.BoardId}'");
                }
                catch (Exception e)
                {
                    log.Error($"Insertion failed for task of the board. TaskId:'{taskDTO.TaskId}', BoardId:'{taskDTO.BoardId}'. cause: {e}");
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
        /// Updates a task's column ordinal number from a specific board
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="boardId"></param>
        /// <param name="taskOrdinalColumn"></param>
        /// <param name="value"></param>
        /// <returns> boolean value whether the update action has succeeded </returns>
        /// <exception cref="ArgumentException"></exception>
        internal bool Update(int taskId, int boardId, string taskOrdinalColumn, int value)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"UPDATE {TaskTableName} set [{taskOrdinalColumn}]={value} where {TaskDTO.TaskIdColumnName}={taskId} AND {TaskDTO.BoardIdColumnName} = {boardId}";
                try
                {
                    command.Parameters.Add(new SQLiteParameter(taskOrdinalColumn, value));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully updated the tasks database for task {taskId} in column {taskOrdinalColumn} with the value {value}");
                }
                catch (Exception e)
                {
                    log.Error($"Update failed for task {taskId} in column {taskOrdinalColumn} with the value {value}");
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
        /// Updates a task's description/ due date/ title/ asignee
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="boardId"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns> boolean value whether the update action has succeeded </returns>
        /// <exception cref="ArgumentException"></exception>
        internal bool Update(int taskId, int boardId, string attributeName, string attributeValue)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"UPDATE {TaskTableName} set [{attributeName}]=@{attributeName} " +
                    $"where {TaskDTO.TaskIdColumnName}={taskId} AND {TaskDTO.BoardIdColumnName}={boardId}";
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Succesfully updated the tasks database for task {taskId} in column {attributeName} with the value {attributeValue}");
                }
                catch (Exception e)
                {
                    log.Error($"Update failed for task {taskId} in column {attributeName} with the value {attributeValue}");
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
        /// Converts the SQLite reader to task object user
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> TaskDTO converted object instance </returns>
        public TaskDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new TaskDTO(reader.GetInt32(1), reader.GetInt32(0), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), DateTime.Parse(reader.GetString(6)), DateTime.Parse(reader.GetString(7)));
        }
    }
}
