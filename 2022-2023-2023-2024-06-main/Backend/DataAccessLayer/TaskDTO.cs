using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskDTO
    {
        public const string BoardIdColumnName = "BoardId";
        public const string TaskIdColumnName = "TaskId";
        public const string TaskOrdinalColumnName = "ColumnOrdinal";
        public const string TaskAssigneeColumnName = "Assignee";
        public const string TaskTitleColumnName = "TaskTitle";
        public const string TaskDescriptionColumnName = "TaskDescription";
        public const string TaskCreationTimeColumnName = "TaskCreationTime";
        public const string TaskDueDateColumnName = "TaskDueDate";
        private TaskController _taskController;

        internal bool isPersisted { get; set; }
        private int _boardId;
        public int BoardId { get => _boardId; }
        private int _taskId;
        public int TaskId { get => _taskId; }


        private int _columnOrdinal;
        public int ColumnOrdinal { get => _columnOrdinal; set { _columnOrdinal = value; if(isPersisted) _taskController.Update(TaskId, BoardId, TaskOrdinalColumnName, value); } }

        private string _assignee;
        public string Assignee { get => _assignee; set { _assignee = value; if (isPersisted) _taskController.Update(TaskId,BoardId, TaskAssigneeColumnName, value); } }

        private string _taskTitle;
        public string TaskTitle { get => _taskTitle; set { _taskTitle = value; if (isPersisted) _taskController.Update(TaskId,BoardId, TaskTitleColumnName, value); } }

        private string _taskDescription;
        public string TaskDescription { get => _taskDescription; set { _taskDescription = value; if (isPersisted) _taskController.Update(TaskId, BoardId, TaskDescriptionColumnName, value); } }

        private DateTime _taskCreationTime;
        public DateTime TaskCreationTime { get => _taskCreationTime; }

        private DateTime _taskDueDate;
        public DateTime TaskDueDate { get => _taskDueDate; set { _taskDueDate = value; if (isPersisted) _taskController.Update(TaskId, BoardId, TaskDueDateColumnName, value.ToString()); } }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public TaskDTO(int TaskId, int BoardId, int ColumnOrdinal, string Assignee, string TaskTitle, string TaskDescription, DateTime TaskCreationTime, DateTime TaskDueDate)
        {
            _taskId = TaskId;
            _boardId = BoardId;
            _columnOrdinal = ColumnOrdinal;
            _assignee = Assignee;
            _taskTitle = TaskTitle;
            _taskDescription = TaskDescription;
            _taskCreationTime = TaskCreationTime;
            _taskDueDate = TaskDueDate;
            isPersisted = false;
            _taskController = new TaskController();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        /// <summary>
        /// Inserts task to tasks table
        /// </summary>
        /// <returns> boolean value whether the Insert action has succeeded </returns>
        public bool Persist()
        {
            if (_taskController.Insert(this))
            {
                isPersisted = true;
                log.Info($"Succesfully inserted task to 'Tasks' database. id:'{TaskId}', which is in board:'{BoardId}'");
                return true;
            }
            throw new Exception($"error when try to Insert task to 'Tasks' database. id:'{TaskId}', which is in board:'{BoardId}'");
        }
        /// <summary>
        /// Deletes task from tasks table
        /// </summary>
        /// <returns> boolean value whether the Delete action has succeeded </returns>
        public bool Delete()
        {
            if (_taskController.Delete(this))
            {
                isPersisted = false;
                log.Info($"Succesfully deleted from 'Tasks' database the task with the id:'{TaskId}', which is in board:'{BoardId}'");
                return true;
            }
            log.Error($"Deletion failed for task with the id:{TaskId}, which is in board '{BoardId}'");
            throw new Exception($"Deletion failed for task with the id:'{TaskId}', which is in board:'{BoardId}'");
        }
    }
}
